using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class EnemyFireAction : EnemyAction
    {
		[SerializeField] private GameObject _projectileWeapon;
		[SerializeField] private float _aimSpeed;
		[SerializeField] private GameObject _chargeParticles;

		protected override IEnumerator CarryOutAction() {
			if (_showDebugLog) {
				Debug.Log("EnemyFireAction.CarryOutAction");
			}
			_actionInEffect = true;
			float t = 0;
			float angle = 0;
			if (_chargeParticles != null) {
				GameObject partSys = Instantiate(_chargeParticles, _projectileWeapon.transform.position, Quaternion.identity, _projectileWeapon.transform) as GameObject;
				Destroy(partSys,_actionTime);
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
				Vector3 vectorToTarget = ec.Target.transform.position - _projectileWeapon.transform.position;
				angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
				if (ec.gameObject.transform.localScale.x < 0) {
					angle += 180;
				}
				Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
				_projectileWeapon.transform.rotation = Quaternion.Slerp(_projectileWeapon.transform.rotation, q, Time.deltaTime * _aimSpeed);

				t += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			
			if (_actionInEffect) {
				ec.EnemyAttack(true);
				yield return new WaitForEndOfFrame();
				ec.EnemyAttack(false);
				_inputManager.EndOverride();
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
				_projectileWeapon.transform.rotation = Quaternion.Slerp(_projectileWeapon.transform.rotation, q, t);
				t += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			_projectileWeapon.transform.rotation = Quaternion.Slerp(_projectileWeapon.transform.rotation, q, 1);
		}
	}
}

