using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class LevelTrigger : MonoBehaviour
    {
		private LevelController lc;
		public GameObject CMCamSwitch;
		public Transform respawnPoint;
		private CamSwitch cmSwitch;
		public bool bossTrigger;
		public bool inputOverride;
		public bool repeatableTrigger;
		public bool respawnTrigger;
		public int camId;
		

		private void Awake() {
			cmSwitch = CMCamSwitch.GetComponent<CamSwitch>();
			lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
		}

		private void OnTriggerEnter2D(Collider2D col) {
			if (col.CompareTag("Player")) {
				if (respawnTrigger) {
					GetComponentInParent<IHazard>().ChangeRespawnLocation(respawnPoint);
				} else {
					if (cmSwitch != null) {
						cmSwitch.SwitchCamera(camId);
					}
					lc.LevelCameraTrigger(bossTrigger, inputOverride);
					if (!repeatableTrigger) {
						this.gameObject.SetActive(false);
					}
				}
			}
		}
	}
}
