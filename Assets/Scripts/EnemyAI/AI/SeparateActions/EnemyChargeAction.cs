using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class EnemyChargeAction : EnemyAction
    {

		[SerializeField] private float _preChargeTime;
		[SerializeField] private float _maxChargeTime;
		[SerializeField] private float _stopChargeTime;
		[SerializeField] private float _chargeSpeedFactor;
		private float dir;

		protected override void Awake() {
			base.Awake();
			_actionTime = _preChargeTime + _maxChargeTime + _stopChargeTime;
		}

		protected override IEnumerator CarryOutAction() {
			if (_showDebugLog) {
				Debug.Log("EnemyChargeAction.CarryOutAction");
			}
			_actionInEffect = true;
			_locomotion.HorizontalLook = Mathf.Sign(ec.target.transform.position.x - gameObject.transform.position.x);
			dir = _locomotion.HorizontalLook;
			yield return new WaitForSeconds(_preChargeTime);

			float t = 0;
			bool crashed = false;
			while (t < _maxChargeTime) {
				_inputManager.OverrideHorizontalInput(dir * _chargeSpeedFactor);
				yield return new WaitForEndOfFrame();
				t += Time.deltaTime;

				if ((_locomotion.IsLeftCollision && dir < 0 ) || (_locomotion.IsRightCollision && dir > 0)) {
					crashed = true;
					break;
				}
			}
			if (crashed) {
				if (_showDebugLog) {
					Debug.Log("EnemyChargeAction.EnemyCrashed");
				}
				_inputManager.OverrideHorizontalInput(-dir);
				ec.Jump();
			} else {
				if (_showDebugLog) {
					Debug.Log("EnemyChargeAction.EnemyStopping");
				}
				_inputManager.OverrideHorizontalInput(dir);
			}
			yield return new WaitForSeconds(_stopChargeTime / 2);
			_inputManager.EndOverride();
			yield return new WaitForSeconds(_stopChargeTime / 2);
			StopAction();
		}
	}
}
