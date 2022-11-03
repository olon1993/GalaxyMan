using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface ILocomotion
    {
        float HorizontalMovement { get; set; }
        float VerticalMovement { get; set; }
        public bool IsDashing { get; set; }
        Vector3 Movement { get; }
        Vector3 Velocity { get; }
    }
}
