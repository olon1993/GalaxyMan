using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{

	public interface IProjectile
	{
		IDamage damage { get; set; }
		GameObject child { get; set; }
		GameObject hitEffect { get; }
		Vector3 direction { get; set; }
		float velocity { get; set; }
		bool active { get; set; }
		void TriggerCollision(Collider2D col);
		void Setup(Vector3 start, Vector3 target, Quaternion rotation, string ownerTag);
	}

}
