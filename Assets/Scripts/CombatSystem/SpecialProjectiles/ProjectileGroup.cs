using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
	public class ProjectileGroup : MonoBehaviour, IProjectile
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

		[SerializeField] private IMovementPatterns.MovementPattern pattern;
		[SerializeField] private GameObject projectilePrefab;
		[SerializeField] private int projectileCount;
		[SerializeField] private float distanceFromCenter;
		[SerializeField] private float expandTime;
		[SerializeField] private GameObject projectileContainer;
		private GameObject[] liveProjectiles;
		private float lerpI;
		private Vector3 dir;
		private int[] skipProjectile;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		public void Setup(Vector3 start, Vector3 target, string owner) {
			skipProjectile = new int[0];
			// Setup the Group
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

			// Create each projectile in the group
			liveProjectiles = new GameObject[projectileCount];
			for (int i = 0; i < projectileCount; i++) {
				GameObject tmp = Instantiate(projectilePrefab, projectileContainer.transform.position, Quaternion.identity, projectileContainer.transform) as GameObject;
				liveProjectiles[i] = tmp;
				tmp.GetComponent<IProjectile>().Setup(Vector3.zero, Vector3.zero, _ownerTag);
			}
			if (pattern == IMovementPatterns.MovementPattern.EXPAND) {
				StartCoroutine(ExpandProjectileGroup());
			} else if (pattern == IMovementPatterns.MovementPattern.OSCILLATE) {
				StartCoroutine(OscillateProjectileGroup());
			}

			_active = true;
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
		//**************************************************\\
		//******************* Coroutines *******************\\
		//**************************************************\\
		private IEnumerator ExpandProjectileGroup() {
			yield return new WaitForEndOfFrame();
			float mathAngle = 360 / projectileCount;
			float t = 0;
			while (t < expandTime) {
				//  float
				lerpI = t / expandTime;
				for (int i = 0; i < projectileCount; i++) {
					dir = Vector3.up * distanceFromCenter;
					dir = Quaternion.AngleAxis(mathAngle * i, Vector3.forward) * dir;
					if (liveProjectiles[i].activeSelf) {
						liveProjectiles[i].transform.localPosition = Vector3.Lerp(projectileContainer.transform.localPosition, dir, lerpI);
					}
				}
				t += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator OscillateProjectileGroup() {
			yield return new WaitForEndOfFrame();
			float mathAngle = 360 / projectileCount;
			float t = 0;
			bool expand = true;
			while (true) {
				if (expand) {
					t = 0;
					while (t < expandTime) {
						lerpI = t / expandTime;
						for (int i = 0; i < projectileCount; i++) {
							if (ProjectileIsAvailable(i)) {
								continue;
							}
							dir = Vector3.up * distanceFromCenter;
							dir = Quaternion.AngleAxis(mathAngle * i, Vector3.forward) * dir;
							try {
								if (liveProjectiles[i].activeSelf) {
									liveProjectiles[i].transform.localPosition = Vector3.Lerp(projectileContainer.transform.localPosition, dir, lerpI);
								}
							} catch (MissingReferenceException mre) {
								Debug.Log("MRE @ Oscillate: Expand: " + mre);
								continue;
							}

						}
						t += Time.deltaTime;
						yield return new WaitForEndOfFrame();
					}
					expand = false;
				} else {
					t = 0;
					while (t < expandTime) {
						lerpI = t / expandTime;
						for (int i = 0; i < projectileCount; i++) {
							if (ProjectileIsAvailable(i)) {
								continue;
							}
							dir = Vector3.up * distanceFromCenter;
							dir = Quaternion.AngleAxis(mathAngle * i, Vector3.forward) * dir;
							try {
								if (liveProjectiles[i].activeSelf) {
									liveProjectiles[i].transform.localPosition = Vector3.Lerp(dir, projectileContainer.transform.localPosition, lerpI);
								} else {
									AddIndexToSkip(i);
								}
							} catch (MissingReferenceException mre) {
								AddIndexToSkip(i);
								Debug.Log("MRE @ Oscillate: Contract: " + mre);
								continue;
							}
						}
						t += Time.deltaTime;
						yield return new WaitForEndOfFrame();
					}
					expand = true;
				}
			}
		}

		private void AddIndexToSkip(int index) {
			int[] tmp = new int[skipProjectile.Length + 1];
			for (int i = 0; i < skipProjectile.Length; i++) {
				tmp[i] = skipProjectile[i];
			}
			tmp[skipProjectile.Length] = index;
			skipProjectile = tmp;
		}

		private bool ProjectileIsAvailable(int index) {
			if (skipProjectile.Length == projectileCount) {
				Debug.Log("All Projectiles are skipped, Destroy Container");
				Destroy(gameObject);
			}
			for (int i = 0; i < skipProjectile.Length; i++) {
				if (index == skipProjectile[i]) {
					return true;
				}
			}
			return false;
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
