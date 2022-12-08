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

        protected override void Awake()
        {
            base.Awake();

            _inputManager = GetComponent<IInputManager>();
            if (_inputManager == null)
            {
                Debug.Log("No Input Manager found on " + name);
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
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

        public bool IsDashing { get; set; }

    }
}
