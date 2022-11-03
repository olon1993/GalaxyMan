using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class Crosshair : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Input.mousePosition;
        }
    }
}
