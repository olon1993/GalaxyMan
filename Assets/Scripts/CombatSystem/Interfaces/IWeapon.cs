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
        AmmoType AmmoTypeDefinition { get; set; }
        Transform PointOfOrigin { get; set; }
        Transform PointOfTargetting { get; set; }
        float AttackActionTime { get; }
		int AnimationLayer { get; set; }
        float AttackCharge { get; set; }
        void ChargeEffect();

		public enum DamageType
        {
            PHYSICAL, FIRE
        }

        public enum AmmoType
        {
            NONE, MAGIC
        }

        void Attack(float charge);

    }
}
