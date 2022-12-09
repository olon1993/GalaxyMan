using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class PCDash : MonoBehaviour, PCAction {

		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\
		[SerializeField] private int _priority = 9;


		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\
		public Vector3 ProcessAction(CollisionInfo ci, IInput _input) {

			return Vector3.zero;
		}

		//**************************************************\\
		//******************* Properties *******************\\
		//**************************************************\\
		public int Priority { get { return _priority; } }
	}
}
