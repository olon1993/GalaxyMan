using UnityEngine;

namespace TheFrozenBanana
{
	public class Projectile3d : MonoBehaviour
	{
		[SerializeField] private bool _showDebugLog = false;

		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		// Dependencies
		private IDamage _damage;
		private Rigidbody _rigidbody;

		[SerializeField] int DamageAmount;
		[SerializeField] public float ProjectileSpeed;
		[SerializeField] private LayerMask _collisionMask;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		private void Awake() {
			_rigidbody = transform.GetComponent<Rigidbody>();
			if (_rigidbody == null) {
				Debug.LogError("No Rigidbody found on " + name);
			}

			_damage = GetComponent<IDamage>();
			if (_damage == null) {
				Debug.LogError("Damage not found on " + name);
			}
		}

		void Start() {
			_rigidbody.velocity = gameObject.transform.forward * ProjectileSpeed;
		}

		private void OnTriggerEnter(Collider col) {
			if (col.gameObject.layer.Equals(_collisionMask) == false) {
				return;
			}

			// TODO: WTF?
			IHealth health = col.GetComponent<IHealth>();
			if (health == null) {
				Destroy(gameObject);
				return;
			}

			if (_showDebugLog) {
				Debug.Log(gameObject.name + " attacks dealing " + _damage.DamageAmount + " damage to " + col.gameObject.name + "!");
			}

			health.TakeDamage(_damage);

			if (_showDebugLog) {
				Debug.Log(col.gameObject.name + " health = " + health.CurrentHealth + " / " + health.MaxHealth);

			}
			IRecoil recoil = col.GetComponent<IRecoil>();
			if (recoil != null) {
				if (_showDebugLog) {
					Debug.Log("Recoil");
				}
				float damageDirection = transform.position.x < col.transform.position.x ? 1 : -1;
				Vector2 closest = col.ClosestPoint(gameObject.transform.position);
				Vector3 src = new Vector3(closest.x, closest.y, 0);

				recoil.ApplyRecoil(_damage.KnockbackForce, src, _damage.StunTime);
			}
			Destroy(gameObject);
		}
	}
}
