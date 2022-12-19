using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana
{
	public class Collectable : PhysicsObject2D, ICollectable
	{
		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\
		[SerializeField] private ICollectable.CollectableType _collectableTypeDefinition;
		[SerializeField] private int pickupAmount;
		[SerializeField] private GameObject pickupEffect;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		private void OnTriggerEnter2D(Collider2D col) {
			if (_showDebugLog) {
				Debug.Log("Collectable Triggered by: " + col.name);
			}
			if (!col.CompareTag("Player")) {
				return;
			}
			if (_collectableTypeDefinition == ICollectable.CollectableType.AMMO) {
				IWeapon weapon = col.GetComponent<ICombatant>().CurrentMainWeapon;
				if (_showDebugLog) {
					Debug.Log("Collectable is Ammo.");
				}
				if (weapon.IsLimitedAmmo && weapon.CurrentAmmo < weapon.MaxAmmo) {
					int diff = weapon.MaxAmmo - weapon.CurrentAmmo;
					if (pickupAmount > diff) {
						weapon.CurrentAmmo += diff;
					} else {
						weapon.CurrentAmmo += pickupAmount;
					}
				}
			} else if (_collectableTypeDefinition == ICollectable.CollectableType.HEALTH) {
				if (_showDebugLog) {
					Debug.Log("Collectable is Health.");
				}
				IHealth health = col.GetComponent<IHealth>();
				if (health != null) {
					health.AddHealth(pickupAmount);
				}
			}
			if (pickupEffect != null) {
				GameObject tmp = Instantiate(pickupEffect, transform.position, Quaternion.identity, null) as GameObject;
				Destroy(tmp, 5f);
				Destroy(this.gameObject, 0.1f);
			}

		}

		//**************************************************\\
		//******************* Properties *******************\\
		//**************************************************\\

		public ICollectable.CollectableType CollectableTypeDefinition { get { return _collectableTypeDefinition; } }
	}
}
