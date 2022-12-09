using UnityEngine;

namespace TheFrozenBanana
{
    public class ProjectileWeapon : MonoBehaviour, IWeapon
    {
        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        [SerializeField] private Transform _pointOfOrigin;
        [SerializeField] private Transform _pointOfTargetting;

        [SerializeField] GameObject DefaultProjectile;
        [SerializeField] GameObject ChargingParticleSystem;

        [SerializeField] private float[] _chargedShotTime;
        [SerializeField] private GameObject[] _chargeProjectile;
        [SerializeField] private int[] _chargeAmmoCost;

        [SerializeField] private bool _isLimitedAmmo;
        [SerializeField] private int _maxAmmo;
        [SerializeField] private int _currentAmmo;
        [SerializeField] private IWeapon.AmmoType _ammoType;
		[SerializeField] private int _animationLayer;
        [SerializeField] private string _owner;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        public void ChargeEffect() {
            if (ChargingParticleSystem != null) {
                ParticleSystem ps = ChargingParticleSystem.GetComponent<ParticleSystem>();
                    ps.Play();
				
			}
		}

        public void Attack(float chargeTime)
        {
            if (ChargingParticleSystem != null) {
                ParticleSystem ps = ChargingParticleSystem.GetComponent<ParticleSystem>();
                    ps.Stop();
			}
            
            GameObject insProjectile = null;
            if (_chargedShotTime.Length == 0 || _chargeProjectile.Length == 0) {
                insProjectile = Instantiate(DefaultProjectile, _pointOfOrigin.position, Quaternion.identity, null);
            } else {
                // Loop backwards to 0, not from 0 and up
                for (int i = _chargedShotTime.Length - 1; i > -1 ; i--) {
                    if (chargeTime > _chargedShotTime[i]) {
                        if (CheckAmmo(i)) {
                            insProjectile = Instantiate(_chargeProjectile[i], _pointOfOrigin.position, Quaternion.identity, null);
                            break;
                        } else {
                            Debug.LogWarning("Not enough ammo");
                            return;
						}
                    }
                }
            }
            
            IProjectile projectile = insProjectile.GetComponent<IProjectile>();
            projectile.Setup(_pointOfOrigin.position, _pointOfTargetting.position, _pointOfTargetting.rotation, _owner);
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

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 direction = _pointOfTargetting.position - _pointOfOrigin.position;
            Gizmos.DrawRay(_pointOfOrigin.position, direction);
        }

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

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

        public IWeapon.AmmoType AmmoTypeDefinition
        {
            get { return _ammoType; }
            set { _ammoType = value; }
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

        public float AttackCharge { get; set; }
    }
}
