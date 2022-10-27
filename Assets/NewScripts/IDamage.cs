using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

public interface IDamage
{
	public enum DamageType
	{
		PHYSICAL,
		ARCANE,
		NATURE,
		FIRE,
		WATER,
		EARTH
	}
	DamageType DamageTypeDefinition { get; set; }
	int DamageAmount { get; set; }
	float KnockbackForce { get; set; }
	float StunTime { get; set; }

}