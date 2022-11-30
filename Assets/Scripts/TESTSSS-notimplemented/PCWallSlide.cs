using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
	public class PCWallSlide : MonoBehaviour, PCAction {

		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\
		[SerializeField] private int _priority = 7;


		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\
		public Vector3 ProcessAction(CollisionInfo ci, IInput _input) {
			if (!ci.Left && !ci.Right) {
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
