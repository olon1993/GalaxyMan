using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IInputManager
    {
        // Output
        float Horizontal { get; }
        float Vertical { get; }
        bool IsJump { get; }
        bool IsJumpCancelled { get; }
        bool IsDash { get; }
        bool IsAttack { get; }
        bool IsAttacking { get; }
        bool IsSecondaryAttack { get; }
        bool IsToggleWeapons { get; }
        bool IsToggleInventory { get; }

        // Input
        bool IsEnabled { get; set; }

        void OverrideHorizontalInput(float val);
        void EndOverride();
    }
}

