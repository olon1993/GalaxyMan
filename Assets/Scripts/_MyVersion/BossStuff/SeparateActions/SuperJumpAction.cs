using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

public class SuperJumpAction : BossAction
{
	[SerializeField] protected float JumpHeight = 10f;
	[SerializeField] protected Vector2 WaveSpeed;
	[SerializeField] protected GameObject WavePrefab;

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
		CreateWave();
		_actionBusy = false;
	}

	private void CreateWave() {
		StartCoroutine(InstantiateWave());
	}

	private IEnumerator InstantiateWave() {
		Vector2 startLocation = transform.position;
		float t = 0;
		float maxTime = 20 / WaveSpeed.x;
		float iteration = 0;
		Vector3 spawnLocationCenter = new Vector3(startLocation.x, startLocation.y - 2, 0);
		GameObject tmp = Instantiate(WavePrefab, spawnLocationCenter, Quaternion.identity, null) as GameObject;
		tmp.GetComponent<GravityHazard>().SetupSpawnedHazard(WaveSpeed.y);
		while (t < maxTime) {
			float i = maxTime / 20;
			yield return new WaitForSeconds(i);
			t += i;
			iteration++;
			Vector3 spawnLocationLeft = new Vector3(startLocation.x - iteration, startLocation.y - 2,0);
			GameObject tmpLeft = Instantiate(WavePrefab, spawnLocationLeft, Quaternion.identity, null) as GameObject;
			tmpLeft.GetComponent<GravityHazard>().SetupSpawnedHazard(WaveSpeed.y);
			Vector3 spawnLocationRight = new Vector3(startLocation.x + iteration, startLocation.y - 2, 0);
			GameObject tmpRight = Instantiate(WavePrefab, spawnLocationRight, Quaternion.identity, null) as GameObject;
			tmpRight.GetComponent<GravityHazard>().SetupSpawnedHazard(WaveSpeed.y);
			Destroy(tmpLeft, 5f);
			Destroy(tmpRight, 5f);
		}
	}


	/// if speed is 10, it should be 2 seconds to do all.
	/// if speed is 1 , it should be 20 seconds to do all.
	/// so the initial time is 20.
	/// the higher the speed, the lower the total time.
	/// but, how much time inbetween?
	/// maxTime / 20 
}