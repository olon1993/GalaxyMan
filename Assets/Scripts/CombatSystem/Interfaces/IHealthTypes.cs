using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana
{
	public interface IHealthTypes : IHealth
	{
		IDamage.DamageType[] ResistanceTypeDefinition { get; }
		IDamage.DamageType[] VulnerableTypeDefinition { get; }
		float TimeInvulnerable { get; } // -> time after hit that can't be hit again
	}
}