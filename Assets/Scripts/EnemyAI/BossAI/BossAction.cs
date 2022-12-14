using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana
{
	public class BossAction : MonoBehaviour, IBossAction
	{

		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		[SerializeField] private string _actionName = "Jump";
		[SerializeField] protected bool _showDebugLog;
		[SerializeField] private GameObject[] _startLocationsGO; // Can't add Interfaces through Unity editor, so using game objects instead
		[SerializeField] private GameObject[] _endLocationsGO;
		[SerializeField] private IActionLocations[] _startLocations;
		[SerializeField] private IActionLocations[] _endLocations;
		[SerializeField] private bool _canEndOnSameLocation;
		[SerializeField] private bool _mustEndOnSameLocation;
		[SerializeField] private float[] _anticipationTime;
		[SerializeField] private float[] _actionTime;
		[SerializeField] private float[] _recoveryTime;
		[SerializeField] private GameObject _anticipationEffect;
		[SerializeField] protected GameObject _juiceEffect;
		protected BossManager _bossManager;
		protected bool _actionBusy;
		protected bool _actionDelay;

		protected int _startLocationId;
		protected int _endLocationId;
		private int _startInt;
		private int _endInt;
		[SerializeField] private bool _phaseChangeAction = false;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		protected virtual void Awake() {

			_bossManager = gameObject.GetComponent<BossManager>();
			_startLocations = new IActionLocations[_startLocationsGO.Length];
			_endLocations = new IActionLocations[_endLocationsGO.Length];
			for (int i = 0; i < _startLocationsGO.Length; i++) {
				_startLocations[i] = _startLocationsGO[i].GetComponent<IActionLocations>();
			}
			for (int i = 0; i < _endLocationsGO.Length; i++) {
				_endLocations[i] = _endLocationsGO[i].GetComponent<IActionLocations>();
			}
		}


		public virtual bool CheckActionPossibility(int start) {
			bool check = false;
			// Start Location Check
			_startLocationId = start;
			if (_showDebugLog) {
				Debug.Log("Action: " + _actionName + ": " + start);
				Debug.Log("Locations count: " + _startLocations.Length);
			}
			for (int i = 0; i < _startLocations.Length; i++) {
				if (_startLocationId == _startLocations[i].LocationId) {
					check = true;
					break;
				}
			}
			if (!check) { // if start location possibility not found, cannot execute action
				return check;
			};
			// End Location Check
			_endLocationId = _endLocations[Random.Range(0, _endLocations.Length)].LocationId;
			int retry = 0;
			if (_mustEndOnSameLocation) {
				_endLocationId = _startLocationId;
			} else if (!_canEndOnSameLocation) {
				while (_endLocationId == _startLocationId && retry < 10) {
					_endLocationId = _endLocations[Random.Range(0, _endLocations.Length)].LocationId;
					retry++;
				}
			}
			if (retry == 10) { // if end location possibility not found, cannot execute action
				check = false;
			}
			if (_showDebugLog) {
				Debug.Log(_actionName + " Start Id: " + _startLocationId + "; End Id: " + _endLocationId);
			}
			for (int i = 0; i < StartLocations.Length; i++) {
				if (StartLocations[i].LocationId == _startLocationId) {
					_startInt = i;
					break;
				}
			}
			for (int i = 0; i < EndLocations.Length; i++) {
				if (EndLocations[i].LocationId == _endLocationId) {
					_endInt = i;
					break;
				}
			}
			return check;
		}

		public void RunAction(int phase) {
			_actionDelay = true;
			StartCoroutine(CarryOutAction(phase));
		}

		protected IEnumerator CarryOutAction(int phase) {
			if (_showDebugLog) {
				Debug.Log("Start Action: " + _actionName);
			}
			if (_anticipationEffect != null) {
				GameObject tmp = Instantiate(_anticipationEffect, transform.position, Quaternion.identity, null) as GameObject;
				Destroy(tmp, _anticipationTime[phase]);
			}
			yield return new WaitForSeconds(_anticipationTime[phase]);
			_actionBusy = true;
			_actionDelay = false;
			StartCoroutine(CarryOutSpecificAction(phase));
		}

		protected virtual IEnumerator CarryOutSpecificAction(int phase) {
			yield return new WaitForEndOfFrame();
		}

		//**************************************************\\
		//****************** PROPERTIES ********************\\
		//**************************************************\\

		public string ActionName { get { return _actionName; } }
		public bool ShowDebugLog { get { return _showDebugLog; } }
		public BossManager BossManagerScript { get { return _bossManager; } }
		public bool ActionBusy { get { return _actionBusy; } }
		public bool ActionDelay { get { return _actionDelay; } }
		public IActionLocations[] StartLocations { get { return _startLocations; } }
		public IActionLocations[] EndLocations { get { return _endLocations; } }
		public bool CanEndOnSameLocation { get { return _canEndOnSameLocation; } }
		public bool MustEndOnSameLocation { get { return _mustEndOnSameLocation; } }
		public float[] AnticipationTime { get { return _anticipationTime; } }
		public float[] ActionTime { get { return _actionTime; } }
		public float[] RecoveryTime { get { return _recoveryTime; } }
		public GameObject AnticipationEffect { get { return _anticipationEffect; } }
		public int StartLocationId { get { return _startLocationId; } }
		public int EndLocationId { get { return _endLocationId; } }
		public int StartInt { get { return _startInt; } }
		public int EndInt { get { return _endInt; } }
		public bool PhaseChangeAction { get { return _phaseChangeAction; } }
	}
}