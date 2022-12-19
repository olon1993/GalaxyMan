using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class FallHazard : Hazard
    {
        public GameObject testPS;

        protected override void OnTriggerEnter2D(Collider2D col) {
            if (_showDebugLog) {
                Debug.Log("Hazard hit :" + col.gameObject.name);
            }
            if (col.tag == null) {
                return;
            }
            if (col.CompareTag(_ignoreTag)) {
                return;
            }

            IHealth otherHealth = col.GetComponent<IHealth>();
            if (_showDebugLog) {
                Debug.Log("Health object: " + otherHealth);
			}
            if (otherHealth != null) {
                otherHealth.TakeDamage(_damage);
            }
            if (col.CompareTag("Player")) {
                StartCoroutine(TransportPlayer(col));
			}
        }



        private IEnumerator TransportPlayer(Collider2D col) {
            GameObject player = col.gameObject;
            Vector3 start = player.transform.position;
            Vector3 end = _respawnLocation.position;

            player.SetActive(false);
            GameObject ps = Instantiate(testPS, start, Quaternion.identity, null) as GameObject;
            yield return new WaitForSeconds(3f);
            float t = 0;
            float maxTime = 1f;
            while (t < maxTime) {
                t += Time.deltaTime;
                float i = t / maxTime;
                float j = i * 2;
                player.transform.position = Vector3.Lerp(start, end, i);
                if (j > 1) {
                    j = 1;
				}
                ps.transform.position = Vector3.Lerp(start, end, j);
                yield return new WaitForEndOfFrame();
            }
            player.transform.position = end;
            player.SetActive(true);
            Destroy(ps, 0.2f);
        }
    }
}
