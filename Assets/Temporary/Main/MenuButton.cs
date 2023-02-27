using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class MenuButton : MonoBehaviour
    {
        public void ButtonPressed() {
			Debug.Log("Button pressed");
			GameController.gc.StartLevel();
		}
	}
}
