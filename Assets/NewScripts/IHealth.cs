using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
	bool IsDead { get; }
	bool IsHurt { get; }
	void TakeDamage(IDamage damage);
	int MaxHealth { get; set; }
	int CurrentHealth { get; set; }
	void AddHealth(int hp);
}

