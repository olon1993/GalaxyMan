using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IHazard
    {
        bool RespawnOnTouch { get; }
        Transform RespawnLocation { get; }

        void ChangeRespawnLocation(Transform newLocation);
    }
}
