using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana 
{

	public class DeathState : AnimationState {

		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		// Dependencies
		private IHealth _health;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		protected override void Awake()
		{
			base.Awake();

			_health = (IHealth)_dependencyManager.Registry[typeof(IHealth)];
			if (_health == null)
			{
				Debug.LogError("IHealth not found on " + name);
			}
		}

		public override bool ShouldPlay() {
			if (_health.CurrentHealth <= 0f) {
				return true;
			}

			return false;
		}
	}
}
