using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IRecoil
    {
        void ApplyRecoil(float knockbackForce, Vector3 forceSource);
    }
}

