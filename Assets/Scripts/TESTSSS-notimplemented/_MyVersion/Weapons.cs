using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{

	public class Weapons : MonoBehaviour
	{
		private IInput _input;
		private MovementRules2D mover;
		private bool charging;
		private bool firing;
		private float chargeTime;
		[SerializeField] protected string owner;
		[SerializeField] protected GameObject hand;
		[SerializeField] protected GameObject[] _projectilesBySize;
		[SerializeField] protected float _maxChargeTime;
		[SerializeField] protected float[] _chargeAnnotations;
		private string[] firingAnnotations;
		private Vector3 firingDirection;

		private void Awake() {
			_input = gameObject.GetComponentInParent<IInput>();
			if (_input == null) {
				Debug.LogError("No Input found on weapon!");
			}
			mover = gameObject.GetComponentInParent<MovementRules2D>();
			firingAnnotations = new string[3];
			firingAnnotations[0] = "BurstFire";
			firingAnnotations[1] = "SemiChargeFire";
			firingAnnotations[2] = "ChargeFire";
		}

		private void Update() {
			ProcessInput();
		}

		public void ProcessInput() {
			if (_input.WeaponInput && !charging && !firing) {
				StartCoroutine(ChargeWeapon());
			}
			if (Mathf.Abs(_input.VerticalInput) > Mathf.Epsilon) {
				if (_input.VerticalInput > 0) {
					hand.transform.localRotation = Quaternion.Euler(0, 0, 90);
					firingDirection = Vector3.up;
				} else {
					hand.transform.localRotation = Quaternion.Euler(0, 0, -90);
					firingDirection = Vector3.down;
				}
			} else {
				hand.transform.localRotation = Quaternion.Euler(0, 0, 0);
				firingDirection = Vector3.right * mover.FaceDirection;
			}
		}

		private IEnumerator ChargeWeapon() {
			charging = true;
			chargeTime = 0f;
			while (_input.WeaponInput) {
				if (chargeTime < _maxChargeTime) {
					chargeTime += Time.deltaTime;
				} else {
					chargeTime = _maxChargeTime;
				}
				yield return new WaitForEndOfFrame();
			}
			firing = true;
			charging = false;
			FireWeapon();
		}

		private void FireWeapon() {
			StartCoroutine(UseWeapon());
		}

		private IEnumerator UseWeapon() {
			for (int i = 0; i < _chargeAnnotations.Length; i++) {
				if (chargeTime < _chargeAnnotations[i]) {
					StartCoroutine(firingAnnotations[i]);
					break;
				}
			}
			yield return new WaitForEndOfFrame();
		}

		private IEnumerator BurstFire() {
			for (int i = 0; i < 3; i++) {
				ShootProjectile(0);
				yield return new WaitForSeconds(0.2f);
			}
			yield return new WaitForEndOfFrame();
			firing = false;
		}

		private IEnumerator SemiChargeFire() {
			ShootProjectile(1);
			yield return new WaitForSeconds(0.2f);
			firing = false;
		}

		private IEnumerator ChargeFire() {
			ShootProjectile(2);
			yield return new WaitForSeconds(0.2f);
			firing = false;
		}

		private void ShootProjectile(int size) {

			Vector3 projectileDirection = gameObject.transform.position + firingDirection;
			GameObject tmp = Instantiate(_projectilesBySize[size], gameObject.transform.position, Quaternion.identity, null) as GameObject;
			tmp.GetComponent<IProjectile>().Setup(gameObject.transform.position, projectileDirection, Quaternion.identity, owner);
		}
	}

}
