using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class CamSwitch : MonoBehaviour
    {
        private Animator anim;
		private int currentCam;
		[SerializeField] private string[] camStateNames;

		private void Awake() {
			currentCam = 0;
			anim = GetComponent<Animator>();
		}
		public void SwitchCamera() {
			currentCam++;
			anim.Play(camStateNames[currentCam]);
		}
    }
}
