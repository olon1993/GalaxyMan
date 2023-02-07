using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class Event : MonoBehaviour, ITriggerable
    {
        [SerializeField] private EventWave[] waves;
        private bool eventActive = false;

        [SerializeField] private GameObject[] triggerables;
        [SerializeField] private bool _isRetriggerable;
        [SerializeField] private GameObject reward;
        private bool _isTriggerable = true;
        private IInputManager playerInput;

		private void ActivateEvent() {
            Debug.Log("Event: Activate");
            foreach (GameObject triggerable in triggerables) {
                triggerable.GetComponent<ITriggerable>().ExecuteTriggerAction(true);
			}

            playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<IInputManager>();
            playerInput.IsEnabled = false;
            StartCoroutine(IntroduceEvent());

        }

        private IEnumerator IntroduceEvent() {
            // Camera stuff goes here
            Debug.Log("Event: Intro");
            yield return new WaitForSeconds(5f);
            playerInput.IsEnabled = true;
            StartCoroutine(RunEventWaves());
        }

        private IEnumerator RunEventWaves() {
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < waves.Length; i++) {
                Debug.Log("Event: Wave: " + i);
                waves[i].StartWave();
                yield return new WaitForEndOfFrame();
                while (waves[i].IsRunning) {
                    yield return new WaitForSeconds(1f);
				}
			}
            StartCoroutine(EndEvent());
		}

        private IEnumerator EndEvent() {
            // Disable player
            // focus camera
            // spawn reward
            // enable player
            Debug.Log("Event: Outro");
            Instantiate(reward, transform.position, Quaternion.identity,null);
            foreach (GameObject triggerable in triggerables) {
                triggerable.GetComponent<ITriggerable>().ExecuteTriggerAction(false);
            }
            yield return new WaitForEndOfFrame();
        }

        public void ExecuteTriggerAction(bool triggerStatus) {
            Debug.Log("Event: Execute");
            if (triggerStatus == eventActive) {
                return;
			}
            if (!_isRetriggerable) {
                if (_isTriggerable) {
                    _isTriggerable = false;
				} else {
                    return;
				}
            }
            eventActive = true;
            ActivateEvent();
        }


        public bool IsRetriggerable { get { return _isRetriggerable; } }
    }
}
