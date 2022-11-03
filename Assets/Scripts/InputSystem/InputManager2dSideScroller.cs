using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{

    public class InputManager2dSideScroller : MonoBehaviour, IInputManager
    {
        [SerializeField] private bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        public bool IsMovementEnabled = true;
        private float _horizontal;
        private float _vertical;
        private bool _isToggleInventory;
        private bool _isJump;
        private bool _isJumpCancelled;
        private bool _isDash;
        private bool _isAttack;
        [SerializeField] private bool _isEnabled;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        #region PublicMethods

        #endregion


        #region UnityMethods

        void Awake()
        {

        }

        void Start()
        {

        }

        void Update()
        {
            if (IsEnabled)
            {
                _horizontal = Input.GetAxisRaw("Horizontal");
                _vertical = Input.GetAxisRaw("Vertical");

                _isJump = Input.GetButtonDown("Jump");
                _isJumpCancelled = Input.GetButtonUp("Jump");

                _isDash = Input.GetButton("Fire3");

                _isAttack = Input.GetButtonDown("Fire1");

                _isToggleInventory = Input.GetKeyDown(KeyCode.I);

                if (_showDebugLog)
                {
                    Debug.Log("Horizontal: " + _horizontal);
                    Debug.Log("Vertical: " + _vertical);
                    Debug.Log("IsJumping: " + _isJump);
                    Debug.Log("IsJumpCancelled: " + _isJumpCancelled);
                    Debug.Log("IsDashing: " + _isDash);
                    Debug.Log("IsAttacking: " + _isAttack);
                    Debug.Log("IsToggleInventory: " + _isToggleInventory);
                }
            }
            else
            {
                _horizontal = 0;
                _vertical = 0;
                _isToggleInventory = false;
                _isJump = false;
                _isJumpCancelled = false;
                _isDash = false;
                _isAttack = false;
            }

        }

        #endregion


        #region PrivateMethods

        #endregion


        //**************************************************\\
        //****************** Properties ********************\\
        //**************************************************\\

        public float Horizontal { get { return _horizontal; } }

        public float Vertical { get { return _vertical; } }

        public bool IsToggleInventory { get { return _isToggleInventory; } }

        public bool IsJump { get { return _isJump; } }

        public bool IsJumpCancelled { get { return _isJumpCancelled; } }

        public bool IsDash { get { return _isDash; } }

        public bool IsAttack { get { return _isAttack; } }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
            }
        }

    }

}
