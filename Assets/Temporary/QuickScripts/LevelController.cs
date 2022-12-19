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
		public void LevelTrigger(bool boss, bool inputOverride) {
			if (boss) {
				StartCoroutine(RunBossTrigger());
			} else {
				StartCoroutine(RunTrigger(inputOverride));
			}
		}

        private IEnumerator RunTrigger(bool inputOverride) {
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

		private IEnumerator RunBossTrigger() {
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
	}
}
