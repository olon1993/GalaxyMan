using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class Collectible : PhysicsObject2D
    {
        [SerializeField] private string _weaponName;
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
			
			if (pickupEffect != null) {
				GameObject tmp = Instantiate(pickupEffect, transform.position, Quaternion.identity, null) as GameObject;
				Destroy(tmp, 5f);
				Destroy(this.gameObject, 0.1f);
			}
			foreach (IWeapon weapon in col.GetComponent<ICombatant>().MainWeapons) {
				if (weapon.WeaponName == _weaponName) {
					weapon.UnlockWeapon();
					break;
				}
			}
			
		}
	}
}
