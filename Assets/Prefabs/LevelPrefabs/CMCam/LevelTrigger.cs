using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class LevelTrigger : MonoBehaviour
    {
		private LevelController lc;
		public GameObject CMCamSwitch;
		private CamSwitch cmSwitch;
		public bool bossTrigger;

		private void Awake() {
			cmSwitch = CMCamSwitch.GetComponent<CamSwitch>();
			lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
		}

		private void OnTriggerEnter2D(Collider2D col) {
			if (col.CompareTag("Player")) {
				cmSwitch.SwitchCamera();
				lc.LevelTrigger(bossTrigger);
				this.gameObject.SetActive(false);
			}
		}
	}
}
