using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpAction : BossAction
{
	[SerializeField] protected float JumpHeight = 10f;

	protected override IEnumerator CarryOutSpecificAction() {
		float t = 0f;
		float startX = StartLocations[StartInt].Trans.position.x;
		float startY = StartLocations[StartInt].Trans.position.y;
		float endX = EndLocations[EndInt].Trans.position.x;
		float endY = EndLocations[EndInt].Trans.position.y;
		Vector2 start = new Vector2(startX, startY);
		Vector2 end = new Vector2(endX, endY);
		while (t < TotalActionTime) {
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;

			if (t < (TotalActionTime / 2)) {
				float i = t / (TotalActionTime / 2);
				float heightY = JumpHeight * Mathf.Sin(i / 2 * Mathf.PI);
				Vector2 bossLoc = Vector2.Lerp(start, end, i) + new Vector2(0, heightY);
				transform.position = bossLoc;
			} else {
				Vector2 bossLoc = end + new Vector2(0, JumpHeight) * (TotalActionTime - t);
				transform.position = bossLoc;
			}
		}
		transform.position = end;
		_actionBusy = false;
	}
}