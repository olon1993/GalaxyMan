using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAction : BossAction
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
		GameObject tmp = Instantiate(_juiceEffect, transform.position, Quaternion.identity, null) as GameObject;
		Destroy(tmp, 2f);
		while (t < TotalActionTime) {
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;

			float i = t / TotalActionTime;
			float heightY = JumpHeight * Mathf.Sin(i * Mathf.PI);
			Vector2 bossLoc = Vector2.Lerp(start, end, i) + new Vector2(0, heightY);
			transform.position = bossLoc;
		}
		GameObject tmp2 = Instantiate(_juiceEffect, transform.position, Quaternion.identity, null) as GameObject;
		Destroy(tmp2, 2f);
		transform.position = end;
		_actionBusy = false;
	}
}