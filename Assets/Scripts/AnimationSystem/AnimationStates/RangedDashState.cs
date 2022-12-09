using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{ 
    public class RangedDashState : AnimationState
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
            _locomotion = GetComponentInParent<ILocomotion>();
            if (_locomotion == null)
            {
                Debug.LogError("Locomotion not found on " + name);
            }
        }

        public override bool ShouldPlay()
        {
            if (_locomotion.IsDashing && Mathf.Abs(_locomotion.Velocity.x) > Mathf.Epsilon)
            {
                return true;
            }

            return false;
        }
    }
}
