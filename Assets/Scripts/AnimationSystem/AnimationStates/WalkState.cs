using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class WalkState : AnimationState
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

            _locomotion = (ILocomotion2dSideScroller)_dependencyManager.Registry[typeof(ILocomotion2dSideScroller)];
            if (_locomotion == null)
            {
                Debug.LogError("Locomotion not found on " + name);
            }
        }

        public override bool ShouldPlay()
        {
            if (_locomotion.Velocity.XZPlane().magnitude > float.Epsilon)
            {
                return true;
            }

            return false;
        }
    }
}
