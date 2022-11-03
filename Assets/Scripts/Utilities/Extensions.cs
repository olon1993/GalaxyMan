using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public static class Extensions
    {
        static public Vector3 XZPlane(this Vector3 vec)
        {
            return new Vector3(vec.x, 0f, vec.z);
        }
    }
}
