using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class DisableMe : MonoBehaviour
    {
        public void TurnMeOff()
        {
            gameObject.SetActive(false);
        }
    }
}
