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
	[SerializeField] protected GameObject _weapon;
	[SerializeField] protected float pauseBetweenActions = 1f;
	private bool pausing;
	private Vector3 baseScale;
	private Vector3 weaponScale;
	private Transform _target;
	private IBossAction[] allActions;
	private IBossAction[] possibleActions;
	private IBossAction currentAction;

	private bool acting;
	private bool active;
	private int currentLocationId = 0;
	protected float _direction = -1f;

	//**************************************************\\
	//******************** Methods *********************\\
	//**************************************************\\

	private void Awake() {
		baseScale = rndrr.transform.localScale;
		weaponScale = _weapon.transform.localScale;
		_target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		allActions = GetComponents<IBossAction>();
		StartCoroutine(EnterBoss());
	}

	public void Activate() {
		active = true;
	}

	private void Update() {
		if (!active) {
			return;
		}
		if (currentAction == null) {
			SelectAction();
		}
		if (currentAction.ActionBusy) {

		} else if (pausing || currentAction.ActionDelay) {
			UpdateDirection();
		} else {
			if (_showDebugLog) {
				Debug.Log("NEW ACTION SELECTION");
			}
			UpdateDirection();
			currentLocationId = currentAction.EndLocationId;
			SelectAction();
		}
	}

	private void UpdateDirection() {
		_direction = Mathf.Sign(_target.position.x - transform.position.x);
		rndrr.transform.localScale = new Vector3(baseScale.x * _direction, baseScale.y, baseScale.z);
		Weapon.transform.localScale = new Vector3(weaponScale.x * _direction, weaponScale.y, weaponScale.z);
	}

	private void SelectAction() {
		possibleActions = new IBossAction[0];
		for (int i = 0; i < allActions.Length; i++) {
			if (allActions[i].CheckActionPossibility(currentLocationId)) {
				AddPossibleAction(allActions[i]);
			}
		}
		pausing = true;
		currentAction = possibleActions[Random.Range(0, possibleActions.Length)];
		StartCoroutine(DelayedStartAction());
		//currentAction.RunAction();
	}

	private IEnumerator DelayedStartAction() {
		yield return new WaitForSeconds(pauseBetweenActions);
		currentAction.RunAction();
		yield return new WaitForEndOfFrame();
		pausing = false;
	}

	private void AddPossibleAction(IBossAction action) {
		IBossAction[] tmp = new IBossAction[possibleActions.Length + 1];
		for (int i = 0; i < possibleActions.Length; i++) {
			tmp[i] = possibleActions[i];
		}
		tmp[possibleActions.Length] = action;
		possibleActions = tmp;

	}

	private IEnumerator EnterBoss() {
		yield return new WaitForEndOfFrame();
	}

	public GameObject Weapon { get { return _weapon; } }
	public Transform Target { get { return _target; } }
}
