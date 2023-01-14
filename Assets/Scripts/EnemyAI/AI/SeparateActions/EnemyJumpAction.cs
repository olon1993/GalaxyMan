using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
 	public class EnemyJumpAction : EnemyAction
	{
		[SerializeField] private GameObject _trailEffect;
		[SerializeField] private GameObject _landingEffect;
		[SerializeField] private Transform _landingEffectLocation;
		private float dir;
		private Animator ac;

		protected override void Awake() {
			base.Awake();
			ac = gameObject.GetComponentInChildren<Animator>();
		}

		protected override IEnumerator CarryOutAction() {
			if (_showDebugLog) {
				Debug.Log("EnemyJumpAction.CarryOutAction");
			}

			StartCoroutine(DelayJumpAnimation());

			_actionInEffect = true;
			float t = 0;
			while (t < 1.5f) {
				yield return new WaitForEndOfFrame();
				t += Time.deltaTime;
				// Turn around
				_locomotion.HorizontalLook = Mathf.Sign(ec.Target.transform.position.x - gameObject.transform.position.x);
				dir = _locomotion.HorizontalLook;
			}

			_inputManager.OverrideHorizontalInput(dir);
			ec.Jump();
			yield return new WaitForSeconds(0.1f);
			while (!_locomotion.IsGrounded) {
				if (dir != Mathf.Sign(ec.Target.transform.position.x - gameObject.transform.position.x)) {
					_inputManager.EndOverride();
				}
				yield return new WaitForEndOfFrame();
			}
			if (_landingEffect != null && _landingEffectLocation != null) {
				GameObject tmp = Instantiate(_landingEffect, _landingEffectLocation.position, Quaternion.identity, null) as GameObject;
				Destroy(tmp, 1f);
			}
			StopAction();
		}

		private IEnumerator DelayJumpAnimation() {
			yield return new WaitForSeconds(0.5f);
			if (ac != null) {
				ac.SetTrigger("ToggleJump");
			}
		}
	}
}
