using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
	[RequireComponent(typeof(Damage))]
	public class MeleeWeapon : MonoBehaviour, IMeleeWeapon
	{
		[SerializeField] protected bool _showDebugLog = false;

		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		// Dependencies
		private IDamage _damage;
		[SerializeField] private IWeapon.AmmoType _ammoTypeDefinition;
		[SerializeField] private IWeapon.WeaponType _weaponTypeDefinition = IWeapon.WeaponType.MELEE;

		[SerializeField] private bool _isLimitedAmmo;
		[SerializeField] private int _maxAmmo;
		[SerializeField] private int _currentAmmo;

		[SerializeField] protected Transform _pointOfOrigin;
		[SerializeField] protected Transform _pointOfTargetting;
		[SerializeField] protected float _radiusOfInteraction;

		[SerializeField] float _delayToHit = 0f;
		[SerializeField] float _attackActionTime = 0.1f;

		[SerializeField] private int _animationLayer;
		[SerializeField] private string _owner;

		[SerializeField] private GameObject _attackGraphics;
		// Sound
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private AudioClip[] _audioClips;

		private bool _is2D = true;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		private void Awake() {
			_damage = GetComponent<IDamage>();
			if (_damage == null) {
				Debug.LogError("Damage not found on " + gameObject.name);
			}
		}

		public void ChargeEffect() {

		}

		public void Attack(float charge) {
			if (_showDebugLog) {
				Debug.Log("Attacking with " + name);
			}

			// This should be broken out into a 2d and a 3d version of this mechanic
			if (_is2D) {
				Invoke("HandleDamage2D", _delayToHit);
			} else {
				HandleDamage();
			}
		}

		private void HandleDamage() {
			Collider[] colliders = Physics.OverlapSphere(_pointOfOrigin.position, _radiusOfInteraction);
			foreach (Collider col in colliders) {
				if (col.CompareTag(_owner)) {
					continue;
				}
				IHealth health = col.GetComponent<IHealth>();
				if (health != null) {
					health.TakeDamage(Damage);
				}

				IRecoil recoil = col.GetComponent<IRecoil>();
				if (recoil != null) {
					if (_showDebugLog) {
						Debug.Log("Recoil");
					}
					recoil.ApplyRecoil(_damage.KnockbackForce, _pointOfOrigin.position);
				}

				if (_showDebugLog) {
					Debug.Log(gameObject.name + " attacks dealing " + Damage.DamageAmount + " damage to " + col.gameObject.name + "!");

					Debug.Log(col.gameObject.name + " health = " + health.CurrentHealth + " / " + health.MaxHealth);

				}
			}
		}

		protected virtual void HandleDamage2D() {
			Collider2D[] colliders = Physics2D.OverlapCircleAll(_pointOfOrigin.position, _radiusOfInteraction);
			StartCoroutine(DisplayGraphics(_pointOfOrigin.position));
			if (colliders.Length > 0) {
				HandleSound();
			}
			foreach (Collider2D col in colliders) {

				if (col.CompareTag(_owner)) {
					continue;
				}
				IHealth health = col.GetComponent<IHealth>();
				if (health != null) {
					health.TakeDamage(Damage);
				}

				IRecoil recoil = col.GetComponent<IRecoil>();
				if (recoil != null) {
					if (_showDebugLog) {
						Debug.Log("Recoil");
					}
					recoil.ApplyRecoil(_damage.KnockbackForce, _pointOfOrigin.position);
				}

				if (_showDebugLog) {
					Debug.Log(gameObject.name + " attacks dealing " + Damage.DamageAmount + " damage to " + col.gameObject.name + "!");

					Debug.Log(col.gameObject.name + " health = " + health.CurrentHealth + " / " + health.MaxHealth);

				}
			}
		}

		private IEnumerator DisplayGraphics(Vector3 origin) {
			GameObject tmp = Instantiate(_attackGraphics, origin, Quaternion.identity,null) as GameObject;
			tmp.transform.localScale = new Vector3(AttackDirection ,1,1) * _radiusOfInteraction * 2;
			Destroy(tmp, AttackActionTime);
			yield return new WaitForEndOfFrame();

		}

		private void HandleSound() {
			if (_audioSource == null) {
				return;
			}

			if (_audioClips.Length > 1) {
				_audioSource.clip = _audioClips[Random.Range(0, _audioClips.Length)];
			}

			_audioSource.Play();
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(_pointOfOrigin.position, _radiusOfInteraction);
		}

		//**************************************************\\
		//******************* Properties *******************\\
		//**************************************************\\

		public IDamage Damage {
			get { return _damage; }
			set { _damage = value; }
		}

		public bool IsLimitedAmmo {
			get { return _isLimitedAmmo; }
			set { _isLimitedAmmo = value; }
		}

		public int MaxAmmo {
			get { return _maxAmmo; }
			set { _maxAmmo = value; }
		}

		public int CurrentAmmo {
			get { return _currentAmmo; }
			set { _currentAmmo = value; }
		}

		public IWeapon.AmmoType AmmoTypeDefinition {
			get { return _ammoTypeDefinition; }
		}

		public IWeapon.WeaponType WeaponTypeDefinition {
			get { return _weaponTypeDefinition; }
		}

		public Transform PointOfOrigin {
			get { return _pointOfOrigin; }
			set { _pointOfOrigin = value; }
		}

		public Transform PointOfTargetting {
			get { return _pointOfTargetting; }
			set { _pointOfTargetting = value; }
		}

		public float AttackRange { get { return _radiusOfInteraction; } }

		public int AnimationLayer {
			get { return _animationLayer; }
			set { _animationLayer = value; }
		}

		public float AttackActionTime {
			get { return _attackActionTime; }
		}

		public float AttackDirection {
			get { return Mathf.Sign(PointOfTargetting.position.x - PointOfOrigin.position.x); }
		}

        public float AttackCharge { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

	}
}
