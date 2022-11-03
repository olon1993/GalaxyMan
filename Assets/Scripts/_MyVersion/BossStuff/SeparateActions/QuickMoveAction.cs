using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMoveAction : BossAction
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
		while (t < TotalActionTime) {
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;

			float i = t / TotalActionTime;
			Vector2 bossLoc = Vector2.Lerp(start, end, i);
			transform.position = bossLoc;
		}
		transform.position = end;
		yield return new WaitForSeconds(1f);
		_actionBusy = false;
	}
}