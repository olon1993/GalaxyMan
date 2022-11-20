using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class HazardSpawner : MonoBehaviour
    {
        public float initialDelay;
        public float spawnInterval;
        public GameObject spawnObject;
        private float realInterval;

		private void Awake() {
            StartCoroutine(DelayStart());
		}

        private IEnumerator DelayStart() {
            yield return new WaitForSeconds(initialDelay);
            StartCoroutine(RunSpawner());
        }

        private IEnumerator RunSpawner() {
            while (true) {
                SpawnObject();
                realInterval = spawnInterval + Random.Range(-2f, 2f);
                yield return new WaitForSeconds(realInterval);
			}
		}

        private void SpawnObject() {
            GameObject obj = Instantiate(spawnObject, transform.position + Vector3.down,Quaternion.identity,null) as GameObject;
            obj.GetComponent<GravityHazard>().SetupSpawnedHazard(-1);
        }
	}
}
