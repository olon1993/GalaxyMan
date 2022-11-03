using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	[SerializeField] private float _totalActionTime = 1f;
	[SerializeField] private float _anticipationTime = 1f;
	[SerializeField] private float _actionDelayTime = 1f;
	[SerializeField] private GameObject _anticipationEffect;
	protected BossManager _bossManager;
	protected bool _actionBusy;
	protected bool _actionDelay;

	private int _startLocationId;
	protected int _endLocationId;
	private int _startInt;
	private int _endInt;

	//**************************************************\\
	//******************** Methods *********************\\
	//**************************************************\\

	protected virtual void Awake() {
		if (_totalActionTime < Mathf.Epsilon) {
			_totalActionTime = 1f;
		}
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


	public bool CheckActionPossibility(int start) {
		bool check = false;
		// Start Location Check
		_startLocationId = start;
		if (_showDebugLog) {
			Debug.Log("Action: "+ _actionName+ ": " + start);
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

	public void RunAction() {
		_actionDelay = true;
		StartCoroutine(CarryOutAction());
	}

	protected IEnumerator CarryOutAction() {
		if (_showDebugLog) {
			Debug.Log("Start Action: " + _actionName);
		}
		GameObject tmp = Instantiate(_anticipationEffect, transform.position, Quaternion.identity, null) as GameObject;
		yield return new WaitForSeconds(_anticipationTime);
		Destroy(tmp, 1f);
		_actionBusy = true;
		_actionDelay = false;
		StartCoroutine(CarryOutSpecificAction());
	}

	protected virtual IEnumerator CarryOutSpecificAction() {
		yield return new WaitForEndOfFrame();
	}

	//**************************************************\\
	//****************** PROPERTIES ********************\\
	//**************************************************\\

	public string ActionName { get { return _actionName; } }
	public bool ShowDebugLog { get { return _showDebugLog; } }
	public BossManager BossManager { get { return _bossManager; } }
	public bool ActionBusy { get { return _actionBusy; } }
	public bool ActionDelay { get { return _actionDelay; } }
	public IActionLocations[] StartLocations { get { return _startLocations; } }
	public IActionLocations[] EndLocations { get { return _endLocations; } }
	public bool CanEndOnSameLocation { get { return _canEndOnSameLocation; } }
	public bool MustEndOnSameLocation { get { return _mustEndOnSameLocation; } }
	public float TotalActionTime { get { return _totalActionTime; } }
	public float AnticipationTime { get { return _anticipationTime; } }
	public float ActionDelayTime { get { return _actionDelayTime; } }
	public GameObject AnticipationEffect { get { return _anticipationEffect; } }
	public int StartLocationId { get { return _startLocationId; } }
	public int EndLocationId { get { return _endLocationId; } }
	public int StartInt { get { return _startInt; } }
	public int EndInt { get { return _endInt; } }
}
