using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrozenBanana
{


    public class AmmoSlider : MonoBehaviour
    {
        public GameObject player;
		public ICombatant cmb;
		public IWeapon weaponDisplayed;
		public Slider ammoSlider;
		public GameObject ammoSliderObject;
		public bool ready = false;

		private void Awake() {
			ammoSliderObject = ammoSlider.gameObject;
			StartCoroutine(FindPlayer());
		}
		
		private IEnumerator FindPlayer() {
			while (player == null) {
				yield return new WaitForSeconds(1f);
				player = GameObject.FindGameObjectWithTag("Player");
			}
			cmb = player.GetComponent<ICombatant>();
			weaponDisplayed = cmb.CurrentWeapon;
			if (!cmb.CurrentWeapon.IsLimitedAmmo) {
				DisplaySlider(false);
			}
			ready = true;
		}

		private void Update() {
			if (!ready) {
				return;
			}
			if (weaponDisplayed != cmb.CurrentWeapon) {
				weaponDisplayed = cmb.CurrentWeapon;
				if (weaponDisplayed.IsLimitedAmmo) {
					DisplaySlider(true);
				} else {
					DisplaySlider(false);
				}
			} else if (!weaponDisplayed.IsLimitedAmmo) {
				return;
			}
			ammoSlider.value = weaponDisplayed.CurrentAmmo;
		}

		private void DisplaySlider(bool act) {
			ammoSliderObject.SetActive(act);
			if (act) {
				ammoSlider.maxValue = weaponDisplayed.MaxAmmo;
			}
		}
	}

}
