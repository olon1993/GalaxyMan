using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
	public class waypointmove : MonoBehaviour
	{
		public int nextWaypoint;
		public Transform[] waypoints;
		public float speed;
		public float pauseAtWaypoint;
		public float distanceToWaypoint;
		public float moveDistance;

		private void Awake() {
			StartCoroutine(RunMovement());
		}
		private IEnumerator RunMovement() {
			while (true) {
				moveDistance = speed * Time.deltaTime;
				distanceToWaypoint = Vector3.Distance(transform.position, waypoints[nextWaypoint].position);
				if (distanceToWaypoint > moveDistance) {
					Vector3 direction = (waypoints[nextWaypoint].position - transform.position).normalized * moveDistance;
					transform.Translate(direction);
				} else {
					transform.position = waypoints[nextWaypoint].position;
					nextWaypoint++;
					if (nextWaypoint == waypoints.Length) {
						nextWaypoint = 0;
					}
					yield return new WaitForSeconds(pauseAtWaypoint);
				}
				yield return new WaitForEndOfFrame();

			}
		}
	}
}
