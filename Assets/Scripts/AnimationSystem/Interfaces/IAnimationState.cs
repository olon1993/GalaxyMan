using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IAnimationState : IComparable<IAnimationState>
    {
        bool ShouldPlay();
        void OnStateEnter();
        void OnStateExecute();
        void OnStateExit();
        bool CanBeInterrupted { get; }
        int Priority { get; }
    }
}
