using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class PCGround : MonoBehaviour, PCAction {

		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\
		[SerializeField] private int _priority = 1;


		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\
		public Vector3 ProcessAction(CollisionInfo ci, IInput _input) {
			if (!ci.Below) {
				return Vector3.zero;
			}
			return Vector3.zero;
		}

		//**************************************************\\
		//******************* Properties *******************\\
		//**************************************************\\
		public int Priority { get { return _priority; } }
	}
}
