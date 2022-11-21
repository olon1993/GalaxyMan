using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class TurretScript : MonoBehaviour
    {
		public GameObject projectilePrefab;
        public GameObject barrel;
		public Transform projectileSpawnpoint;
		public float maxDistance;
		public float speed;
		public float fireInterval;
		public float fireAnimationDelay;
        private GameObject target;
		private bool openFire;
		private Animator ac;

		private void Awake() {
			target = GameObject.FindGameObjectWithTag("Player");
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
					openFire = true;
				} else {
					Vector3 vectorToTarget = (barrel.transform.position + (Vector3.down)) - barrel.transform.position;
					angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
					openFire = false;
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
				} else {
					t = 0;
				}
				if (t >= fireInterval) {
					t -= fireInterval;
					ac.SetTrigger("Fire");
					yield return new WaitForSeconds(fireAnimationDelay);
					FireProjectile();
				}
			}
		}

		private void FireProjectile() {
			GameObject prj = Instantiate(projectilePrefab, projectileSpawnpoint.position, Quaternion.identity, null) as GameObject;
			Vector3 start = barrel.transform.position;
			Vector3 end = projectileSpawnpoint.position;
			Quaternion q = barrel.transform.rotation;
			q *= Quaternion.Euler(0, 0, 90);
			prj.GetComponent<Projectile2DFreeAim>().Setup(start, end, q, "Enemy");
			Destroy(prj, 5f);
		}
	}
}
