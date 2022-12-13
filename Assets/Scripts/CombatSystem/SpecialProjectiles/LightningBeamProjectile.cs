using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class LightningBeamProjectile : MonoBehaviour, IProjectile
	{
		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		[SerializeField] protected bool _showDebugLog;
		[SerializeField] private float _velocity;
		[SerializeField] GameObject _child;
		[SerializeField] GameObject _hitEffect;
		private string _ownerTag;
		private IDamage _damage;
		private Vector3 _direction;
		private bool _active;

		// Lightning stuff
		[SerializeField] private float _refreshRate = 0.12f;
		[SerializeField] private float _vertexDistance = 0.5f;
		[SerializeField] private Vector3 _vertexVariance = new Vector3(0.4f,0.4f,0f);
		[SerializeField] private float _duration = 0.5f;
		private Vector3[] _vertexPoints;
		private LineRenderer line;
		private BoxCollider2D myCollider;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		private void Awake() {
			_vertexPoints = new Vector3[0];
			_damage = gameObject.GetComponent<IDamage>();
			if (_child == null) {
				Debug.LogError(gameObject.name + " is not properly setup! Missing Child!");
			}
			if (_damage == null) {
				Debug.LogError(gameObject.name + " is not properly setup! Missing Damage!");
			}
		}

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
		}

		// Setup gives the target to go to and the rotation of the projectile renderer
		public void Setup(Vector3 start, Vector3 target, string owner) {
			_ownerTag = owner;
			_direction = target - start;
			_child.transform.localPosition = _direction.normalized * _velocity;
			line = gameObject.GetComponent<LineRenderer>();
			myCollider = gameObject.GetComponentInChildren<BoxCollider2D>();
			_active = true;
			if (Mathf.Abs(_direction.x) > Mathf.Abs(_direction.y)) {
				myCollider.size = new Vector2(_velocity, line.startWidth);
			} else {
				myCollider.size = new Vector2(line.startWidth, _velocity);
			}


			Vector3 colliderOffsetLoc = - _direction.normalized * _velocity / 2;

			myCollider.offset = colliderOffsetLoc; // Vector3.Lerp(child.transform.position, colliderEndLoc, 0.5f);
			StartCoroutine(CreateLightningRendering());
			Destroy(this.gameObject, _duration);
		}

		private IEnumerator CreateLightningRendering() {
			Vector3 start = gameObject.transform.position;
			Vector3 end = _child.transform.position;
			float distance = Vector3.Distance(start,end);
			int vertices = Mathf.RoundToInt(distance / _vertexDistance);

			float t = 0;
			bool debugOnce = false;
			while (t < _duration) {
				_vertexPoints = new Vector3[vertices + 1];
				_vertexPoints[0] = start;
				for (int i = 1; i < vertices; i++) {
					float vertexLerp = (float)i / (float)vertices;
					Vector3 origin = Vector3.Lerp(start, end, vertexLerp);
					CreateNewVertex(origin, i);
				}
				_vertexPoints[vertices] = end;
				line.positionCount = _vertexPoints.Length;
				line.SetPositions(_vertexPoints);
				if (debugOnce) {
					debugOnce = false;
					for (int d = 0; d < _vertexPoints.Length; d++) {
						Debug.Log("Point _" + d + "_: " + _vertexPoints[d]);
					}
				}
				yield return new WaitForSeconds(_refreshRate);
				t += _refreshRate;
			}
		}

		private void CreateNewVertex(Vector3 origin, int i) {
			Vector3 randomizer = new Vector3(Random.Range(-_vertexVariance.x, _vertexVariance.x)
											,Random.Range(-_vertexVariance.y, _vertexVariance.y)
											,Random.Range(-_vertexVariance.z, _vertexVariance.z));
			Vector3 newVertex = origin + randomizer;
			_vertexPoints[i] = newVertex;
		}

		// turns off the projectile. Not destroyed here, as it is destroyed in main 
		// should it miss. calling destroy here as well will make destroy be called 
		// twice and creating null reference exception. hence, deactivate.
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

