using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class JumpStraightUpState : AnimationState
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        private ILocomotion _locomotion;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        protected override void Awake()
        {
            base.Awake();

            _locomotion = (ILocomotion)_dependencyManager.Registry[typeof(ILocomotion)];
            if (_locomotion == null)
            {
                Debug.LogError("Locomotion not found on " + name);
            }
        }

        public override bool ShouldPlay()
        {
            if (_locomotion.Velocity.y > Mathf.Epsilon && Mathf.Abs(_locomotion.Velocity.x) <= Mathf.Epsilon)
            {
                return true;
            }

            return false;
        }
    }
}
