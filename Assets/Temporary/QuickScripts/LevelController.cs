using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class LevelController : MonoBehaviour
    {
		private GameObject player;
		[SerializeField] private AudioClip levelMusic;
		[SerializeField] private AudioClip bossMusic;
		[SerializeField] private AudioSource musicAudio;
		[SerializeField] private GameObject boss;
		[SerializeField] private Vector3 bossSpawnLoc;
		[SerializeField] private Vector3 bossStartLoc;

		public void Awake() {
			player = GameObject.FindGameObjectWithTag("Player");
			
		}
		public void LevelCameraTrigger(bool boss, bool inputOverride) {
			if (boss) {
				StartCoroutine(RunCameraBossTrigger());
			} else {
				StartCoroutine(RunCameraTrigger(inputOverride));
			}
		}

        private IEnumerator RunCameraTrigger(bool inputOverride) {
			if (inputOverride) {
				IInputManager input = player.GetComponent<IInputManager>();
				musicAudio.Stop();
				input.IsEnabled = false;
				input.OverrideHorizontalInput(1);
				yield return new WaitForSeconds(0.4f);
				input.EndOverride();
				yield return new WaitForSeconds(2f);
				input.IsEnabled = true;
			}
		}

		private IEnumerator RunCameraBossTrigger() {
			IInputManager input = player.GetComponent<IInputManager>();
			input.IsEnabled = false;
			input.OverrideHorizontalInput(1);
			yield return new WaitForSeconds(0.4f);
			input.EndOverride();

			yield return new WaitForSeconds(2f);
			GameObject tmpBoss = Instantiate(boss, bossSpawnLoc, Quaternion.identity, null) as GameObject;
			yield return new WaitForSeconds(2f);
			float t = 0;
			while (t < 3) {
				float i = t / 3;
				Vector3 lerpPosition = Vector3.Lerp(bossSpawnLoc,bossStartLoc,i);
				tmpBoss.transform.position = lerpPosition;
				t += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			tmpBoss.transform.position = bossStartLoc;
			yield return new WaitForSeconds(2f);
			tmpBoss.GetComponent<BossManager>().Activate();
			musicAudio.clip = bossMusic;
			musicAudio.Play();
			input.IsEnabled = true;
		}

		public void BossIsDefeated() {
			StartCoroutine(BossDefeatChanges());
		}

		private IEnumerator BossDefeatChanges() {
			IInputManager input = player.GetComponent<IInputManager>();
			input.IsEnabled = false;
			DoorScript[] doors = gameObject.GetComponentsInChildren<DoorScript>();
			yield return new WaitForSeconds(10f);
			input.IsEnabled = true;
			foreach (DoorScript door in doors) {
				door.AlwaysOpen();
			}
		}
	}
}
