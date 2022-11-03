using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{ 
    public class WallSlideState : AnimationState
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

            _locomotion = (ILocomotion2dSideScroller)_dependencyManager.Registry[typeof(ILocomotion)];
            if (_locomotion == null)
            {
                Debug.LogError("Locomotion not found on " + name);
            }
        }

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
