using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrozenBanana
{
	public class Locomotion2dSideScroller : PhysicsObject2D, ILocomotion2dSideScroller, IRecoil
	{

		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\
		private IInputManager _inputManager;

		// Speed and smoothing
		[SerializeField] protected float _walkSpeed = 6f;
		[SerializeField] protected float _dashSpeed = 11f;
		[SerializeField] protected float _wallSlideSpeedMax = 3f;
		[SerializeField] protected float _terminalFallingVelocity = -30f;

		// Jumping
		[SerializeField] protected float _minJumpHeight = 1f;
		protected float _maxJumpVelocity;
		protected float _minJumpVelocity;
		[SerializeField] private float _coyoteTimeMargin = 0.25f;
		private float _coyoteTime;

		// Wall Jump / Slide
		[SerializeField] protected float _wallStickTime = 0.25f;
		protected float _timeToWallUnstick;
		[SerializeField] protected Vector2 _wallJumpClimb;
		[SerializeField] protected Vector2 _wallJumpOff;
		[SerializeField] protected Vector2 _wallLeap;
		private int _wallDirectionX;
		private bool _isWallSliding;

		// Dash and Stamina
		[SerializeField] private Slider _dashSlider;
		private bool _wantsToDash;
		private bool _dashLock; //, _groundDash, _airDash;
		private float _dashTimer;
		private float _dashX;
		private float _dashY;
		[SerializeField] protected float _dashDuration;
		private bool _isDashing;

		// Damage Force

		private bool _handlingKnockback;

		// Graphics
		private float _horizontalLook = 1;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		protected override void Awake() {
			base.Awake();

			GetDependencies();
		}

		private void GetDependencies() {
			_inputManager = GetComponent<IInputManager>();
			if (_inputManager == null) {
				Debug.Log("No Input Manager found on " + name);
			}

		}

		protected override void Start() {
			base.Start();

			Init();
		}

		protected void OnEnable() {
			_velocity.x = 0;
			_velocity.y = 0;
			HorizontalMovement = 0f;
			_handlingKnockback = false;
		}

		private void Init() {
			_collisions.FaceDirection = 1;
			_gravityStrength = -(2 * _maxJumpHeight) / Mathf.Pow(_timeToJumpApex, 2);
			_maxJumpVelocity = Mathf.Abs(_gravityStrength) * _timeToJumpApex;
			_minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(_gravityStrength) * _minJumpHeight);
			_coyoteTime = -1f;
			if (!_useGravity) {
				_gravityStrength = 0;
			}

		}

        protected override void Update()
        {
            GetInput();

			CalculateVelocity();

			if (!_handlingKnockback) {
				HandleWallSliding();
				HandleJumping();
				FaceDirectionBasedOnInput();
				HandleDash();
			}

			Move(_velocity * Time.deltaTime);

			VerticalCollisionAdjustment();

		}


		private void GetInput() {
			if (_handlingKnockback) {
				return;
			}
			HorizontalMovement = _inputManager.Horizontal;
			VerticalMovement = _inputManager.Vertical;
			IsJumping = _inputManager.IsJump;
			IsJumpCancelled = _inputManager.IsJumpCancelled;
			_wantsToDash = _inputManager.IsDash;
		}

		protected override void CalculateVelocity() {
			float targetVelocityX = HorizontalMovement * _walkSpeed;
			_velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, _collisions.Below ? _smoothTimeGrounded : _smoothTimeAirborne);
			_velocity.y += _gravityStrength * Time.deltaTime;

			if (_velocity.y < _terminalFallingVelocity) {
				_velocity.y = _terminalFallingVelocity;
			}
		}

		//**************************************************\\
		//****************** WALL MOVEMENT *****************\\
		//**************************************************\\
		protected void HandleWallSliding() {

			if (_collisions.Left) {
				_wallDirectionX = -1;
			} else if (_collisions.Right) {
				_wallDirectionX = 1;
			} else {
				_wallDirectionX = 0;
			}

			_isWallSliding = false;

			// Slide down wall
			if ((_collisions.Left || _collisions.Right) && !_collisions.Below && _velocity.y < 0) {
				_isWallSliding = true;

				if (_velocity.y < -_wallSlideSpeedMax) {
					_velocity.y = -_wallSlideSpeedMax;
				}

				// Stick to wall for a short time to perform wall jumps easier
				if (_timeToWallUnstick > 0) {
					_velocityXSmoothing = 0;
					_velocity.x = 0;

					if (HorizontalMovement != _wallDirectionX && HorizontalMovement != 0) {
						_timeToWallUnstick -= Time.deltaTime;
					} else {
						_timeToWallUnstick = _wallStickTime;
					}
				} else {
					_timeToWallUnstick = _wallStickTime;
				}
			}
		}

		//**************************************************\\
		//****************** JUMP MOVEMENT *****************\\
		//**************************************************\\
		private void HandleJumping() {
			if (IsJumping) {
				// Wall Jumps
				if (_isWallSliding) {
					// Jump towards wall that you're sliding down
					if (_wallDirectionX == HorizontalMovement) {
						_velocity.x = -_wallDirectionX * _wallJumpClimb.x;
						_velocity.y = _wallJumpClimb.y;
					}
					// Jump off wall
					else if (HorizontalMovement == 0) {
						_velocity.x = -_wallDirectionX * _wallJumpOff.x;
						_velocity.y = _wallJumpOff.y;
					}
					// Jump away from wall
					else {
						_velocity.x = -_wallDirectionX * _wallLeap.x;
						_velocity.y = _wallLeap.y;
					}
				}

				// Jump from the ground
				if (_collisions.Below || Time.time <= _coyoteTime) {
					// Jump while sliding down a slope
					if (_collisions.SlidingDownMaxSlope) {
						if (HorizontalMovement != -Mathf.Sign(_collisions.SlopeNormal.x)) {
							_velocity.y = _maxJumpVelocity * _collisions.SlopeNormal.y;
							_velocity.x = _maxJumpVelocity * _collisions.SlopeNormal.x;
						}
					}
					// Normal jump
					else {
						_velocity.y = _maxJumpVelocity;
					}
				}
			}

			if (IsJumpCancelled) {
				if (_velocity.y > _minJumpVelocity) {
					_velocity.y = _minJumpVelocity;
				}
			}
		}

		private void FaceDirectionBasedOnInput() {
			if (_inputManager.Horizontal != 0 && !_isWallSliding) {
				HorizontalLook = Mathf.Sign(_inputManager.Horizontal);
			}
		}

		//**************************************************\\
		//****************** DASH MOVEMENT *****************\\
		//**************************************************\\
		protected virtual void HandleDash() 
		{
			// DASH STARTUP
			if (_wantsToDash && !IsDashing && _dashTimer < _dashDuration && !_dashLock) 
			{
				_dashLock = true;
				if (_wallDirectionX < 0) 
				{
					_velocity.x = _dashSpeed;
					_velocity.y = 0f;
				}
				else if (_wallDirectionX > 0) 
				{
					_velocity.x = -_dashSpeed;
					_velocity.y = 0f;
				} 
				else
				{
					if (HorizontalMovement == 0 && VerticalMovement == 0) {
						_velocity.x = HorizontalLook * _dashSpeed;
						_velocity.y = 0f;
					} else { 
						_velocity.x = Movement.normalized.x * _dashSpeed;
						_velocity.y = Movement.normalized.y * _dashSpeed;
					}
				}

				IsDashing = true;
				_dashX = _velocity.x;
				_dashY = _velocity.y;
			}
			// DASH CONTINUATION
			else if (_wantsToDash && IsDashing && _dashTimer < _dashDuration) 
			{
//				Debug.Log("Continue Dash");
				if (_dashTimer < _dashDuration) 
				{
					_dashTimer += Time.deltaTime;
				}

				_velocity.y = _dashY;
				_velocity.x = _dashX;

				if (Mathf.Sign(_dashX) == WallDirectionX) 
				{
					IsDashing = false;
				}

			// DASH END
			} 
			else 
			{
	//			Debug.Log("End Dash");
				IsDashing = false;
				_dashTimer = 0; 
			}
			if (!_wantsToDash && (IsWallSliding || IsGrounded)) 
			{
	//			Debug.Log("Unlock Dash");
				_dashLock = false;
			}

			if (IsDashing) 
			{
				HorizontalLook = Mathf.Sign(_velocity.x);
			}
			
			if (_dashSlider != null) 
			{
				_dashSlider.value = (1 - (_dashTimer / _dashDuration));
			} 
		}
		//**************************************************\\
		//*************** COLLISION DETECTION **************\\
		//**************************************************\\

		private void VerticalCollisionAdjustment() {
			if (_collisions.Above || _collisions.Below) {
				if (_collisions.SlidingDownMaxSlope) {
					_velocity.y += _collisions.SlopeNormal.y * -_gravityStrength * Time.deltaTime;
				} else {
					_velocity.y = 0;
				}
			}
		}


		//**************************************************\\
		//*************** COLLISION DETECTION **************\\
		//**************************************************\\
		protected override void DetectHorizontalCollisions(ref Vector2 moveAmount) {
			float directionX = _collisions.FaceDirection;
			float rayLength = Mathf.Abs(moveAmount.x) + _skinWidth;

			if (Mathf.Abs(moveAmount.x) < _skinWidth) {
				rayLength = 2 * _skinWidth;
			}

			for (int i = 0; i < _horizontalRayCount; i++) {
				Vector2 rayOrigin;

				if (directionX == -1) {
					rayOrigin = _raycastOrigins.BottomLeft;
				} else {
					rayOrigin = _raycastOrigins.BottomRight;
				}

				rayOrigin += Vector2.up * (_horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

				if (_showDebugLog) {
					Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);
				}

				if (hit) {
					if (hit.distance == 0) {
						continue;
					}

					float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

					if (i == 0 && slopeAngle <= _maxSlopeAngle) {
						if (_collisions.DescendingSlope) {
							_collisions.DescendingSlope = false;
							moveAmount = _collisions.PreviousVelocity;
						}

						float distanceToSlopeStart = 0;
						if (slopeAngle != _collisions.PreviousSlopeAngle) {
							distanceToSlopeStart = hit.distance - _skinWidth;
							moveAmount.x -= distanceToSlopeStart * directionX;
						}
						ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
						moveAmount.x += distanceToSlopeStart * directionX;
					}

					if (!_collisions.ClimbingSlope || slopeAngle > _maxSlopeAngle) {
						if (moveAmount.x != 0) {
							moveAmount.x = (hit.distance - _skinWidth) * directionX;
						}

						rayLength = hit.distance;

						if (_collisions.ClimbingSlope) {
							moveAmount.y = Mathf.Tan(_collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
						}

						_collisions.Left = directionX == -1;
						_collisions.Right = directionX == 1;
					}
				}
			}
		}

		protected override void DetectVerticalCollisions(ref Vector2 moveAmount) {
			float directionY = Mathf.Sign(moveAmount.y);
			float rayLength = Mathf.Abs(moveAmount.y) + _skinWidth;

			for (int i = 0; i < _verticalRayCount; i++) {
				Vector2 rayOrigin;

				if (directionY == -1) {
					rayOrigin = _raycastOrigins.BottomLeft;
				} else {
					rayOrigin = _raycastOrigins.TopLeft;
				}

				rayOrigin += Vector2.right * (_verticalRaySpacing * i + moveAmount.x);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);

				if (_showDebugLog) {
					Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);
				}

				if (hit) {
					if (hit.collider.CompareTag("PassableObstacle")) {
						if (directionY == 1 || hit.distance == 0) {
							continue;
						}

						if (_collisions.FallingThroughPlatform) {
							continue;
						}

						if (VerticalMovement == -1) {
							_collisions.FallingThroughPlatform = true;
							Invoke("ResetFallingThroughPlatform", 0.25f);
							continue;
						}
					}

					_collisions.Below = directionY == -1;
					_collisions.Above = directionY == 1;

					if (_collisions.ClimbingSlope) {
						moveAmount.x = moveAmount.y / Mathf.Tan(_collisions.SlopeAngle * Mathf.Rad2Deg) * Mathf.Sign(moveAmount.x);
					}

					moveAmount.y = (hit.distance - _skinWidth) * directionY;
					rayLength = hit.distance;
					_coyoteTime = Time.time + _coyoteTimeMargin;
				}
			}

			if (_collisions.ClimbingSlope) {
				float directionX = Mathf.Sign(moveAmount.x);
				rayLength = Mathf.Abs(moveAmount.x) + _skinWidth;
				Vector2 rayOrigin;

				if (directionX == -1) {
					rayOrigin = _raycastOrigins.BottomLeft;
				} else {
					rayOrigin = _raycastOrigins.BottomRight;
				}

				rayOrigin += Vector2.up * moveAmount.y;
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

				if (hit) {
					float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
					if (slopeAngle != _collisions.SlopeAngle) {
						moveAmount.x = (hit.distance - _skinWidth) * directionX;
						_collisions.SlopeAngle = slopeAngle;
						_collisions.SlopeNormal = hit.normal;
					}
				}
			}
		}

		protected void ClimbSlope(ref Vector2 velocity, float slopeAngle, Vector2 slopeNormal) {
			float moveDistance = Mathf.Abs(velocity.x);
			float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

			if (velocity.y <= climbVelocityY) {
				velocity.y = climbVelocityY;
				velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
				_collisions.Below = true;
				_collisions.ClimbingSlope = true;
				_collisions.SlopeAngle = slopeAngle;
				_collisions.SlopeNormal = slopeNormal;
			}
		}

		protected void ResetFallingThroughPlatform() {
			_collisions.FallingThroughPlatform = false;
		}

		public void ApplyRecoil(float amount, Vector3 source, float stunTime) {
			StartCoroutine(RunRecoil(amount, source, stunTime));
		}

		//**************************************************\\
		//********************* Recoil *********************\\
		//**************************************************\\

		private IEnumerator RunRecoil(float forceValue, Vector3 forceSource, float stunTime) {
			if (_showDebugLog) {
				Debug.Log("Applying Damage Force: " + gameObject.name + " Source: " + forceSource);
			}
			if (stunTime < Mathf.Epsilon) {
				stunTime = 0.1f;
			}
			Vector3 direction = (gameObject.transform.position - forceSource).normalized;
			if (_showDebugLog) {
				Debug.Log("Applying Damage Force: " + gameObject.name + " Source: " + forceSource + " Direction: " + direction);
			}
			_handlingKnockback = true;
			DisruptInput();
			HorizontalMovement = forceValue * direction.x;
			_velocity.y = forceValue * direction.y;
			if (_velocity.y > 0) {
				_velocity.y *= 4;
			}
			yield return new WaitForSeconds(stunTime);
			HorizontalMovement = 0f;
			_handlingKnockback = false;
		}

		private void DisruptInput() {
			IsDashing = false;
			IsJumping = false;
		}

		//**************************************************\\
		//******************* Properties *******************\\
		//**************************************************\\

		public float HorizontalLook {
			get { return _horizontalLook; }
			set {
				if (_horizontalLook != value) {
					_horizontalLook = value;
					transform.localScale = new Vector3(_horizontalLook, 1, 1);
				}
			}
		}

		public float VerticalLook { get { throw new NotImplementedException(); } set { } }

		public bool IsJumping { get; set; }

		public bool IsJumpCancelled { get; set; }

		public bool IsDashing 
		{ 
			get { return _isDashing; }
            set
            {
				if(_isDashing != value)
                {
					_isDashing = value;
					if(_isDashing == false && _velocity.y > 0)
                    {
						_velocity.y = 0;
                    }
				}
            }
		}
		public new bool IsGrounded { get { return _collisions.Below; } }

		public bool IsWallSliding { get { return _isWallSliding; } }

        public int WallDirectionX { get { return _wallDirectionX; } }
    }
}
