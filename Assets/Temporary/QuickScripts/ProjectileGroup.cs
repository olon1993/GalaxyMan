using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class ProjectileGroup : MonoBehaviour
    {
        public IMovementPatterns.MovementPattern pattern;
        public GameObject projectilePrefab;
        public int projectileCount;
        public float distanceFromCenter;
        public float expandTime;
        public GameObject projectileContainer;
        public GameObject[] liveProjectiles;
        public float lerpI;
        public Vector3 dir;

		private void Awake() {
            liveProjectiles = new GameObject[projectileCount];
            for (int i = 0; i < projectileCount; i++) {
                GameObject tmp = Instantiate(projectilePrefab, projectileContainer.transform.position, Quaternion.identity, projectileContainer.transform) as GameObject;
                liveProjectiles[i] = tmp;
			}
            if (pattern == IMovementPatterns.MovementPattern.EXPAND) {
                StartCoroutine(ExpandProjectileGroup());
            } else if (pattern == IMovementPatterns.MovementPattern.OSCILLATE) {
                StartCoroutine(OscillateProjectileGroup());
            }
		}

        private IEnumerator ExpandProjectileGroup() {
            yield return new WaitForEndOfFrame();
            float mathAngle = 360 / projectileCount;
            float t = 0;
            while (t < expandTime) {
              //  float
                lerpI = t / expandTime;
                Debug.Log("Lerp: " + lerpI);
                for (int i = 0; i < projectileCount; i++) {
                    dir = Vector3.up * distanceFromCenter;
                    dir = Quaternion.AngleAxis(mathAngle * i, Vector3.forward) * dir;
                    Debug.Log("i: " + i + " : " + mathAngle * i + " - dir: " + dir);
                    if (liveProjectiles[i].activeSelf) {
                        liveProjectiles[i].transform.localPosition = Vector3.Lerp(projectileContainer.transform.localPosition, dir, lerpI);
                    }
				}
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
			}
		}

        private IEnumerator OscillateProjectileGroup() {
            yield return new WaitForEndOfFrame();
            float mathAngle = 360 / projectileCount;
            float t = 0;
            bool expand = true;
            while (true) {
                if (expand) {
                    t = 0;
                    while (t < expandTime) {
                        //  float
                        lerpI = t / expandTime;
                        Debug.Log("Lerp: " + lerpI);
                        for (int i = 0; i < projectileCount; i++) {
                            dir = Vector3.up * distanceFromCenter;
                            dir = Quaternion.AngleAxis(mathAngle * i, Vector3.forward) * dir;
                            Debug.Log("i: " + i + " : " + mathAngle * i + " - dir: " + dir);
                            if (liveProjectiles[i].activeSelf) {
                                liveProjectiles[i].transform.localPosition = Vector3.Lerp(projectileContainer.transform.localPosition, dir, lerpI);
                            }
                        }
                        t += Time.deltaTime;
                        yield return new WaitForEndOfFrame();
                    }
                    expand = false;
                } else {
                    t = 0;
                    while (t < expandTime) {
                        //  float
                        lerpI = t / expandTime;
                        Debug.Log("Lerp: " + lerpI);
                        for (int i = 0; i < projectileCount; i++) {
                            dir = Vector3.up * distanceFromCenter;
                            dir = Quaternion.AngleAxis(mathAngle * i, Vector3.forward) * dir;
                            Debug.Log("i: " + i + " : " + mathAngle * i + " - dir: " + dir);
                            if (liveProjectiles[i].activeSelf) {
                                liveProjectiles[i].transform.localPosition = Vector3.Lerp(dir, projectileContainer.transform.localPosition,  lerpI);
                            }
                        }
                        t += Time.deltaTime;
                        yield return new WaitForEndOfFrame();
                    }
                    expand = true;
                }
            }
        }
    }
}
