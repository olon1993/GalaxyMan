using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class GravityHazard : Hazard
    {
		private bool _active;
		private float _velocityY;
        public void SetupSpawnedHazard(float startY) {
			_velocityY = startY;
			_active = true;
		}


		private void Update() {
			if (_active) {
				Vector2 move = new Vector2(0, _velocityY * Time.deltaTime);
				transform.Translate(move);
				_velocityY -= 10 * Time.deltaTime;
			}
		}
	}
}

