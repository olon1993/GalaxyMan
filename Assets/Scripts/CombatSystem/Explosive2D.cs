using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
	public class Explosive2D : MonoBehaviour
	{

		[SerializeField] protected bool _showDebugLog;
		[SerializeField] protected float _explosionRadius;
		protected CircleCollider2D circle;
		private IDamage _damage;
		

		private void Awake() {
			_damage = GetComponent<IDamage>();
			circle = GetComponent<CircleCollider2D>();
			StartCoroutine(ExplosionCalculation());
		}

		private IEnumerator ExplosionCalculation() {
			float t = 0;
			float i = 0;
			while (t < 0.4f) {
				i = t / 0.4f;
				circle.radius = Mathf.Lerp(0, _explosionRadius, i);
				t += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			circle.radius = _explosionRadius;
			yield return new WaitForSeconds(0.3f);
			circle.enabled = false;
			Destroy(this.gameObject, 0.4f);
		}

		private void OnTriggerEnter2D(Collider2D col) {

			IHealth hpScript = col.GetComponent<IHealth>();
			if (hpScript != null) {
				hpScript.TakeDamage(_damage);
			}
			IRecoil recoil = col.GetComponent<IRecoil>();
			if (recoil != null) {
				if (_showDebugLog) {
					Debug.Log("Recoil");
				}
				recoil.ApplyRecoil(_damage.KnockbackForce, gameObject.transform.position, _damage.StunTime);
			}
		}
	}
}