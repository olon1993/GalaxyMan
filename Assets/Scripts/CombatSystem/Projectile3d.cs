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
		private Damage _damage;
		private Rigidbody _rigidbody;

		[SerializeField] int DamageAmount;
		[SerializeField] IWeapon.DamageType DamageTypeDefinition;
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

			_damage = GetComponent<Damage>();
			if (_damage == null) {
				Debug.LogError("Damage not found on " + name);
			}
		}

		void Start() {
			_rigidbody.velocity = gameObject.transform.forward * ProjectileSpeed;
		}

		private void OnTriggerEnter(Collider collider) {
			if (collider.gameObject.layer.Equals(_collisionMask) == false) {
				return;
			}

			IHealth health = collider.GetComponent<IHealth>();
			if (health == null) {
				Destroy(gameObject);
				return;
			}

			if (_showDebugLog) {
				Debug.Log(gameObject.name + " attacks dealing " + _damage.DamageAmount + " damage to " + collider.gameObject.name + "!");
			}

			health.TakeDamage(_damage);

			if (_showDebugLog) {
				Debug.Log(collider.gameObject.name + " health = " + health.CurrentHealth + " / " + health.MaxHealth);

			}

			Destroy(gameObject);
		}
	}
}
