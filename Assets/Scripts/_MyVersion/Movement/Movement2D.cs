using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MovementRules2D
{
	[SerializeField] protected GameObject rndrr;
	private Vector3 baseScale;

	protected override void Awake() {
		base.Awake();
		baseScale = rndrr.transform.localScale;
		_input = this.gameObject.GetComponent<IInput>();
		if (_input == null) {
			Debug.LogError("No Input found!");
		}
	}

	protected override void ProcessInput() {
		base.ProcessInput();
		ProcessHorizontalInput();
		ProcessJumpInput();
		ProcessDashInput();
		ProcessFaceDirection();
	}

	protected virtual void ProcessHorizontalInput() {
		float airGroundFactor = Grounded ? 1f : 0.3f;
		// Accelerate
		if (Mathf.Abs(_input.HorizontalInput) > Mathf.Epsilon) {
			if (_wallSliding) {
				_movement.x = 0f;
			}
			if (!_wallSliding || _currentWallStickTime > _wallStickTime) {
				_movement.x += _input.HorizontalInput * acceleration * Time.deltaTime * 2 * airGroundFactor;
			} else {
				if ((WallLeft && _input.HorizontalInput > Mathf.Epsilon) || 
					 WallRight && _input.HorizontalInput < - Mathf.Epsilon) {
					_currentWallStickTime += Time.deltaTime;
				} else {
					_currentWallStickTime = 0f;
				}
				_movement.x = _input.HorizontalInput * maxHorizontalSpeed;
				if (Mathf.Abs(_movement.x) < 0.2f) {
					_movement.x = 0f;
				}
			}
		// Auto-brakes
		} else { 
			_currentWallStickTime = 0f;
			if (Mathf.Abs(_movement.x) > 0.2f && !_wallSliding) {
				_movement.x += -Mathf.Sign(_movement.x) * acceleration * Time.deltaTime * airGroundFactor;
			} else {
				_movement.x = 0f;
			}
		}
	}

	protected void ProcessFaceDirection() {
		if (!_wallSliding) {
			if (Mathf.Abs(_input.HorizontalInput) > Mathf.Epsilon) {
				_moveDirection = Mathf.Sign(_movement.x);
			}
		} else if (WallLeft) {
			_moveDirection = 1f;
			_faceDirection = 1f;
		} else if (WallRight) {
			_moveDirection = -1f;
			_faceDirection = -1f;
		}
		
		rndrr.transform.localScale = new Vector3(baseScale.x * _faceDirection, baseScale.y, baseScale.z);
	}

	protected virtual void ProcessJumpInput() {
		wantsToJump = _input.JumpInput;
		
	}

	protected virtual void ProcessDashInput() {
		wantsToDash = _input.DashInput;
	}
}
