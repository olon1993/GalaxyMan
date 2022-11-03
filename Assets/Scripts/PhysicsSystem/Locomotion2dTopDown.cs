using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{

    public class Locomotion2dTopDown : PhysicsObject2D, ILocomotion
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        private IInputManager _inputManager;

        [SerializeField] private float _speed = 6;


        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        #region PublicMethods

        #endregion


        #region UnityMethods

        void Awake()
        {
            base.Awake();

            _inputManager = GetComponent<IInputManager>();
            if (_inputManager == null)
            {
                Debug.Log("No Input Manager found on " + name);
            }
        }

        void Start()
        {

        }

        void Update()
        {
            GetInput();

            Move(new Vector2(HorizontalMovement, VerticalMovement) * _speed * Time.deltaTime);
        }

        #endregion


        #region PrivateMethods


        private void GetInput()
        {
            HorizontalMovement = _inputManager.Horizontal;
            VerticalMovement = _inputManager.Vertical;
        }

        #endregion


        //**************************************************\\
        //****************** Properties ********************\\
        //**************************************************\\

        public float HorizontalMovement { get; set; }
        public float VerticalMovement { get; set; }
        public bool IsDashing { get; set; }
        public Vector3 Movement { get; }
        public Vector3 Velocity { get; }

    }
}
