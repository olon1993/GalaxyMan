using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class DoorScript : MonoBehaviour, ITriggerable
    {
        public Animator ac;
		public Collider2D doorCollider;
		public string tagTrigger;
		public bool startOpen = false;
		public bool canOpen = true;
		private AudioSource ad;
		private bool allowChange = true;
		private bool _isRetriggerable; 

		private void Awake() {
			ad = GetComponent<AudioSource>();
			if (startOpen) {
				OpenDoor(true);
			}
		}

		public void OnTriggerEnter2D(Collider2D col) {
			if (!allowChange) {
				return;
			}
			if (col.CompareTag(tagTrigger)) {
				StartCoroutine(OpenDoor(true));
			}
		}

		public void OnTriggerExit2D(Collider2D col) {
			if (!allowChange) {
				return;
			}
			if (col.CompareTag(tagTrigger)) {
				StartCoroutine(OpenDoor(false));
			}
		}


		private IEnumerator OpenDoor(bool open) {
			if (ad != null) {
				ad.Play();
			}
			ac.SetBool("Open", open);
			yield return new WaitForSeconds(0.4f);
			doorCollider.enabled = !open;
		} 

		public void AlwaysOpen() {
			StartCoroutine(OpenDoor(true));
			allowChange = false;
		}

		public void ExecuteTriggerAction(bool triggerStatus) {
			if (triggerStatus) {
				OpenDoor(false);
				canOpen = false;
				allowChange = false;

			} else {
				Debug.Log("door should now be open");
				canOpen = true;
				allowChange = true;
				OpenDoor(true);
				allowChange = false;
			}
		}


		public bool IsRetriggerable { get { return _isRetriggerable; } }
	}
}
