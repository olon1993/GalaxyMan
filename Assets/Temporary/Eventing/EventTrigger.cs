using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class EventTrigger : MonoBehaviour
    {
		[SerializeField] private GameObject eventGameObject;

		private void OnTriggerEnter2D(Collider2D col) {
			if (col.CompareTag("Player")) {
				eventGameObject.GetComponent<ITriggerable>().ExecuteTriggerAction(true);
				Debug.Log("Event Trigger");
				this.gameObject.SetActive(false);
			}
		}
	}
}
