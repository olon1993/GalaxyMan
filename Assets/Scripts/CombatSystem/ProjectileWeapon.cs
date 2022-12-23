using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class ProjectileWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] protected bool _showDebugLog = false;
		[SerializeField] protected string _weaponName;
        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        [SerializeField] private Transform _pointOfOrigin;
        [SerializeField] private Transform _pointOfTargetting;

        [SerializeField] GameObject DefaultProjectile;
        [SerializeField] GameObject ChargingParticleSystem;

        [SerializeField] private float _attackSpeed = 0.1f;

        [SerializeField] private float[] _chargedShotTime;
        [SerializeField] private GameObject[] _chargeProjectile;
        [SerializeField] private int[] _chargeAmmoCost;

        [SerializeField] private bool _isLimitedAmmo;
        [SerializeField] private int _maxAmmo;
        [SerializeField] private int _currentAmmo;
        [SerializeField] private IWeapon.AmmoType _ammoTypeDefinition;
		[SerializeField] private IWeapon.WeaponType _weaponTypeDefinition = IWeapon.WeaponType.RANGED;
		[SerializeField] private int _animationLayer;
        [SerializeField] private string _owner;

        [SerializeField] protected bool _isUnlocked = false;
        // Minimum interval variables
        private bool _isAttacking;
        private bool _isQueuedAttack;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        public void ChargeEffect() {
            if (ChargingParticleSystem != null) {
                ParticleSystem ps = ChargingParticleSystem.GetComponent<ParticleSystem>();
                    ps.Play();
			}
		}

        public void Attack(float chargeTime) {

            if (_showDebugLog) {
                Debug.Log("Attacking with " + name);
            }
            if (!_isAttacking) {
                StartCoroutine(HandleAttack(chargeTime));
            } else if (!_isQueuedAttack) {
                _isQueuedAttack = true;
            }
        }

        private IEnumerator HandleAttack(float chargeTime) {
            _isAttacking = true;
            if (ChargingParticleSystem != null) {
                ParticleSystem ps = ChargingParticleSystem.GetComponent<ParticleSystem>();
                ps.Clear();
                ps.Stop();
			}
            
            GameObject insProjectile = null;
            if (_chargedShotTime.Length == 0 || _chargeProjectile.Length == 0) {
                if (CheckAmmo(0)) {
                    insProjectile = Instantiate(DefaultProjectile, _pointOfOrigin.position, Quaternion.identity, null);
                } else {
                    Debug.LogWarning("Not enough ammo");
                    yield break;
                }
            } else {
                // Loop backwards to 0, not from 0 and up
                for (int i = _chargedShotTime.Length - 1; i > -1 ; i--) {
                    if (chargeTime > _chargedShotTime[i]) {
                        if (CheckAmmo(i)) {
                            insProjectile = Instantiate(_chargeProjectile[i], _pointOfOrigin.position, Quaternion.identity, null);
                            break;
                        } else {
                            Debug.LogWarning("Not enough ammo");
                            yield break;
                        }
                    }
                }
            }
            
            IProjectile projectile = insProjectile.GetComponent<IProjectile>();
            projectile.Setup(_pointOfOrigin.position, _pointOfTargetting.position, _owner);
            yield return new WaitForSeconds(_attackSpeed);
            _isAttacking = false;
            if (_isQueuedAttack) {
                _isQueuedAttack = false;
                StartCoroutine(HandleAttack(0.01f));
            }
        }

        private bool CheckAmmo(int i) {
            if (_isLimitedAmmo) {
                if (_chargeAmmoCost[i] > _currentAmmo) {
                    return false;
				} else {
                    _currentAmmo -= _chargeAmmoCost[i];
                }
            }
            return true;
		}

        public void UnlockWeapon() {
            _isUnlocked = true;
		}

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 direction = _pointOfTargetting.position - _pointOfOrigin.position;
            Gizmos.DrawRay(_pointOfOrigin.position, direction);
        }

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        public string WeaponName {
            get { return _weaponName; }
		}

        public bool IsLimitedAmmo
        {
            get { return _isLimitedAmmo; }
            set { _isLimitedAmmo = value; }
        }

        public int MaxAmmo
        {
            get { return _maxAmmo; }
            set { _maxAmmo = value; }
        }

        public int CurrentAmmo
        {
            get { return _currentAmmo; }
            set { _currentAmmo = value; }
        }

        public IWeapon.AmmoType AmmoTypeDefinition {
            get { return _ammoTypeDefinition; }
        }

        public IWeapon.WeaponType WeaponTypeDefinition {
            get { return _weaponTypeDefinition; }
        }

        public Transform PointOfOrigin
        {
            get { return _pointOfOrigin; }
            set { _pointOfOrigin = value; }
        }

        public Transform PointOfTargetting
        {
            get { return _pointOfTargetting; }
            set { _pointOfTargetting = value; }
        }

        public float AttackActionTime { get; }

		public int AnimationLayer {
			get { return _animationLayer; }
			set { _animationLayer = value; }
		}

        public float AttackSpeed { get { return _attackSpeed; } }
        public bool IsAttacking { get { return _isAttacking; } }

        public bool IsUnlocked { get { return _isUnlocked; } }
    }
}
