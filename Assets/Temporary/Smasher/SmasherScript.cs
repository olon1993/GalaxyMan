using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class SmasherScript : MonoBehaviour
    {
        public float delayInTheBeginning = 0f;
        public float timeToStart = 5f;
        public float timeToSmash = 1f;
        public float timeToPause = 2f;
        public float timeToReset = 5f;
        public Vector3 start;
        public Vector3 end;
        public Material pipeSpriteMaterial;
        public LineRenderer pipe;

        private void Awake() {
            start = gameObject.transform.position;
            end = start + Vector3.down * 8;
            pipe = gameObject.AddComponent<LineRenderer>();
            pipe.material = pipeSpriteMaterial;
            pipe.positionCount = 2;
            StartCoroutine(DelayingStart());
        }

		private void Update() {
            Vector3[] pipePoints = new Vector3[2];
            pipePoints[0] = start;
            pipePoints[1] = transform.position;
            pipe.SetPositions(pipePoints);
		}

        private IEnumerator DelayingStart() {
            yield return new WaitForSeconds(delayInTheBeginning);
            StartCoroutine(RunStart());
        }

		private IEnumerator RunStart() {
            yield return new WaitForSeconds(timeToStart);
            StartCoroutine(RunSmash());
		}

        private IEnumerator RunSmash() {
            float t = 0;
            while (t < timeToSmash) {
                t += Time.deltaTime;
                float i = t / timeToSmash;
                transform.position = Vector3.Lerp(start,end,i);
                yield return new WaitForEndOfFrame();
            }
            transform.position = Vector3.Lerp(start, end, 1);
            StartCoroutine(RunPause());
		}

        private IEnumerator RunPause() {
            yield return new WaitForSeconds(timeToPause);
            StartCoroutine(RunReset());
		}

        private IEnumerator RunReset() {
            float t = 0;
            while (t < timeToReset) {
                t += Time.deltaTime;
                float i = t / timeToReset;
                transform.position = Vector3.Lerp(end, start, i);
                yield return new WaitForEndOfFrame();
            }
            transform.position = Vector3.Lerp(end, start, 1);
            StartCoroutine(RunStart());
		}
	}
}
