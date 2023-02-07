using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface ICombatant
    {
        IHealth Health { get; set; }
        IList<IWeapon> MainWeapons { get; set; }
        IWeapon CurrentMainWeapon { get; set; }
        bool IsAttack { get; set; }
        bool IsAttacking { get; set; }
        bool FeignAttack { get; set; }
        int HorizontalFacingDirection { get; set; }
    }
}
