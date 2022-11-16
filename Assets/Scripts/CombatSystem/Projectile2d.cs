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
		[SerializeField] private LayerMask _collisionMask;
		private Damage _damage;
		private Vector3 _direction;

		private void Awake() {
			_damage = gameObject.GetComponent<Damage>();
			if (_damage == null) {
				Debug.LogError("Damage not found on " + name);
			}
		}

		private void FixedUpdate() {
			gameObject.transform.Translate(_direction.normalized * _velocity * Time.fixedDeltaTime);
		}

		private void OnCollisionEnter2D(Collision2D collision) {
			if (collision.collider.gameObject.layer.Equals(_collisionMask) == false) {
				return;
			}

			if (_hitEffect != null) {
				Instantiate(_hitEffect, gameObject.transform.position, Quaternion.identity, null);
			}

			IHealth health = collision.gameObject.GetComponent<IHealth>();
			if (health != null) {
				// Do damage
				health.TakeDamage(_damage);
			}

			IRecoil recoil = collision.collider.GetComponent<IRecoil>();
			if (recoil != null) {
				float damageDirection = transform.position.x < collision.transform.position.x ? 1 : -1;
				//	recoil.ApplyDamageForce(_damage.DamageForce, damageDirection);
			}
		}

		public Vector3 Direction
        {
            get { return _direction; }
            set
            {
				if(_direction != value)
                {
					_direction = value;
                }
            }
        }
	}
}
