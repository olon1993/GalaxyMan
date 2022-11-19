using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class DoorScript : MonoBehaviour
    {
        public Animator ac;
		public Collider2D doorCollider;

		public void OnTriggerEnter2D(Collider2D col) {
			if (col.CompareTag("Player")) {
				StartCoroutine(OpenDoor(true));
			}
		}

		public void OnTriggerExit2D(Collider2D col) {
			if (col.CompareTag("Player")) {
				StartCoroutine(OpenDoor(false));
			}
		}


		private IEnumerator OpenDoor(bool open) {
			ac.SetBool("Open", open);
			yield return new WaitForSeconds(0.4f);
			doorCollider.enabled = !open;

		}
	}
}
