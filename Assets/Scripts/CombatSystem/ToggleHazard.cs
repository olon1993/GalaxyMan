using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class ToggleHazard : Hazard
    {
        [SerializeField] private Collider2D triggerCollider;

        public void Toggle() {
            triggerCollider.enabled = !triggerCollider.enabled;
		}
    }
}
