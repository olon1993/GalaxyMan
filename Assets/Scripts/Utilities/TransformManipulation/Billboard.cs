using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] Camera Camera;

        // Update is called once per frame
        void LateUpdate()
        {
            transform.LookAt(Camera.transform);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }
}
