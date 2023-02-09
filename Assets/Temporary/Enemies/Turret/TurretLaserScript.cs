using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
	public class TurretLaserScript : MonoBehaviour
	{
		public GameObject projectilePrefab;
		public GameObject barrel;
		public Transform projectileSpawnpoint;
		public float maxDistance;
		public float speed;
		public float laserActiveTime;
		public float laserChargeTime;
		private GameObject target;
		private bool openFire;
		private Animator ac;
		[SerializeField] protected LayerMask layer;
		private LaserScript laser;

		private void Awake() {
			target = GameObject.FindGameObjectWithTag("Player");
			laser = GetComponentInChildren<LaserScript>();
			ac = GetComponentInChildren<Animator>();
			StartCoroutine(FiringMechanism());
		}

		private void OnDrawGizmosSelected() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, maxDistance);
		}

		private void Update() {
			if (target != null) {
				if (target.transform.position.y > transform.position.y) {
					return;
				}
				float distance = Vector3.Distance(transform.position, target.transform.position);
				float angle = 0;
				if (distance < maxDistance) {
					Vector3 vectorToTarget = target.transform.position - barrel.transform.position;
					angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
					if (!openFire) {
						ac.SetTrigger("Charge");
					}
					openFire = true;
				} else {
					Vector3 vectorToTarget = (barrel.transform.position + (Vector3.down)) - barrel.transform.position;
					angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
					//	openFire = false;
				}
				angle += 90;
				Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
				barrel.transform.rotation = Quaternion.Slerp(barrel.transform.rotation, q, Time.deltaTime * speed);
			}
		}

		private IEnumerator FiringMechanism() {
			float t = 0;
			while (true) {
				yield return new WaitForEndOfFrame();
				if (openFire) {
					t += Time.deltaTime;
				}

				if (t >= laserChargeTime) {
					ac.SetTrigger("Fire");
					UpdateLaser();
					ToggleLaser(true);
					float tFire = laserActiveTime;
					while (tFire > 0) {
						tFire -= Time.deltaTime;
						UpdateLaser();
						yield return new WaitForEndOfFrame();
					}
					ToggleLaser(false);
					openFire = false;
					t = 0;
				}
			}
		}

		private void ToggleLaser(bool setTo) {
			laser.ToggleLaser(setTo);
		}

		private void UpdateLaser() {
			RaycastHit2D hit;
			Vector3 dir = barrel.transform.rotation * Quaternion.AngleAxis(-90, Vector3.forward) * Vector3.right;
			Vector3 startPos = projectileSpawnpoint.position;
			Vector3 endPos = startPos + dir * 20;
			hit = Physics2D.Raycast(projectileSpawnpoint.position, dir, 20, layer);
			if (hit) {
				endPos = hit.point;
			}
			laser.UpdateLaserPoints(startPos,endPos);
		}
	}
}
