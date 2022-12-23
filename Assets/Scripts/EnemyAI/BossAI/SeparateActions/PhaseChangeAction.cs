using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TheFrozenBanana;

namespace TheFrozenBanana
{
	public class PhaseChangeAction : BossAction
	{

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

		public override bool CheckActionPossibility(int start) {
			return false;
		}

		protected override IEnumerator CarryOutSpecificAction(int phase) {
			_startLocationId = BossManagerScript.BossLocationId;
			_endLocationId = _startLocationId;
			StartCoroutine(ShakeCamera());
			BossManagerScript.HP.ToggleHealthActive(false);
			GameObject tmp = Instantiate(_juiceEffect, gameObject.transform.position + Vector3.up, Quaternion.identity, null) as GameObject;
			yield return new WaitForSeconds(ActionTime[phase]);
			tmp.GetComponent<ParticleSystem>().Stop();
			BossManagerScript.HP.ToggleHealthActive(true);
			Destroy(tmp, 1f);

			_actionBusy = false;
		}

		private IEnumerator ShakeCamera() {
			vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
			float maxTime = 2.5f;
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
