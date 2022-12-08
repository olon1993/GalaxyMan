using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{ 
    public class RangedWallSlideState : AnimationState
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        private ILocomotion2dSideScroller _locomotion;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        protected override void Awake()
        {
            base.Awake();
            _locomotion = GetComponentInParent<ILocomotion>() as ILocomotion2dSideScroller;
            if (_locomotion == null)
            {
                Debug.LogError("Locomotion not found on " + name);
            }
        }

        public override void OnStateEnter() { }

        public override void OnStateExecute() { }

        public override void OnStateExit() { }

        public override bool ShouldPlay()
        {
            if ((_locomotion.IsLeftCollision || _locomotion.IsRightCollision) && _locomotion.Velocity.y < -Mathf.Epsilon)
            {
                return true;
            }

            return false;
        }
    }
}
