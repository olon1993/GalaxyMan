using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana
{
    public interface ILocomotion3d
    {
        Vector2 HorizontalPlaneMovement { get; set; }
        float VerticalVelocity { get; set; }
        bool IsGrounded { get; }
        bool MoveOverride { get; }
    }
}
