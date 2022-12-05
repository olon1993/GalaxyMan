using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
	public class EnemyWaitAction : EnemyAction
	{
		protected override IEnumerator CarryOutAction() {
			if (_showDebugLog) {
				Debug.Log("EnemyWaitAction.CarryOutAction");
			}
			_actionInEffect = true;
			float t = 0;
			while (t < _actionTime) {
				yield return new WaitForEndOfFrame();
				t += Time.deltaTime;
			}
			StopAction();
		}
	}
}
