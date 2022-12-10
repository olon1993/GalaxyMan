using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class Projectile2D : MonoBehaviour, IProjectile
	{

		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		[SerializeField] protected bool _showDebugLog;
		[SerializeField] private float _velocity;
		[SerializeField] GameObject _hitEffect;
		[SerializeField] private bool _useGravity;
		[SerializeField] private float _gravity = 8f;

		private IDamage _damage;
		private bool _active;
		private Vector3 _direction;
		private string _ownerTag;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		private void OnTriggerEnter2D(Collider2D col) {
			if (!_active) {
				return;
			}
			if (col.CompareTag(_ownerTag)) {
				return;
			}
			if (_hitEffect != null) {
				Instantiate(_hitEffect, gameObject.transform.position, Quaternion.identity, null);
			}
			IHealth hpScript = col.GetComponent<IHealth>();
			if (hpScript != null) {
				hpScript.TakeDamage(_damage);
			}
			IRecoil recoil = col.GetComponent<IRecoil>();
			if (recoil != null) {
				if (_showDebugLog) {
					Debug.Log("Recoil");
				}
				recoil.ApplyRecoil(_damage.KnockbackForce, gameObject.transform.position - _direction * 5);
			}
			if (_active) {
				Deactivate();
			}
		}

		private void FixedUpdate() {
			if (_useGravity) {
				ApplyGravity();
			}
			gameObject.transform.Translate(Vector3.right * _direction.magnitude * Time.fixedDeltaTime);
		}

		private void ApplyGravity() {
			_direction += Vector3.down * _gravity * Time.fixedDeltaTime;
			float angle = Vector3.Angle(_direction, Vector3.right);
			if (_direction.y < 0) {
				angle = -angle;
			}

			if (_showDebugLog) {
				Debug.Log(gameObject.name + "  : [Direction] = " + _direction + " ; [Z Angle] = " + angle);
			}
			Quaternion euler = Quaternion.Euler(0, 0, angle);
			transform.rotation = euler;
		}

		public void Setup(Vector3 start, Vector3 target, string owner) {
			_damage = gameObject.GetComponent<IDamage>();
			if (_damage == null) {
				Debug.LogError("Damage not found on " + name);
			}
			_ownerTag = owner;
			_direction = (target - start).normalized * _velocity;
			float angle = Vector3.Angle(_direction, Vector3.right);
			if (_direction.y < 0) {
				angle = -angle;
			}

			if (_showDebugLog) {
				Debug.Log(gameObject.name + "  : [Direction] = " + _direction + " ; [Z Angle] = " + angle);
			}
			Quaternion euler = Quaternion.Euler(0, 0, angle);
			transform.rotation = euler;
			_active = true;
			Destroy(gameObject, 5f);
		}


		private void Deactivate() {
			_active = false;
			gameObject.SetActive(false);
		}

		//**************************************************\\
		//******************* Properties *******************\\
		//**************************************************\\

		public IDamage Damage {
			get { return _damage; }
		}

		public bool Active {
			get { return _active; }
			set { _active = value; }
		}

		public Vector3 Direction {
			get { return _direction; }
		}

		public float Velocity {
			get { return _velocity; }

		}

		public string OwnerTag {
			get { return _ownerTag; }
			set {
				if (_ownerTag != value) {
					_ownerTag = value;
				}
			}
		}
	}
}
