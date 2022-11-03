using UnityEngine;

namespace TheFrozenBanana
{
    public class Locomotion3ds : MonoBehaviour, ILocomotion3d
    {
        [SerializeField] private bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        private IInputManager3d _inputManager;
        private CharacterController _characterController;

        // Gravity
        protected float _gravityStrength = -2.98f;
        [SerializeField] protected bool _useGravity;

        // Speed and smoothing
        protected Vector3 _velocity = Vector3.zero;
        protected float _velocityXSmoothing;
        protected float _smoothTimeAirborne = 0.1f;
        protected float _smoothTimeGrounded = 0f;
        [SerializeField] protected float _walkSpeed = 6f;
        [SerializeField] protected float _sprintSpeed = 11f;
        [SerializeField] protected float _wallSlideSpeedMax = 3f;
        [SerializeField] protected float _terminalFallingVelocity = -30f;

        // Rotation
        [SerializeField] private bool _rotateRelativeToCamera;
        [SerializeField] private bool _rotateWithMouse;
        [SerializeField] private Transform _camera;
        [SerializeField] private float _rotationYSpeed = 6f;
        private float _smoothTurnVelocity;
        [SerializeField] private float _smoothTurnTime = 0.1f;
        private float _targetAngle;

        // Jumping
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius;
        private Collider[] _ground;
        [SerializeField] protected float _maxJumpHeight = 2f;
        [SerializeField] protected float _minJumpHeight = 1f;
        [SerializeField] protected float _timeToJumpApex = 0.4f;
        protected float _maxJumpVelocity;
        protected float _minJumpVelocity;
        [SerializeField] private float _coyoteTimeMargin = 0.25f;
        private float _coyoteTime;

        // Inputs / Outputs
        private Vector2 _horizontalPlaneMovement;
        private float _verticalVelocity;
        private bool _isGrounded;
        private bool _isJump;
        private bool _isSprinting;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        #region PublicMethods

        #endregion


        #region UnityMethods

        void Awake()
        {
            _inputManager = GetComponent<IInputManager3d>();
            if (_inputManager == null)
            {
                Debug.LogError("IInputManager not found on " + name);
            }

            _characterController = GetComponent<CharacterController>();
            if (_characterController == null)
            {
                Debug.LogError("CharacterController not found on " + name);
            }
            if (_showDebugLog)
            {
                Debug.Log("Debug log is on. Removing warnings: " + _coyoteTime + " " + _coyoteTimeMargin);
            }
        }

        void Start()
        {
            Init();
        }

        void Update()
        {
            if (_showDebugLog)
            {
                Init();
            }

            _verticalVelocity += _gravityStrength * Time.deltaTime;
            _ground = Physics.OverlapSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
            if (_ground != null && _ground.Length > 0)
            {
                _isGrounded = true;
                _verticalVelocity = -2f;
            }
            else
            {
                _isGrounded = false;
            }

            _isJump = _isGrounded && _inputManager.IsJump;
            if (_isJump)
            {
                _verticalVelocity = _maxJumpVelocity;
            }

            _horizontalPlaneMovement = new Vector2(_inputManager.Horizontal, _inputManager.Vertical);

            if (_horizontalPlaneMovement.magnitude > 0.1)
            {
                Rotate();
                Move();
            }

            _characterController.Move(new Vector3(0f, _verticalVelocity * Time.deltaTime, 0f));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        #endregion


        #region PrivateMethods

        private void Init()
        {
            _gravityStrength = -(2 * _maxJumpHeight) / Mathf.Pow(_timeToJumpApex, 2);
            _maxJumpVelocity = Mathf.Abs(_gravityStrength) * _timeToJumpApex;
            _minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(_gravityStrength) * _minJumpHeight);
            _coyoteTime = -1f;
            if (!_useGravity)
            {
                _gravityStrength = 0;
            }
        }

        private void Rotate()
        {
            if (_rotateRelativeToCamera)
            {
                _targetAngle = Mathf.Atan2(_horizontalPlaneMovement.x, _horizontalPlaneMovement.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _smoothTurnVelocity, _smoothTurnTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            else
            {
                var targetPosition = _inputManager.CurrentTarget;
                targetPosition.y = transform.position.y;
                var targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationYSpeed * Time.deltaTime);
            }
        }

        private void Move()
        {
            float speedModifier = _isSprinting ? _sprintSpeed : _walkSpeed;
            Vector3 direction;
            if (_rotateRelativeToCamera)
            {
                direction = Quaternion.Euler(0f, _targetAngle, _verticalVelocity) * Vector3.forward;
            }
            else
            {
                direction = new Vector3(_horizontalPlaneMovement.x, 0f, _horizontalPlaneMovement.y);
            }

            _characterController.Move(direction.normalized * speedModifier * Time.deltaTime);
        }

        #endregion


        //**************************************************\\
        //****************** Properties ********************\\
        //**************************************************\\

        public Vector2 HorizontalPlaneMovement
        {
            get { return _horizontalPlaneMovement; }
            set
            {
                if (_horizontalPlaneMovement != value)
                {
                    _horizontalPlaneMovement = value;
                }
            }
        }

        public float VerticalVelocity
        {
            get { return _verticalVelocity; }
            set
            {
                if (_verticalVelocity != value)
                {
                    _verticalVelocity = value;
                }
            }
        }

        public float RotationYSpeed
        {
            get { return _rotationYSpeed; }
            set
            {
                if (_rotationYSpeed != value)
                {
                    _rotationYSpeed = value;
                }
            }
        }

        public bool IsGrounded
        {
            get { return _isGrounded; }
        }

        public bool IsJump
        {
            get { return _isJump; }
            set
            {
                if (_isJump != value)
                {
                    _isJump = value;
                }
            }
        }

        public bool IsSprinting
        {
            get { return _isSprinting; }
            set
            {
                if (_isSprinting != value)
                {
                    _isSprinting = value;
                }
            }
        }

        #region DEPRECATED
        public float RunSpeed => throw new System.NotImplementedException();

        public float TurnSpeed => throw new System.NotImplementedException();

        public float JumpSpeed => throw new System.NotImplementedException();

        public float Gravity => throw new System.NotImplementedException();

        public Rigidbody RB => throw new System.NotImplementedException();

        public bool MoveOverride => false;

        public float WalkSpeed => throw new System.NotImplementedException();
        #endregion
    }
}
