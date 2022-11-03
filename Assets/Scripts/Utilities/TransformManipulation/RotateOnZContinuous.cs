using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class RotateOnZContinuous : MonoBehaviour
    {
        [SerializeField] float RotationSpeed = 3f;

        void FixedUpdate()
        {
            transform.Rotate(Vector3.forward * RotationSpeed);
        }
    }
}
