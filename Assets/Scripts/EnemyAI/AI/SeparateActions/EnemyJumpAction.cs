using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
 	public class EnemyJumpAction : EnemyAction
	{
		private float dir;

		protected override IEnumerator CarryOutAction() {
			if (_showDebugLog) {
				Debug.Log("EnemyJumpAction.CarryOutAction");
			}
			_actionInEffect = true;
			float t = 0;
			while (t < _actionTime) {
				yield return new WaitForEndOfFrame();
				t += Time.deltaTime;
				// Turn around
				_locomotion.HorizontalLook = Mathf.Sign(ec.target.transform.position.x - gameObject.transform.position.x);
				dir = _locomotion.HorizontalLook;
			}

			_inputManager.OverrideHorizontalInput(dir);
			ec.Jump();
			yield return new WaitForSeconds(0.1f);
			while (!_locomotion.IsGrounded) {
				if (dir != Mathf.Sign(ec.target.transform.position.x - gameObject.transform.position.x)) {
					_inputManager.EndOverride();
				}
				yield return new WaitForEndOfFrame();
			}
			StopAction();
		}
	}
}
