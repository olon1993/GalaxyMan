using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class EnemyFireAction : EnemyAction
    {
		[SerializeField] private GameObject[] _projectileWeapon;
		[SerializeField] private float _aimSpeed;
		[SerializeField] private GameObject _chargeParticleEffect;
		[SerializeField] private int shotsPerVolley = 1;
		[SerializeField] private float volleyTime = 1;
		private int currentWeaponId = 0;

		protected override IEnumerator CarryOutAction() {
			if (_showDebugLog) {
				Debug.Log("EnemyFireAction.CarryOutAction");
			}
			_actionInEffect = true;
			float t = 0;
			float angle = 0;
			if (_chargeParticleEffect != null) {
				foreach (GameObject weap in _projectileWeapon) {
					GameObject partSys = Instantiate(_chargeParticleEffect, weap.transform.position, Quaternion.identity, weap.transform) as GameObject;
					Destroy(partSys, _actionTime * (shotsPerVolley/_projectileWeapon.Length));
				}
			}
			while (t < _actionTime) {
				if (!_actionInEffect) {
					if (_showDebugLog) {
						Debug.Log("EnemyFireAction.StopAction");
					}
					break;
				}
				// Turn around
				_locomotion.HorizontalLook = Mathf.Sign(ec.Target.transform.position.x - gameObject.transform.position.x);

				// Angle handling
				foreach (GameObject weap in _projectileWeapon) {
					Vector3 vectorToTarget = ec.Target.transform.position - weap.transform.position;
					angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
					if (ec.gameObject.transform.localScale.x < 0) {
						angle += 180;
					}
					Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
					weap.transform.rotation = Quaternion.Slerp(weap.transform.rotation, q, Time.deltaTime * _aimSpeed);
				}
				t += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			/*
			 *			if (_actionInEffect) {

							ec.EnemyAttack(true);
							yield return new WaitForEndOfFrame();
							ec.EnemyAttack(false);
							_inputManager.EndOverride();

							}*/
			int shotsFired = 0;
			while (shotsFired < shotsPerVolley) {
				float shotTime = volleyTime / (float)shotsPerVolley;
				foreach (GameObject weap in _projectileWeapon) {
					weap.GetComponent<IWeapon>().Attack(shotTime);
					yield return new WaitForSeconds(shotTime);
					shotsFired++;
					if (shotsFired >= shotsPerVolley) {
						break;
					}
				}
			}
			StopAction();
		}

		protected override IEnumerator EndAction() {

			float angle = 0;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			float t = 0;
			while (t < 1) {
				if (this._actionInEffect) {
					yield break;
				}
				foreach (GameObject weap in _projectileWeapon) {
					weap.transform.rotation = Quaternion.Slerp(weap.transform.rotation, q, t);
				}
				t += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			foreach (GameObject weap in _projectileWeapon) {
				weap.transform.rotation = Quaternion.Slerp(weap.transform.rotation, q, 1);
			}
		}
	}
}

