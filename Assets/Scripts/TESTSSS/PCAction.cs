using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface PCAction
    {
        int Priority { get; }
        Vector3 ProcessAction(CollisionInfo ci, IInput _input);
    }
}
