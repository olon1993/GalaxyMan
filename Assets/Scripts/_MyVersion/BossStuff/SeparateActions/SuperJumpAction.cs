using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpAction : BossAction
{
	protected override IEnumerator CarryOutAction() {
		base.CarryOutAction();

		float t = 0f;
		float startX = StartLocations[StartInt].Trans.position.x;
		float startY = StartLocations[StartInt].Trans.position.y;
		float endX = EndLocations[EndInt].Trans.position.x;
		float endY = EndLocations[EndInt].Trans.position.y;
		Vector2 start = new Vector2(startX, startY);
		Vector2 end = new Vector2(endX, endY);
		float jumpY = 20f;
		while (t < TotalActionTime) {
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;

			if (t < (TotalActionTime / 2)) {
				float i = t / (TotalActionTime / 2);
				float heightY = jumpY * Mathf.Sin(i / 2 * Mathf.PI);
				Vector2 bossLoc = Vector2.Lerp(start, end, i) + new Vector2(0, heightY);
				transform.position = bossLoc;
			} else {
				Vector2 bossLoc = end + new Vector2(0, jumpY) * (TotalActionTime - t);
				transform.position = bossLoc;
			}
		}
		transform.position = end;
		yield return new WaitForSeconds(1f);
		_actionBusy = false;
	}
}
