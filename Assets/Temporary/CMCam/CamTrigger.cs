using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class CamTrigger : MonoBehaviour
    {

		public GameObject CMCamSwitch;
		private CamSwitch cmSwitch;

		private void Awake() {
			cmSwitch = CMCamSwitch.GetComponent<CamSwitch>();
		}

		private void OnTriggerEnter2D(Collider2D col) {
			if (col.CompareTag("Player")) {
				cmSwitch.SwitchCamera();
				this.gameObject.SetActive(false);
			}
		}
	}
}
