using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class rotate : MonoBehaviour
    {
        public float rotateSpeed;

		private void Update() {
			transform.Rotate(0,0,rotateSpeed);
		}
	}
}
