using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class LaserScript : MonoBehaviour
    {
		[SerializeField] private GameObject hitPrefab;
		[SerializeField] private float hitsPerSecond;
        private LineRenderer lr;
        private PolygonCollider2D poly;
		private Transform parent;
		private IDamage dmg;
		private bool isActive;
		private Vector3[] vertices;
		private List<GameObject> targetsHit;
		private GameObject endPoint;


		private void Awake() {
			dmg = GetComponent<IDamage>();
			lr = GetComponent<LineRenderer>();
			poly = GetComponent<PolygonCollider2D>();
			parent = GetComponentInParent<Transform>();
			vertices = new Vector3[2];
			vertices[0] = Vector3.zero;
			vertices[1] = Vector3.zero;
			endPoint = Instantiate(hitPrefab);
			ToggleLaser(false);
		}

		public void ToggleLaser(bool setTo) {
			poly.enabled = setTo;
			lr.enabled = setTo;
			isActive = setTo;
			if (setTo) {
				targetsHit = new List<GameObject>();
				StartCoroutine(DoDamageToObjects());
				endPoint.SetActive(true);

			} else {
				endPoint.SetActive(false);
			}
        }

		private IEnumerator DoDamageToObjects() {
			float iterationLength = 1f / hitsPerSecond;
			WaitForSeconds iteration = new WaitForSeconds(iterationLength);
			while (isActive) {
				yield return iteration;
				foreach (GameObject isHit in targetsHit) {
					isHit.GetComponent<IHealth>().TakeDamage(dmg);
					GameObject hit = Instantiate(hitPrefab, isHit.transform.position,Quaternion.identity, null) as GameObject;
					Destroy(hit, iterationLength);
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D collision) {
			Debug.Log(collision.name + " entered laser");
			IHealth hitObject = collision.gameObject.GetComponent<IHealth>();
			if (hitObject != null) {
				targetsHit.Add(collision.gameObject);
			}
		}

		private void OnTriggerExit2D(Collider2D collision) {
			Debug.Log(collision.name + " exited laser");
			IHealth hitObject = collision.gameObject.GetComponent<IHealth>();
			if (hitObject != null) {
				targetsHit.Remove(collision.gameObject);
			}
		}

		public void UpdateLaserPoints(Vector3 start, Vector3 end) {
			vertices[0] = start;
			vertices[1] = end;
			lr.positionCount = vertices.Length;
			lr.SetPositions(vertices);
			UpdateColliderPoints(start, end);
			if (endPoint != null) {
				endPoint.transform.position = end;
			}
			
		}

		private void UpdateColliderPoints(Vector3 start, Vector3 end) {
			Vector3 laserDir = end - start;
			Vector3 offsetDir = Vector2.Perpendicular(laserDir);
			Vector2[] points = new Vector2[4];
			points[0] = start + offsetDir.normalized * 0.3f;
			points[1] = start - offsetDir.normalized * 0.3f;
			points[2] = end - offsetDir.normalized * 0.3f;
			points[3] = end + offsetDir.normalized * 0.3f;
			poly.points = points;
			poly.offset = -parent.position;
		}
	}
}
