using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;
using Cinemachine;

namespace TheFrozenBanana
{
	public class SuperJumpAction : BossAction
	{
		[SerializeField] protected float JumpHeight = 10f;
		[SerializeField] protected Vector2 WaveSpeed;
		[SerializeField] protected GameObject WavePrefab;
		[SerializeField] protected Vector2 CameraVibration;
		private GameObject cam;
		private CinemachineStateDrivenCamera sdcam;
		private CinemachineVirtualCamera vcam;

		protected override void Awake() {
			base.Awake();
			cam = GameObject.FindGameObjectWithTag("MainCamera");
			// Cinemachine requires to wait a second before getting the active camera, delay the camera retrieval
			StartCoroutine(RetrieveVirtualCamera());

		}

		private IEnumerator RetrieveVirtualCamera() {
			CinemachineBrain cine = cam.GetComponent<CinemachineBrain>();
			Debug.Log("Cinemachine Brain on : " + cine.gameObject.name);
			yield return new WaitForSeconds(1f);
			sdcam = cine.ActiveVirtualCamera as CinemachineStateDrivenCamera;
			vcam = sdcam.LiveChild as CinemachineVirtualCamera;
			yield return new WaitForSeconds(1f);
			Debug.Log("V Cam: " + vcam);
		}

		protected override IEnumerator CarryOutSpecificAction(int phase) {
			float t = 0f;
			float startX = StartLocations[StartInt].Trans.position.x;
			float startY = StartLocations[StartInt].Trans.position.y;
			float endX = EndLocations[EndInt].Trans.position.x;
			float endY = EndLocations[EndInt].Trans.position.y;
			Vector2 start = new Vector2(startX, startY);
			Vector2 end = new Vector2(endX, endY);
			GameObject tmp = Instantiate(_juiceEffect, transform.position, Quaternion.identity, null) as GameObject;
			Destroy(tmp, 2f);
			while (t < ActionTime[phase]) {
				yield return new WaitForEndOfFrame();
				t += Time.deltaTime;

				if (t < (ActionTime[phase] / 2)) {
					float i = t / (ActionTime[phase] / 2);
					float heightY = JumpHeight * Mathf.Sin(i / 2 * Mathf.PI);
					Vector2 bossLoc = Vector2.Lerp(start, end, i) + new Vector2(0, heightY);
					transform.position = bossLoc;
				} else {
					Vector2 bossLoc = end + new Vector2(0, JumpHeight) * (ActionTime[phase] - t);
					transform.position = bossLoc;
				}
			}
			transform.position = end;
			CreateWave();
			_actionBusy = false;
		}

		private void CreateWave() {
			GameObject tmp = Instantiate(_juiceEffect, transform.position, Quaternion.identity, null) as GameObject;
			Destroy(tmp, 2f);
			StartCoroutine(InstantiateWave());
			StartCoroutine(ShakeCamera());
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
				Vector3 spawnLocationLeft = new Vector3(startLocation.x - iteration, startLocation.y - 2, 0);
				GameObject tmpLeft = Instantiate(WavePrefab, spawnLocationLeft, Quaternion.identity, null) as GameObject;
				tmpLeft.GetComponent<GravityHazard>().SetupSpawnedHazard(WaveSpeed.y);
				Vector3 spawnLocationRight = new Vector3(startLocation.x + iteration, startLocation.y - 2, 0);
				GameObject tmpRight = Instantiate(WavePrefab, spawnLocationRight, Quaternion.identity, null) as GameObject;
				tmpRight.GetComponent<GravityHazard>().SetupSpawnedHazard(WaveSpeed.y);
				Destroy(tmpLeft, 5f);
				Destroy(tmpRight, 5f);
			}
		}

		private IEnumerator ShakeCamera() {
			float maxTime = 20 / WaveSpeed.x;
			vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
			float t = 0;
			float i = 0;
			while (t < maxTime) {
				t += Time.deltaTime;
				i = t / maxTime;
				yield return new WaitForEndOfFrame();
				vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = (1 - i);
			}
			vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
		}
	}
}