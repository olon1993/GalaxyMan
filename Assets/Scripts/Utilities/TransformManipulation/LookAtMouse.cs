using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class LookAtMouse : MonoBehaviour
    {

        [SerializeField] private Transform gameObjectTransform;

        void FixedUpdate()
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                if (hitInfo.collider != null)
                {
                    gameObjectTransform.rotation = Quaternion.LookRotation(hitInfo.point - gameObjectTransform.position);
                }
            }
        }
    }
}
