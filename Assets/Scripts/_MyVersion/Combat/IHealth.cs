using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana
{
	public interface IHealth
	{
		bool IsDead { get; }
		bool IsHurt { get; set; }
		int MaxHealth { get; set; }
		int CurrentHealth { get; set; }
		float TimeToDie { get; set; }
		void TakeDamage(IDamage damage);
		void AddHealth(int hp);
	}
}