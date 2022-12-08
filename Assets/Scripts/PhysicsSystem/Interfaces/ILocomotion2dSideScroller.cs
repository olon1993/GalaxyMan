using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{

    public interface ILocomotion2dSideScroller : ILocomotion
    {
        bool IsJumping { get; set; }
        bool IsJumpCancelled { get; set; }
        bool IsGrounded { get; }
        bool IsRightCollision { get; }
        bool IsLeftCollision { get; }
        int WallDirectionX { get; }
        bool IsWallSliding { get; }
        float HorizontalLook { get; set; }
    }

}