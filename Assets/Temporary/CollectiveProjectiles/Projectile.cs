using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class Projectile : MonoBehaviour
    {


		[SerializeField] GameObject _hitEffect;
		private IDamage _damage;
		private bool active;
		public string ownerTag;

		private void Awake() {
			_damage = GetComponent<IDamage>();
			active = true;
		}

		private void OnTriggerEnter2D(Collider2D col) {
			if (!active) {
				return;
			}
			if (col.CompareTag(ownerTag)) {
				return;
			}
			if (_hitEffect != null) {
				Instantiate(_hitEffect, gameObject.transform.position, Quaternion.identity, null);
			}
			IHealth hpScript = col.GetComponent<IHealth>();
			if (hpScript != null) {
				// Do damage
				hpScript.TakeDamage(_damage);
				HandleDamageForce(col);
			}
			if (active) {
				Deactivate();
			}
		}

		private void HandleDamageForce(Collider2D col) {
			float damageDirection = transform.position.x < col.transform.position.x ? 1 : -1;

		}

		private void Deactivate() {
			active = false;
			gameObject.SetActive(false);
		}

	}
}
