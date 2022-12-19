using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IWeapon
    {
        bool IsLimitedAmmo { get; set; }
        int MaxAmmo { get; set; }
        int CurrentAmmo { get; set; }
        AmmoType AmmoTypeDefinition { get; }
        WeaponType WeaponTypeDefinition { get; }
        Transform PointOfOrigin { get; set; }
        Transform PointOfTargetting { get; set; }
        float AttackActionTime { get; }
		int AnimationLayer { get; set; }
        float AttackCharge { get; set; }

        void Attack(float charge);
        void ChargeEffect();

        public enum AmmoType
        {
            NONE, MAGIC
        }

        public enum WeaponType
		{
            MELEE, RANGED
		}
    }
}
