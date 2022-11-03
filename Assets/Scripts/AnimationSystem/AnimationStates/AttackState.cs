using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class AttackState : AnimationState
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        private ICombatant _combatant;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        protected override void Awake()
        {
            base.Awake();

            _combatant = (ICombatant)_dependencyManager.Registry[typeof(ICombatant)];
            if (_combatant == null)
            {
                Debug.LogError("ICombatant not found on " + name);
            }
        }

        public override bool ShouldPlay()
        {
            if (_combatant.IsAttacking)
            {
                return true;
            }

            return false;
        }

        public override bool CanBeInterrupted
        {
            get
            {
                if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
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
