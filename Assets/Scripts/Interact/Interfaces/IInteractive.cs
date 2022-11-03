using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;


namespace TheFrozenBanana {
	public interface IInteractive<T> 
	{
		void Interact(T collider);
	}
}