using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IDropLoot
	{
		float DropChance { get; }
		GameObject[] LootList { get; }
		float[] LootWeightedChance { get; }
		void DropRandomLoot();
	}
}
