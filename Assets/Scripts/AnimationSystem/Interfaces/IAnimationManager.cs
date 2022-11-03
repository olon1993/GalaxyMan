using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IAnimationManager
    {
        IAnimationState CurrentAnimationState { get; }
    }
}
