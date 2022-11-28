using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana
{

	public class Projectile2d : MonoBehaviour
	{

		[SerializeField] private float _velocity;
		[SerializeField] private GameObject _hitEffect;
		[SerializeField] private string _ownerTag;
		private Damage _damage;
		private Vector3 _direction;
		private bool _active;

		private void Awake() {
			_damage = gameObject.GetComponent<Damage>();
			if (_damage == null) {
				Debug.LogError("Damage not found on " + name);
			}
			_active = true;
		}

		private void FixedUpdate() {
			gameObject.transform.Translate(_direction.normalized * _velocity * Time.fixedDeltaTime);
		}

		private void OnTriggerEnter2D(Collider2D col) {
			if (!_active) {
				return;
			}
			if (col.gameObject.CompareTag(_ownerTag)) {
				return;
			}

			if (_hitEffect != null) {
				Instantiate(_hitEffect, gameObject.transform.position, Quaternion.identity, null);
			}

			IHealth health = col.gameObject.GetComponent<IHealth>();
			if (health != null) {
				// Do damage
				health.TakeDamage(_damage);
			}

			IRecoil recoil = col.GetComponent<IRecoil>();
			if (recoil != null) {
				float damageDirection = transform.position.x < col.transform.position.x ? 1 : -1;
				//	recoil.ApplyDamageForce(_damage.DamageForce, damageDirection);
			}
			if (_active) {
				Deactivate();
			}
		}

		private void Deactivate() {
			_active = false;
			gameObject.SetActive(false);
		}

		public Vector3 Direction {
			get { return _direction; }
			set {
				if (_direction != value) {
					_direction = value;
				}
			}
		}
	}
}
