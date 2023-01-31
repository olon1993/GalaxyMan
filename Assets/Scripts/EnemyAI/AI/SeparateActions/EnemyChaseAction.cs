using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
	public class EnemyChaseAction : EnemyAction
	{
		[SerializeField] private bool _jumpToPlayer;
		private float dir;

		protected override IEnumerator CarryOutAction() {
			if (_showDebugLog) {
				Debug.Log("EnemyChaseAction.CarryOutAction");
			}
			_actionInEffect = true;
			float t = 0;
			while (t < _actionTime) {

				if (!_actionInEffect) {
					if (_showDebugLog) {
						Debug.Log("EnemyChaseAction.StopAction");
					}
					break;
				}
				if (ec.Target.transform.position.x > gameObject.transform.position.x) {
					dir = 1f;
				} else {
					dir = -1f;
				}
				if (dir > 0 && _locomotion.IsRightCollision && !_inputManager.IsJump) {
					ec.Jump();
				} else if (dir < 0 && _locomotion.IsLeftCollision && !_inputManager.IsJump) {
					ec.Jump();
				}
				if (_jumpToPlayer) {
					if (Mathf.Abs(gameObject.transform.position.x - ec.Target.transform.position.x) < ec.Target.transform.position.y - gameObject.transform.position.y) {
						ec.Jump();
					}
				}

				if (_inputManager.IsEnabled) {
					_inputManager.OverrideHorizontalInput(dir);
				}
				yield return new WaitForEndOfFrame();
				t += Time.deltaTime;
			}
			StopAction();
		}
	}
}
