using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{

	public interface IProjectile
	{
		IDamage Damage { get; }
		bool Active { get; set; }
		Vector3 Direction { get; }
		float Velocity { get; }
		string OwnerTag { get; set; }
		void Setup(Vector3 start, Vector3 target, string ownerTag);
	}

}
