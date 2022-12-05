using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
	public class EnemyAction : MonoBehaviour, IEnemyAction
	{
		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\
		[SerializeField] protected bool _showDebugLog = false;

		[SerializeField] private int _priority;
		[SerializeField] protected float _actionRange;
		[SerializeField] protected float _actionTime;
		[SerializeField] protected bool _requiresTarget;
		[SerializeField] protected bool _requiresLineOfSight;
		[SerializeField] protected bool _moveAction;
		protected EnemyController ec;
		protected bool _actionPossible = false;
		protected bool _actionInEffect = false;
		[SerializeField] protected bool _actionInterruptable = true;
		protected IInputManager _inputManager;
		protected ILocomotion2dSideScroller _locomotion;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		protected virtual void Awake() {
			ec = gameObject.GetComponent<EnemyController>();
			_inputManager = gameObject.GetComponent<IInputManager>();
			_locomotion = gameObject.GetComponent<ILocomotion2dSideScroller>();
			if (_moveAction) {
				_actionRange = ec.AggroRange;
			}
		}

		public virtual bool CheckActionPossibility(float distance) {

			if (distance < _actionRange || !_requiresTarget) {
				if (_requiresLineOfSight) {
					return LineOfSight();
				}
				return true;
			} else {
				return false;
			}
		}

		public void RunAction() {
			StartCoroutine(CarryOutAction());
		}

		public void StopAction() {
			if (_showDebugLog) {
				Debug.Log("EnemyAction.StopAction");
			}
			ec.EndOverride();
			_actionInEffect = false;
			StartCoroutine(EndAction());
		}

		protected virtual IEnumerator CarryOutAction() {
			if (_showDebugLog) {
				Debug.Log("EnemyAction.CarryOutAction");
			}
			_actionInEffect = true;
			float t = 0;
			while (t < _actionTime) {
				if (!_actionInEffect) {
					break;
				}
				yield return new WaitForEndOfFrame();
				t += Time.deltaTime;
			}
			_actionInEffect = false;
		}

		protected virtual IEnumerator EndAction() {
			yield return new WaitForEndOfFrame();
		}

		protected bool LineOfSight() {
			bool inSight = false; 
			RaycastHit2D hit;
			RaycastHit2D hitUp;
			RaycastHit2D hitDown;
			hit = Physics2D.Raycast(gameObject.transform.position, ec.target.transform.position - gameObject.transform.position);
			hitUp = Physics2D.Raycast(gameObject.transform.position, ec.target.transform.position - gameObject.transform.position + Vector3.up);
			hitDown = Physics2D.Raycast(gameObject.transform.position, ec.target.transform.position - gameObject.transform.position + Vector3.down);
			if (_showDebugLog) { 
				Debug.DrawRay(gameObject.transform.position, ec.target.transform.position - gameObject.transform.position, Color.magenta, 4f);
				Debug.DrawRay(gameObject.transform.position, ec.target.transform.position - gameObject.transform.position + Vector3.up, Color.cyan, 1f);
				Debug.DrawRay(gameObject.transform.position, ec.target.transform.position - gameObject.transform.position + Vector3.down, Color.blue, 1f);
			}
			inSight = CheckHit(hit) ? true : inSight;
			inSight = CheckHit(hitUp) ? true : inSight;
			inSight = CheckHit(hitDown) ? true : inSight;
			return inSight;
		}

		private bool CheckHit(RaycastHit2D hit) {
			if (hit) {
				if (hit.collider.gameObject.tag == "Player") {
					return true;
				}
			}
			return false;
		} 

		//**************************************************\\
		//****************** Properties ********************\\
		//**************************************************\\
		public int Priority {
			get { return _priority; }
		}
		public float ActionRange {
			get { return _actionRange; }
		}
		public float ActionTime {
			get { return _actionTime; }
		}
		public bool ActionPossible {
			get { return _actionPossible; }
		}
		public bool ActionInterruptable {
			get { return _actionInterruptable; }
		}
		public bool RequiresTarget {
			get { return _requiresTarget; }
		}
		public bool MoveAction {
			get { return _moveAction; }
		}
		public bool ActionInEffect {
			get { return _actionInEffect; }
		}
	}
}
