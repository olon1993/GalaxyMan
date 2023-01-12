using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
	public class EnemyController : MonoBehaviour, IInputManager
	{
		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		[SerializeField] private bool _showDebugLog = false;

		// Input Manager
		public bool IsMovementEnabled = true;
		private float _horizontal;
		private float _vertical;
		private bool _isToggleInventory;
		private bool _isJump;
		private bool _isJumpCancelled;
		private bool _isDash;
		private bool _isAttack;
		private bool _isAttacking;
		private bool _isSecondaryAttack;
		private bool _isToggleWeapons;
		private bool overrideInput;
		[SerializeField] private bool _isEnabled;


		[SerializeField] private float _aggroRange;

		private IEnemyAction[] allActions;
		private IEnemyAction[] possibleActions;
		private IEnemyAction currentAction;
		protected ILocomotion2dSideScroller _locomotion;
		protected GameObject _target;
		private float _distance;
		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\
		private void Awake() {
			_locomotion = gameObject.GetComponent<ILocomotion2dSideScroller>();
			allActions = GetComponents<IEnemyAction>();
			_target = GameObject.FindGameObjectWithTag("Player");
		}

		private void OnDrawGizmosSelected() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _aggroRange);
		}

		private void Update() {
			float distance = Vector3.Distance(transform.position, _target.transform.position);
			_distance = distance;
			if (currentAction == null) {
				DetermineAction(distance);
			}
			if (currentAction.ActionInEffect) {
				if (currentAction.RequiresTarget && _aggroRange < distance) {
					DetermineOverrideAction(distance);

				} else if (currentAction.MoveAction) {
					DetermineOverrideAction(distance);
				}  
			} else {
				DetermineAction(distance);
			}
		}


		private void DetermineAction(float distance) {
			if (_showDebugLog) {
				Debug.Log("EnemyController.DetermineAction");
			}
			possibleActions = new IEnemyAction[0];
			for (int i = 0; i < allActions.Length; i++) {
				if (allActions[i].CheckActionPossibility(distance)) {
					AddPossibleAction(allActions[i]);
				}
			}
			for (int i = 0; i < possibleActions.Length; i++) {
				if (i == 0) {
					currentAction = possibleActions[i];
				}
				if (currentAction.Priority < possibleActions[i].Priority) {
					currentAction = possibleActions[i];
				}
			}
			currentAction.RunAction();
		}

		private void DetermineOverrideAction(float distance) {
			if (!currentAction.ActionInterruptable) {
				return;
			}
			for (int i = 0; i < allActions.Length; i++) {
				if (allActions[i].CheckActionPossibility(distance)) {
					AddPossibleAction(allActions[i]);
				}
			}
			for (int i = 0; i < possibleActions.Length; i++) {
				IEnemyAction newAction = null;
				bool overrideAction = false;
				if (currentAction.Priority < possibleActions[i].Priority) {
					newAction = possibleActions[i];
					overrideAction = true;
				}
				if (overrideAction) {
					currentAction.StopAction();
					currentAction = newAction;
					currentAction.RunAction();
				}
			}
		}

		private void AddPossibleAction(IEnemyAction action) {
			IEnemyAction[] tmp = new IEnemyAction[possibleActions.Length + 1];
			for (int i = 0; i < possibleActions.Length; i++) {
				tmp[i] = possibleActions[i];
			}
			tmp[possibleActions.Length] = action;
			possibleActions = tmp;
		}

		//**************************************************\\
		//************ External Input Overriders ***********\\
		//**************************************************\\

		public void OverrideHorizontalInput(float val) {
			_horizontal = val;
		}

		public void Jump() {
			if (!_isJump && _locomotion.IsGrounded) {
				StartCoroutine(TempJumpInput());
			}
		}

		private IEnumerator TempJumpInput() {
			_isJump = true;
			yield return new WaitForEndOfFrame();
			_isJump = false;
		}

		public void EndOverride() {
			_horizontal = 0;
		}

		public void EnemyAttack(bool setto) {
			_isAttack = setto;
		}

		//**************************************************\\
		//****************** Properties ********************\\
		//**************************************************\\


		public float Horizontal { get { return _horizontal; } }
		public float Vertical { get { return _vertical; } }
		public bool IsToggleInventory { get { return _isToggleInventory; } }
		public bool IsJump { get { return _isJump; } }
		public bool IsJumpCancelled { get { return _isJumpCancelled; } }
		public bool IsDash { get { return _isDash; } }
		public bool IsAttack { get { return _isAttack; } }
		public bool IsAttacking { get { return _isAttacking; } }
		public bool IsSecondaryAttack { get { return _isSecondaryAttack; } }
		public bool IsToggleWeapons { get { return _isToggleWeapons; } }
		public bool IsEnabled {
			get { return _isEnabled; }
			set {
				_isEnabled = value;
			}
		}

		public GameObject Target {
			get { return _target; }
		}
		public float AggroRange {
			get { return _aggroRange; }
		}

	}
}
