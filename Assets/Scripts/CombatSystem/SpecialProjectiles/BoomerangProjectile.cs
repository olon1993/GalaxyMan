using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class BoomerangProjectile : MonoBehaviour, IProjectile
    {
		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		[SerializeField] protected bool _showDebugLog;
		[SerializeField] private float _velocity;
		[SerializeField] GameObject _child;
		[SerializeField] GameObject _hitEffect;
		private string _ownerTag;
		private IDamage _damage;
		private Vector3 _direction;
		private bool _active;
		private IWeapon _weapon;


		// Boomerang stuff
		[SerializeField] private float _timeBeforeReturn = 1.2f;
		private bool _canCollect = false;
		private GameObject owner;
		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		private void Awake() {
			owner = GameObject.FindGameObjectWithTag("Player");
			_damage = gameObject.GetComponent<IDamage>();
			if (_damage == null) {
				Debug.LogError(gameObject.name + " is not properly setup! Missing Damage!");
			}
			StartCoroutine(ReturnToSender());
		}

		private void OnTriggerEnter2D(Collider2D col) {
			if (!_active) {
				return;
			}
			if (col.CompareTag(_ownerTag)) {
				if (_canCollect) {
					RecollectAmmo();
					Deactivate();
				}
				return;
			}
			if (_hitEffect != null) {
				Instantiate(_hitEffect, gameObject.transform.position, Quaternion.identity, null);
			}
			IHealth hpScript = col.GetComponent<IHealth>();
			if (hpScript != null) {
				hpScript.TakeDamage(_damage);
			}
			IRecoil recoil = col.GetComponent<IRecoil>();
			if (recoil != null) {
				if (_showDebugLog) {
					Debug.Log("Recoil");
				}
				recoil.ApplyRecoil(_damage.KnockbackForce, gameObject.transform.position, _damage.StunTime);
			}
		}

		private void FixedUpdate() {
			gameObject.transform.Translate(_direction * Time.fixedDeltaTime);
		}

		private IEnumerator ReturnToSender() {
			yield return new WaitForSeconds(_timeBeforeReturn);
			_canCollect = true;
			while (_active) {
				Vector3 currentDirection = _direction;
				Vector3 newDirection = (owner.transform.position - transform.position).normalized * _velocity;
				_direction = Vector3.Lerp(currentDirection,newDirection, 0.02f);
				yield return new WaitForEndOfFrame();
			}
		}

		// Setup gives the target to go to and the rotation of the projectile renderer
		public void Setup(Vector3 start, Vector3 target, string owner) {
			_weapon = GameObject.FindGameObjectWithTag("Player").GetComponent<ICombatant>().CurrentMainWeapon;
			_damage = gameObject.GetComponent<IDamage>();
			if (_damage == null) {
				Debug.LogError("Damage not found on " + name);
			}
			_ownerTag = owner;
			_direction = (target - start).normalized * _velocity;
			_active = true;
			Destroy(gameObject, 10f);
		}

		private void RecollectAmmo() {
			if (_weapon.CurrentAmmo < _weapon.MaxAmmo) {
				_weapon.CurrentAmmo += 1;
			}
		}

		// turns off the projectile. Not destroyed here, as it is destroyed in main 
		// should it miss. calling destroy here as well will make destroy be called 
		// twice and creating null reference exception. hence, deactivate.
		private void Deactivate() {
			_active = false;
			gameObject.SetActive(false);
		}

		//**************************************************\\
		//******************* Properties *******************\\
		//**************************************************\\

		public IDamage Damage {
			get { return _damage; }
		}

		public bool Active {
			get { return _active; }
			set { _active = value; }
		}

		public Vector3 Direction {
			get { return _direction; }
		}

		public float Velocity {
			get { return _velocity; }

		}

		public string OwnerTag {
			get { return _ownerTag; }
			set {
				if (_ownerTag != value) {
					_ownerTag = value;
				}
			}
		}
	}
}
