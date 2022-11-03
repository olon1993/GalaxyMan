using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IHealth
    {
		bool IsDead { get; }
        bool IsHurt { get; }
        bool TakeDamage(Damage damage);
        int MaxHealth { get; set; }
        int CurrentHealth { get; set; }
		void AddHealth(int hp);
    }
}
