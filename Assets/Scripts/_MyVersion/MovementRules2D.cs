using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRules2D : ColliderBounding2D
{

	//**************************************************\\
	//********************* Fields *********************\\
	//**************************************************\\

	protected Vector2 _movement = Vector2.zero; // Desired movement
	protected Vector2 _velocity = Vector2.zero; // Actual movement
	// Collider
	protected ColliderInfo _colliderInfo;
	protected Transform trans;

	// Vertical
	[SerializeField] protected bool _useGravity = true;
	[SerializeField] protected float gravity = 36f;
	[SerializeField] protected float jumpSpeed = 14f;
	[SerializeField] protected float maxJumpHoldTime = 0.4f;
	[SerializeField] protected float maxFallSpeed = 18f;
	[SerializeField] protected float minJumpTime = 0.1f;
	protected bool jumping;
	protected bool jumpLock;
	protected bool wantsToJump;
	protected float jumpTimer;

	// Horizontal
	[SerializeField] protected float acceleration = 5f;
	[SerializeField] protected float maxHorizontalSpeed = 5f;
	[SerializeField] protected float maxDashHoldTime = 0.6f;
	[SerializeField] protected float dashSpeed = 15f;
	protected float _direction = 1f;
	protected bool dashing;
	protected bool wantsToDash;
	protected bool dashLock;
	protected bool groundDash;
	protected bool airDash;
	protected float dashTimer;

	// Walls
	[SerializeField] protected float _maxSlopeAngle = 46f;
	[SerializeField] protected float _wallStickTime = 0.5f;
	protected bool _wallSliding;
	protected float _currentWallStickTime = 0f;


	//**************************************************\\
	//******************** Methods *********************\\
	//**************************************************\\
	protected override void Awake() {
		base.Awake();
		trans = this.gameObject.transform;
	}

	protected virtual void Update() {
		ProcessInput();
	}

	protected virtual void FixedUpdate() {
		_colliderInfo.ResetCollider();
		UpdateBounds();
		DetectHorizontalCollisions();
		DetectVerticalCollisions();
		DetermineWallSliding();
		ProcessDash();
		ProcessJump();
		DetermineMovement();
		Move();
	}

	protected virtual void ProcessInput() {

	}

	protected void DetermineMovement() {
		if (dashing) {

		} else if (!Grounded) {

		} else if (Mathf.Abs(_movement.x) > maxHorizontalSpeed) {
			_movement.x = maxHorizontalSpeed * Mathf.Sign(_movement.x);
		}
		if (!Grounded) {
			if (_wallSliding) {
				_movement.y += -gravity * Time.fixedDeltaTime / 3;
			} else if (airDash) {
				_movement.y = 0f;
			} else { 
				_movement.y += -gravity * Time.fixedDeltaTime;
			}
			if (_movement.y < -maxFallSpeed) {
				_movement.y = -maxFallSpeed;
			}
		}
		_velocity = _movement;
		if (!jumping && !Grounded && WallLeft && _velocity.x > Mathf.Epsilon && _currentWallStickTime < _wallStickTime) {
			_velocity.x = 0f;
		}
		if (!jumping && !Grounded && WallRight && _velocity.x < - Mathf.Epsilon && _currentWallStickTime < _wallStickTime) {
			_velocity.x = 0f;
		}
		if (WallLeft && _velocity.x < -Mathf.Epsilon) {
			_velocity.x = 0f;
		}
		if (WallRight && _velocity.x > Mathf.Epsilon) {
			_velocity.x = 0f;
		}
		if (Grounded && !jumping) {
			ConvertVelocityToSlope();
		}
		if (_showDebugLog) {
			Debug.DrawRay(trans.position,_movement, Color.blue);
			Debug.DrawRay(trans.position,_velocity.normalized, Color.magenta);
			Debug.DrawRay(trans.position,Vector2.right * _direction, Color.cyan);
		}
	}

	protected virtual void Move() {

		trans.Translate(_velocity * Time.fixedDeltaTime);
	}

	protected void ConvertVelocityToSlope() {
		if (_colliderInfo.meanSlopeAngle > Mathf.Epsilon) {
			_velocity.y = _velocity.x * Mathf.Tan(_colliderInfo.meanSlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(_colliderInfo.SlopeNormal.x) * - 0.5f;
			_velocity.x = _velocity.x + (_velocity.y * Mathf.Sign(_colliderInfo.SlopeNormal.x));
		}
	}

	//**************************************************\\
	//****************** JUMP MOVEMENT *****************\\
	//**************************************************\\
	protected virtual void ProcessJump() {
		if (!jumping && Grounded && wantsToJump && !jumpLock) {
			if (_showDebugLog) {
				Debug.Log("Jump Ground");
			}
			jumpLock = true;
			_movement.y = jumpSpeed;
			jumping = true;
		} else if (!jumping && _wallSliding && wantsToJump && !jumpLock) {
			jumpLock = true;
			jumping = true;
			_wallSliding = false;
			_movement.y = jumpSpeed;
			if (WallLeft) {
				if (_movement.x > 0f) {
					_movement.x = maxHorizontalSpeed;
					if (_showDebugLog) {
						Debug.Log("Jump Wall Left Hard" + ": " + _movement.x);
					}
				} else if (_movement.x == 0f) {
					_movement.x = maxHorizontalSpeed * (2f / 3f);
					if (_showDebugLog) {
						Debug.Log("Jump Wall Left Normal" + ": " + _movement.x);
					}
				} else {
					_movement.x = maxHorizontalSpeed * (1f / 3f);
					if (_showDebugLog) {
						Debug.Log("Jump Wall Left Soft" + ": " + _movement.x);
					}
				}
			} else if (WallRight) {
				if (_movement.x < 0f) {
					_movement.x = -maxHorizontalSpeed;
					if (_showDebugLog) {
						Debug.Log("Jump Wall Right Hard" + ": " + _movement.x);
					}
				} else if (_movement.x == 0f) {
					_movement.x = -maxHorizontalSpeed * (2f / 3f);
					if (_showDebugLog) {
						Debug.Log("Jump Wall Right Normal" + ": " + _movement.x);
					}
				} else {
					_movement.x = -maxHorizontalSpeed * (1f / 3f);
					if (_showDebugLog) {
						Debug.Log("Jump Wall Right Soft" + ": " + _movement.x);
					}
				}
			}
		} else if (jumping && jumpTimer < maxJumpHoldTime && wantsToJump && jumpLock) {
			_movement.y = jumpSpeed;
			jumpTimer += Time.deltaTime;
		} 
		if (jumpTimer > minJumpTime && !wantsToJump) {
			jumping = false;
			jumpTimer = 0f;
		}
		if (!wantsToJump) {
			jumpLock = false;
		}
	}
	//**************************************************\\
	//****************** DASH MOVEMENT *****************\\
	//**************************************************\\

	protected virtual void ProcessDash() {
		if (wantsToDash && !dashing && dashTimer < maxDashHoldTime && !dashLock) {
			dashLock = true;
			if (Grounded) {
				_movement.x = Mathf.Sign(_direction) * dashSpeed;
				groundDash = true;
			} else {
				if (WallLeft) {
					_currentWallStickTime = 10f;
					_movement.x = Mathf.Sign(1) * dashSpeed;
				} else if (WallRight) {
					_currentWallStickTime = 10f;
					_movement.x = Mathf.Sign(-1) * dashSpeed;
				} else {
					_movement.x = Mathf.Sign(_direction) * dashSpeed;
				}
				airDash = true;
			}
			dashing = true;
		} else if (wantsToDash && dashing && dashTimer < maxDashHoldTime) {
			if (dashTimer < maxDashHoldTime) {
				dashTimer += Time.fixedDeltaTime;
			}
			if (airDash) {
				_movement.y = 0f;
			}
			_movement.x = Mathf.Sign(_direction) * dashSpeed;
		} else {
			dashing = false;
			dashTimer -= Time.fixedDeltaTime;
			if (dashTimer < 0f) {
				dashTimer = 0f;
			}
			airDash = false;
			groundDash = false;
		}
		if (!wantsToDash && (_wallSliding || Grounded)) {
			dashLock = false;
		}
	}

	//**************************************************\\
	//*************** COLLISION DETECTION **************\\
	//**************************************************\\

	private void DetectHorizontalCollisions() {
		HorizontalRaycheck();
	}

	private void DetectVerticalCollisions() {
		VerticalRaycheck();
	}

	private void HorizontalRaycheck() {
		for (int i = 1; i < _horizontalRayCount - 1; i++) {
			// RIGHT CHECK
			if (_velocity.x > -Mathf.Epsilon) {
				Vector2 rayOrigin = _colliderCorners.BottomRight + new Vector2(-_edgeCheckWidth, i * _horizontalRaySpacing);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.right, _edgeCheckWidth * 2, collisionMask);
				if (hit) {
					_colliderInfo.Right = true;
					jumping = false;
				}
				if (_showDebugLog) {
					Debug.DrawRay(rayOrigin, Vector3.right * (_edgeCheckWidth * 2), Color.red);
				}
			}
			if (_velocity.x < Mathf.Epsilon) {
				// LEFT CHECK
				Vector2 rayOrigin = _colliderCorners.BottomLeft + new Vector2(+_edgeCheckWidth, i * _horizontalRaySpacing);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.left, _edgeCheckWidth * 2, collisionMask);
				if (hit) {
					_colliderInfo.Left = true;
					jumping = false;
				}
				if (_showDebugLog) {
					Debug.DrawRay(rayOrigin, Vector3.left * (_edgeCheckWidth * 2), Color.red);
				}
			}
		}

	}

	private void VerticalRaycheck() {
		_colliderInfo.raysToGround = 0;
		for (int i = 0; i < _verticalRayCount; i++) {
			// BOTTOM CHECK
			Vector2 rayOrigin = _colliderCorners.BottomLeft + new Vector2(i * _verticalRaySpacing, +_edgeCheckWidth);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.down, _edgeCheckWidth * 2, collisionMask);
			if (hit) {
				if (_movement.y < -Mathf.Epsilon) {
					_movement.y = 0;
				}
				ProcessFloorAngle(hit);

				_colliderInfo.raysToGround += 1;
				jumping = false;
			}
			if (_showDebugLog) {
				Debug.DrawRay(rayOrigin, Vector3.down * (_edgeCheckWidth * 2), Color.red);
			}

			// TOP CHECK
			rayOrigin = _colliderCorners.TopLeft + new Vector2(i * _verticalRaySpacing, -_edgeCheckWidth);
			hit = Physics2D.Raycast(rayOrigin, Vector3.up, _edgeCheckWidth * 2, collisionMask);
			if (hit) {
				_colliderInfo.Above = true;
				if (_movement.y > Mathf.Epsilon) {
					_movement.y = 0;
				}
				jumping = false;
			}
			if (_showDebugLog) {
				Debug.DrawRay(rayOrigin, Vector3.up * (_edgeCheckWidth * 2), Color.red);
			}
		}
		if (_colliderInfo.raysToGround > 0) {
			_colliderInfo.Below = true;
		}
		if (_colliderInfo.raysToGround == 0) {
			_colliderInfo.meanSlopeAngle = 0;
		}
	}

	//**************************************************\\
	//******************** SLOPING *********************\\
	//**************************************************\\

	protected void ProcessFloorAngle(RaycastHit2D hit) {
		if (hit) {
			_colliderInfo.AddAngleToSlopes(Vector2.Angle(hit.normal, Vector2.up));
			_colliderInfo.SlopeNormal = hit.normal;
		}
	}


	//**************************************************\\
	//**************** WALL SLIDING ********************\\
	//**************************************************\\
	protected void DetermineWallSliding() {
		if (!WallLeft && !WallRight || Grounded) {
			_wallSliding = false;
			return;
		} else {
			_wallSliding = true;
		}
	}

	//**************************************************\\
	//******************* Properties *******************\\
	//**************************************************\\
	public struct ColliderInfo
	{
		public bool Right, Left, Above, Below;
		public float[] SlopeAngles;
		public float meanSlopeAngle;
		public Vector2 SlopeNormal;
		public int raysToGround;
		public void AddAngleToSlopes(float angle) {
			meanSlopeAngle = angle;
		}

		public void ResetCollider() {
			Right = Left = Above = Below = false;
			SlopeAngles = new float[0];
		}
	}

	public bool Grounded { get { return _colliderInfo.Below; } }
	public bool Ceiling { get { return _colliderInfo.Above; } }
	public bool WallLeft { get { return _colliderInfo.Left; } }
	public bool WallRight { get { return _colliderInfo.Right; } }
	public float Direction { get { return _direction; } }
}
