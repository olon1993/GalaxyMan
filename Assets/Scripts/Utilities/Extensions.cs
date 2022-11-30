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

        static public float Sign(this float number)
        {
            return number > 0 ? 1f : number < 0 ? -1f : 0f;
        }
    }
}
