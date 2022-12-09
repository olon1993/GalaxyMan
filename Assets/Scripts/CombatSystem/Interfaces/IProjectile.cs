using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{

	public interface IProjectile
	{
		void Setup(Vector3 start, Vector3 target, Quaternion rotation, string ownerTag);
	}

}
