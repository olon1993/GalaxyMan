using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IEnemyAction 
    {
        int Priority { get; }
        float ActionRange { get; }
        float ActionTime { get; }
        bool RequiresTarget { get; }
        bool MoveAction { get; }
        bool ActionPossible { get; }
        bool ActionInEffect { get; }
        bool ActionInterruptable { get; }

        bool CheckActionPossibility(float distance);
        void RunAction();
        void StopAction();
    }
}
