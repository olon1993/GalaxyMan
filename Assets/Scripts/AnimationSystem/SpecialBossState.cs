using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class SpecialBossState : AnimationState
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        private BossManager bm;
        [SerializeField] private string _actionName;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        protected override void Awake() {
            base.Awake();
            bm = GetComponentInParent<BossManager>();
            
        }

        public override bool ShouldPlay() {
            if (bm == null) {
                Debug.Log(_actionName + " + BM is null");
                return false;
            }
            if (bm.CurrentAction == null) {
                Debug.Log(_actionName + " + BM.Action is null");
                return false;
            }
            if (bm.CurrentAction.ActionName == _actionName &&
                bm.CurrentAction.ActionBusy) {
                return true;
            } else {
                return false;
			}

        }
    }
}
