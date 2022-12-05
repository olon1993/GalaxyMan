using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
	public class EnemyPatrolAction : EnemyAction
	{
		[SerializeField] private Transform[] locations;
		public int currentLocation;
		private float dir;

		protected override IEnumerator CarryOutAction() {
			if (_showDebugLog) {
				Debug.Log("EnemyPatrolAction.CarryOutAction");
			}
			_actionInEffect = true;
			currentLocation++;
			if (currentLocation == locations.Length) {
				currentLocation = 0;
			}
			if (locations[currentLocation].position.x > transform.position.x) {
				_inputManager.OverrideHorizontalInput(1);
				dir = 1;
			} else {
				_inputManager.OverrideHorizontalInput(-1);
				dir = -1;
			}
			float t = 0;
			while (t < _actionTime) {
				if (dir > 0 && _locomotion.IsRightCollision && !_inputManager.IsJump) {
					ec.Jump();
				} else if (dir < 0 && _locomotion.IsLeftCollision && !_inputManager.IsJump) {
					ec.Jump();
				}
				if (Vector3.Distance(locations[currentLocation].position, gameObject.transform.position) < 1f) {
					_inputManager.EndOverride();
					break;
				}
				yield return new WaitForEndOfFrame();
				t += Time.deltaTime;
			}
			StopAction();
		}
	}

}
