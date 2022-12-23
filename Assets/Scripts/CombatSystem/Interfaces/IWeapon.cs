using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IWeapon
    {
        string WeaponName { get; }
        bool IsLimitedAmmo { get; set; }
        int MaxAmmo { get; set; }
        int CurrentAmmo { get; set; }
        AmmoType AmmoTypeDefinition { get; }
        WeaponType WeaponTypeDefinition { get; }
        Transform PointOfOrigin { get; set; }
        Transform PointOfTargetting { get; set; }
        float AttackActionTime { get; }
		int AnimationLayer { get; set; }
        float AttackSpeed { get; }
        bool IsAttacking { get; }
        bool IsUnlocked { get; }

        void Attack(float charge);
        void ChargeEffect();
        void UnlockWeapon();

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
