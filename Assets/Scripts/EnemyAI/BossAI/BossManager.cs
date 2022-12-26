using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana
{
	public class BossManager : MonoBehaviour
	{
		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		[SerializeField] protected bool _showDebugLog = false;
		[SerializeField] protected GameObject rndrr;
		[SerializeField] protected GameObject _weapon;
		private bool pausing;
		private Vector3 baseScale;
		private Vector3 weaponScale;
		private Transform _target;
		private int currentLocationId = 0;
		protected float _direction = -1f;

		// Boss Phase Variables
		private IBossAction[] allActions;
		private IBossAction[] possibleActions;
		private IBossAction _currentAction;
		private IBossAction _phaseAction;
		protected IHealth _hp;

		private bool acting;
		private bool active;
		[SerializeField] protected float[] hpPercentageAtPhase;
		[SerializeField] protected float[] pauseBetweenActions;
		[SerializeField] protected GameObject deathEffect;
		private int phase = 0;
		private bool startNextPhase;

		// ending
		private GameObject CMCamSwitch;
		private CamSwitch cmSwitch;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		private void Awake() {
			phase = 0;
			CMCamSwitch = GameObject.FindGameObjectWithTag("CMCamera");
			cmSwitch = CMCamSwitch.GetComponent<CamSwitch>();
			_hp = GetComponent<IHealth>();
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
			CheckHealth();
			if (_currentAction == null) {
				SelectAction();
			}
			if (_currentAction.ActionBusy) {

			} else if (pausing || _currentAction.ActionDelay) {
				UpdateDirection();
			} else {
				if (_showDebugLog) {
					Debug.Log("NEW ACTION SELECTION @ " + _currentAction.EndLocationId);
				}
				UpdateDirection();
				currentLocationId = _currentAction.EndLocationId;
				if (startNextPhase) {
					SetupNextPhase();
				} else {
					SelectAction();
				}
			}
		}

		private void UpdateDirection() {
			_direction = Mathf.Sign(_target.position.x - transform.position.x);
			rndrr.transform.localScale = new Vector3(baseScale.x * _direction, baseScale.y, baseScale.z);
			Weapon.transform.localScale = new Vector3(weaponScale.x * _direction, weaponScale.y, weaponScale.z);
		}

		private void CheckHealth() {
			if (phase < hpPercentageAtPhase.Length) {
				float percentage = (float)_hp.CurrentHealth / (float)_hp.MaxHealth;
				if (percentage < hpPercentageAtPhase[phase]) {
					startNextPhase = true;
				}
			} 
			if (_hp.CurrentHealth <= 0) {
				StartCoroutine(DelaySpawnDeathEffect());
				active = false;
			}
		}

		private void SetupNextPhase() {
			pausing = true;
			startNextPhase = false;
			phase++; 
			if (_phaseAction == null) {
				for (int i = 0; i < allActions.Length; i++) {
					if (allActions[i].PhaseChangeAction) {
						_phaseAction = allActions[i];
						break;
					}
				}
			}
			_currentAction = _phaseAction;
			_currentAction.RunAction(phase);
			pausing = false;
		}

		private IEnumerator DelaySpawnDeathEffect() {
			LevelController lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
			lc.BossIsDefeated();
			yield return new WaitForSeconds(4f);
			Instantiate(deathEffect,transform.position + Vector3.up, Quaternion.identity,this.gameObject.transform);
			yield return new WaitForSeconds(5f);
			cmSwitch.SwitchCamera(0);
		}

		//**************************************************\\
		//******************** Actions *********************\\
		//**************************************************\\

		private void SelectAction() {
			possibleActions = new IBossAction[0];
			for (int i = 0; i < allActions.Length; i++) {
				if (allActions[i].CheckActionPossibility(currentLocationId)) {
					AddPossibleAction(allActions[i]);
				}
			}
			pausing = true;
			bool actionSelected = false;
			while (!actionSelected) {
				int ran = Random.Range(0, possibleActions.Length);
				if (_currentAction != possibleActions[ran]) {
					_currentAction = possibleActions[ran];
					actionSelected = true;
				}
			}
			StartCoroutine(DelayedStartAction());
		}

		private IEnumerator DelayedStartAction() {
			yield return new WaitForSeconds(pauseBetweenActions[phase]);
			_currentAction.RunAction(phase);
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
		public IBossAction CurrentAction { get { return _currentAction; } }
		public IHealth HP { get { return _hp; } }
		public int BossLocationId { get { return currentLocationId; } }
	}
}