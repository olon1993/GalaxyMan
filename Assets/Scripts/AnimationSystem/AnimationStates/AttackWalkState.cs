using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class AttackWalkState : AnimationState
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        private ICombatant _combatant;
        private ILocomotion2dSideScroller _locomotion;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        protected override void Awake()
        {
            base.Awake();

            _locomotion = (ILocomotion2dSideScroller)_dependencyManager.Registry[typeof(ILocomotion2dSideScroller)];
            _combatant = (ICombatant)_dependencyManager.Registry[typeof(ICombatant)];
            if (_combatant == null)
            {
                Debug.LogError("ICombatant not found on " + name);
            }
            if (_locomotion == null) {
                Debug.LogError("Locomotion not found on " + name);
            }
        }

        public override bool ShouldPlay()
        {
            if (_combatant.IsAttacking && _locomotion.Velocity.XZPlane().magnitude > float.Epsilon)
            {
                return true;
            }

            return false;
        }

        public override bool CanBeInterrupted
        {
            get
            {
                if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
