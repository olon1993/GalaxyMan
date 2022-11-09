using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAction : BossAction
{
	[SerializeField] private float bounceHeight;
	[SerializeField] private float bounceHeightTimer;
	[SerializeField] private GameObject[] bounceEndLocationsGO;
	[SerializeField] private IActionLocations[] bounceEndLocations;

	protected override void Awake() {
		base.Awake();
		bounceEndLocations = new IActionLocations[bounceEndLocationsGO.Length];
		for (int i = 0; i < bounceEndLocationsGO.Length; i++) {
			bounceEndLocations[i] = bounceEndLocationsGO[i].GetComponent<IActionLocations>();
		}
	}

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

			float i = t / TotalActionTime;
			Vector2 bossLoc = Vector2.Lerp(start, end, i);
			transform.position = bossLoc;
		}
		// HIT WALL EFFECT
		float dir = Mathf.Sign(endX - startX);
		GameObject tmp = Instantiate(_juiceEffect, transform.position - Vector3.right * dir, Quaternion.identity, null) as GameObject;
		Destroy(tmp, 2f);
		// NOW GO UP
		t = 0f;
		start = new Vector2(end.x, end.y);
		end = new Vector2(end.x, end.y + bounceHeight);
		while (t < bounceHeightTimer) {
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;

			float i = t / bounceHeightTimer;
			Vector2 bossLoc = Vector2.Lerp(start, end, i);
			transform.position = bossLoc;
		}

		// HIT THE TOP 

		GameObject tmp2 = Instantiate(_juiceEffect, transform.position + Vector3.up, Quaternion.identity, null) as GameObject;
		Destroy(tmp2, 2f);
		// NOW ARC TO A RANDOM PLATFORM LOCATION
		t = 0f;
		start = new Vector2(end.x, end.y);
		int bounceInt = Random.Range(0, bounceEndLocations.Length);
		int bounceId = bounceEndLocations[bounceInt].LocationId;
		endX = bounceEndLocations[bounceInt].Trans.position.x;
		endY = bounceEndLocations[bounceInt].Trans.position.y;

		end = new Vector2(endX, endY);
		while (t < bounceHeightTimer) {
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;
			float i = t / bounceHeightTimer;
			Vector2 tmpEnd = Vector2.Lerp(new Vector2(end.x,start.y + bounceHeight),end, i);
			Vector2 bossLoc = Vector2.Lerp(start, tmpEnd, i);
			transform.position = bossLoc;
		}
		GameObject tmp3 = Instantiate(_juiceEffect, transform.position - Vector3.up, Quaternion.identity, null) as GameObject;
		Destroy(tmp3, 2f);
		transform.position = end;
		_endLocationId = bounceId;
		_actionBusy = false;
	}
}
