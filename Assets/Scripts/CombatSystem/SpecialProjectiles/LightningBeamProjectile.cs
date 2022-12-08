using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class LightningBeamProjectile : MonoBehaviour, IProjectile
    {
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

		public void TriggerCollision(Collider2D col) {
			RunCollisionCheck(col);
		}

		// This is the actual OnTriggerEnter2D, but it's called from the child
		private void RunCollisionCheck(Collider2D col) {
			if (!active) {
				return;
			}
			if (col.CompareTag(ownerTag)) {
				return;
			}
			if (_hitEffect != null) {
				Instantiate(_hitEffect, gameObject.transform.position, Quaternion.identity, null);
			}
			IHealth hpScript = col.GetComponent<IHealth>();
			if (hpScript != null) {
				// Do damage
				hpScript.TakeDamage(_damage);
				HandleDamageForce(col);
			}
		}

		private void HandleDamageForce(Collider2D col) {
			float damageDirection = transform.position.x < col.transform.position.x ? 1 : -1;

		}

		// only handles movement if active
		private void FixedUpdate() {
			if (active) {
		//		gameObject.transform.Translate(direction.normalized * velocity * Time.fixedDeltaTime);
			}
		}

		// Setup gives the target to go to and the rotation of the projectile renderer
		public void Setup(Vector3 start, Vector3 target, Quaternion projectileRotation, string owner) {
			ownerTag = owner;
			direction = target - start;
			Debug.Log("Direction: " + direction);
			child.transform.localPosition = direction.normalized * velocity;
			line = gameObject.GetComponent<LineRenderer>();
			myCollider = gameObject.GetComponentInChildren<BoxCollider2D>();
			active = true;
			if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
				myCollider.size = new Vector2(velocity, line.startWidth);
			} else {
				myCollider.size = new Vector2(line.startWidth, velocity);
			}
			Debug.Log("Local: " + child.transform.localPosition);
			Debug.Log("World: " + child.transform.position);


			Vector3 colliderOffsetLoc = - direction.normalized * velocity / 2;

			myCollider.offset = colliderOffsetLoc; // Vector3.Lerp(child.transform.position, colliderEndLoc, 0.5f);
			StartCoroutine(CreateLightningRendering());
			Destroy(this.gameObject, _duration);
		}

		private IEnumerator CreateLightningRendering() {
			Vector3 start = gameObject.transform.position;
			Vector3 end = child.transform.position;
			float distance = Vector3.Distance(start,end);
			int vertices = Mathf.RoundToInt(distance / _vertexDistance);
			Debug.Log("Start: " + start);
			Debug.Log("End: " + end);
			Debug.Log("Distance: " + distance + "  ; Vertices: " + vertices);

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
			active = false;
			gameObject.SetActive(false);
		}

		// Getters and Setters from interface
		public IDamage damage {
			get { return _damage; }
			set { _damage = value; }
		}

		public GameObject child {
			get { return _child; }
			set { _child = value; }
		}

		public GameObject hitEffect {
			get { return _hitEffect; }
		}

		public string ownerTag {
			get { return _ownerTag; }
			set { _ownerTag = value; }
		}

		public Vector3 direction {
			get { return _direction; }
			set { _direction = value; }
		}

		public bool active {
			get { return _active; }
			set { _active = value; }
		}

		public float velocity {
			get { return _velocity; }
			set { _velocity = value; }

		}

	}
}
