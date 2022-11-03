using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class Transition : MonoBehaviour
    {
        [SerializeField] Transform toDisable;
        [SerializeField] Transform toEnable;
        [SerializeField] float Delay;
        private bool isActive = true;

        private float currentTime = 0;

        // Update is called once per frame
        void Update()
        {
            if (isActive)
            {
                currentTime += Time.deltaTime;

                if (currentTime >= Delay)
                {
                    toDisable.gameObject.SetActive(false);
                    toEnable.gameObject.SetActive(true);
                    isActive = false;
                }
            }
        }
    }
}
