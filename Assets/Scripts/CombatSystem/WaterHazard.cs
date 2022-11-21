using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class WaterHazard : Hazard
    {
        [SerializeField] private Transform knockbackToLocation;
        protected override void OnTriggerEnter2D(Collider2D col) {
            if (_showDebugLog) {
                Debug.Log("Hazard hit :" + col.gameObject.name);
            }
            if (col.tag == null) {
                return;
            }
            if (col.CompareTag(_ignoreTag)) {
                return;
            }

            IHealth otherHealth = col.GetComponent<IHealth>();
            if (otherHealth != null) {
                otherHealth.TakeDamage(_damage);
            }
            if (col.CompareTag("Player")) {
                col.transform.position = knockbackToLocation.position;
			}
        }
    }
}
