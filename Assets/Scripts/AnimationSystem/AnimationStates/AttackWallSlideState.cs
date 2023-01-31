using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{ 
    public class AttackWallSlideState : AnimationState
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        private ILocomotion2dSideScroller _locomotion;
        private ICombatant _combatant;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        protected override void Awake()
        {
            base.Awake();

            _locomotion = (ILocomotion2dSideScroller)_dependencyManager.Registry[typeof(ILocomotion2dSideScroller)];
            _combatant = (ICombatant)_dependencyManager.Registry[typeof(ICombatant)];
            if (_locomotion == null)
            {
                Debug.LogError("Locomotion not found on " + name);
            }
            if (_combatant == null) {
                Debug.LogError("Combatant not found on " + name);
            }
        }

        public override bool ShouldPlay()
        {
            if ((_locomotion.IsLeftCollision || _locomotion.IsRightCollision) && _locomotion.Velocity.y < -Mathf.Epsilon && (_combatant.IsAttack || _combatant.IsAttacking))
            {
                return true;
            }

            return false;
        }
    }
}
