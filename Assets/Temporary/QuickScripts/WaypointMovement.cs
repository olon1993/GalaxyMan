using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
	public class WaypointMovement : MonoBehaviour
	{
		private int _nextWaypoint;
		[SerializeField] protected Vector3[] _localWaypoints;
		private Vector3[] _globalWaypoints;
		[SerializeField] private float _speed;
		[SerializeField] private float _pauseAtWaypoint;
		private float _distanceToWaypoint;
		private float _moveDistance;

		private void Awake() 
		{
			_globalWaypoints = new Vector3[_localWaypoints.Length];
			for (int i = 0; i < _globalWaypoints.Length; i++)
			{
				_globalWaypoints[i] = _localWaypoints[i] + transform.position;
			}

			if(_globalWaypoints.Length > 0)
			{
				StartCoroutine(RunMovement());
			}
		}

		private IEnumerator RunMovement() 
		{
			while (true) 
			{
				_moveDistance = _speed * Time.deltaTime;
				_distanceToWaypoint = Vector3.Distance(transform.position, _globalWaypoints[_nextWaypoint]);

				if (_distanceToWaypoint > _moveDistance) 
				{
					Vector3 direction = (_globalWaypoints[_nextWaypoint] - transform.position).normalized * _moveDistance;
					transform.Translate(direction);
				}
				else 
				{
					transform.position = _globalWaypoints[_nextWaypoint];
					_nextWaypoint++;
					if (_nextWaypoint == _globalWaypoints.Length) 
					{
						_nextWaypoint = 0;
					}
					yield return new WaitForSeconds(_pauseAtWaypoint);
				}
				yield return new WaitForEndOfFrame();

			}
		}

		void OnDrawGizmos()
		{
			if (_localWaypoints != null)
			{
				Gizmos.color = Color.red;
				float size = 0.3f;

				for (int i = 0; i < _localWaypoints.Length; i++)
				{
					Vector3 globalWaypointPosition = (Application.isPlaying) ? _globalWaypoints[i] : _localWaypoints[i] + transform.position;
					Gizmos.DrawLine(globalWaypointPosition - Vector3.up * size, globalWaypointPosition + Vector3.up * size);
					Gizmos.DrawLine(globalWaypointPosition - Vector3.left * size, globalWaypointPosition + Vector3.left * size);

				}
			}
            else
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 1);
            }
		}
	}
}
