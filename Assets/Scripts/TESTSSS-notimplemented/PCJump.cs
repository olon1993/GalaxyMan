using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class PCJump : MonoBehaviour, PCAction {

		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\
		[SerializeField] private int _priority = 8;


		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\
		public Vector3 ProcessAction(CollisionInfo ci, IInput _input) {
			Vector3 effect = Vector3.zero;
			if (!ci.Left && !ci.Right && !ci.Above && !ci.Below) {
				effect = Vector3.zero;
			}
			return effect;
		}

		//**************************************************\\
		//******************* Properties *******************\\
		//**************************************************\\
		public int Priority { get { return _priority; } }
	}
}
