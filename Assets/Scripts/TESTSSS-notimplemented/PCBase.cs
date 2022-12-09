using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class PCBase : MonoBehaviour
    {
		[SerializeField] private bool _showDebugLog = false;
		private Vector3 desiredMovement = Vector3.zero;
		private Vector3 physicsMovement = Vector3.zero;
		private Vector3 velocity = Vector3.zero;
		private CollisionInfo ci;
		private IInput _input;

		private PCAction[] sortedActions;

		protected void Awake() {
			//base.Awake();
			sortedActions = new PCAction[0];
			PrioritizeActions();
		}

		private void Update() {
			/*
			physicsMovement = velocity;
			GetInput();
			ProcessActions();
			TranslateMovement();
			Move();*/
		}

		private void GetInput() {

		}

		private void ProcessActions() {
			for (int i = 0; i < sortedActions.Length; i++) {
				 sortedActions[i].ProcessAction(ci, _input);
			}
		}

		private void TranslateMovement() {
			velocity += physicsMovement + desiredMovement;
		}

		private void Move() {
			PostProcessing();
		}

		private void PostProcessing() {

		}

		private void PrioritizeActions() {
			PCAction[] tmpActions = gameObject.GetComponents<PCAction>();
			if (_showDebugLog) {
				Debug.Log("Actions found: " + tmpActions.Length);
			}
			for (int i = 0; i < tmpActions.Length; i++) {
				if (_showDebugLog) {
					Debug.Log("i: " + i + " - starting check");
				}
				int prio = 0;
				PCAction currentAction = tmpActions[i];
				for (int j = 0; j < tmpActions.Length; j++) {
					if (_showDebugLog) {
						Debug.Log("j: " + j +" - found priority: "+ tmpActions[j].Priority);
					}
					bool skip = false;
					if (tmpActions[j].Priority > prio) {
						if (sortedActions.Length > 0) {
							for (int k = 0; k < sortedActions.Length; k++) {
								if (_showDebugLog) {
									Debug.Log("k: " + k + " - check if priority exists");
								}
								if (sortedActions[k].Priority == tmpActions[j].Priority) {
									skip = true;
									if (_showDebugLog) {
										Debug.Log("k: " + k + " - priority exists, skip");
									}
								}
							}
							if (!skip) {
								if (_showDebugLog) {
									Debug.Log(" - adding priority");
								}
								prio = tmpActions[j].Priority;
								currentAction = tmpActions[j];
							}
						}
					}
				}
				AddAction(currentAction);
			}
			if (_showDebugLog) {
				for (int i = 0; i < sortedActions.Length; i++) {
					Debug.Log("Action Priorities: " + sortedActions[i].Priority);
				}
			}
		}

		private void AddAction(PCAction action) {
			PCAction[] tmp = new PCAction[sortedActions.Length + 1];
			for (int i = 0; i < sortedActions.Length; i++) {
				tmp[i] = sortedActions[i];
			}
			tmp[sortedActions.Length] = action;
			sortedActions = tmp;
		}
	}

	public struct CollisionInfo
	{
		public bool Above, Below, Left, Right;
		public bool Airborne; 

	}

}
