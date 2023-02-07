using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class EventWave : MonoBehaviour
    {
        [SerializeField] private int waveId;
        [SerializeField] private GameObject spawnEffect;
        [SerializeField] private GameObject[] waveMonsters;
        private List<GameObject> monsterTracker;
        private bool _isRunning;

        public void StartWave() {
            _isRunning = true;
            monsterTracker = new List<GameObject>();
            StartCoroutine(SpawnEnemies());
		}

        private IEnumerator SpawnEnemies() {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < waveMonsters.Length; i++) {
                GameObject tmp = Instantiate(spawnEffect, waveMonsters[i].transform.position, Quaternion.identity, null) as GameObject;
                Destroy(tmp,1f);
                yield return new WaitForSeconds(0.4f);
                waveMonsters[i].SetActive(true);
                monsterTracker.Add(waveMonsters[i]);
			}
            yield return new WaitForEndOfFrame();
            StartCoroutine(TrackMonsters());
		}

        private IEnumerator TrackMonsters() {
            while (monsterTracker.Count > 0) {
                foreach(GameObject monster in monsterTracker) {
                    if (monster.GetComponent<IHealth>().IsDead) {
                        monsterTracker.Remove(monster);
                        break;
					}
                    yield return new WaitForSeconds(1f);
				}
			}
            _isRunning = false;
		}

        public bool IsRunning { get { return _isRunning; } }
    }
}
