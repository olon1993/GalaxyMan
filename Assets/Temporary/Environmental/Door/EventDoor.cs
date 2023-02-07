using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class EventDoor : MonoBehaviour, ITriggerable
    {
        public Animator ac;
        public Collider2D doorCollider;
        [SerializeField] private bool startOpen = true;
        [SerializeField] private bool _isRetriggerable;
        private bool isOpen;

		private void Awake() {
			if (startOpen) {
                StartCoroutine(ChangeDoorState(true));
			}
		}
		private IEnumerator ChangeDoorState(bool open) {
            if (isOpen == open) {
                yield break;
            }
            ac.SetBool("Open", open);
            yield return new WaitForSeconds(0.4f);
            doorCollider.enabled = !open;
        } 


        public void ExecuteTriggerAction(bool triggerStatus) {
            StartCoroutine(ChangeDoorState(!triggerStatus));
		}

        public bool IsRetriggerable { get { return _isRetriggerable; } }
    }
}
