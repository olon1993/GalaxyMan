using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLocations : MonoBehaviour, IActionLocations
{

	[SerializeField] private int _locationId;
	private Transform _trans;

	private void Awake() {
		_trans = GetComponent<Transform>();
	}

	public int LocationId { get { return _locationId; } }
	public Transform Trans { get { return _trans; } }
}
