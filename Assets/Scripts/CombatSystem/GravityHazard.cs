using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class GravityHazard : Hazard
    {
		private bool _active;
		private float _velocityY;
		[SerializeField] private bool despawnWhenColliding;
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

		protected override void OnTriggerEnter2D(Collider2D col) {
			base.OnTriggerEnter2D(col);
			if (despawnWhenColliding) {
				_active = false;
				Destroy(this.gameObject, 0.1f);
				return;
			}
		}
	}
}

