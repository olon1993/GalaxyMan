using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
	//**************************************************\\
	//********************* Fields *********************\\
	//**************************************************\\

	[SerializeField] protected bool _showDebugLog = false;
	[SerializeField] protected GameObject rndrr;
	private Vector3 baseScale;
	private Transform target;
	private IBossAction[] allActions;
	private IBossAction[] possibleActions;
	private IBossAction currentAction;

	private bool acting;
	private int currentLocationId = 0;
	protected float _direction = -1f;

	//**************************************************\\
	//******************** Methods *********************\\
	//**************************************************\\

	private void Awake() {
		baseScale = rndrr.transform.localScale;
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		allActions = GetComponents<IBossAction>();
	}

	private void Update() {
		if (currentAction != null) {
			if (!currentAction.ActionBusy) {
				if (_showDebugLog) {
					Debug.Log("NEW ACTION SELECTION");
				}
				currentLocationId = currentAction.EndLocationId;
				_direction = Mathf.Sign(target.position.x - transform.position.x);
				rndrr.transform.localScale = new Vector3(baseScale.x * _direction, baseScale.y, baseScale.z);
				SelectAction();
			}
		} else {
			SelectAction();
		}
	}

	private void SelectAction() {
		possibleActions = new IBossAction[0];
		for (int i = 0; i < allActions.Length; i++) {
			if (allActions[i].CheckActionPossibility(currentLocationId)) {
				AddPossibleAction(allActions[i]);
			}
		}
		currentAction = possibleActions[Random.Range(0, possibleActions.Length)];
		currentAction.RunAction();
	}

	private void AddPossibleAction(IBossAction action) {
		IBossAction[] tmp = new IBossAction[possibleActions.Length + 1];
		for (int i = 0; i < possibleActions.Length; i++) {
			tmp[i] = possibleActions[i];
		}
		tmp[possibleActions.Length] = action;
		possibleActions = tmp;

	}
}
