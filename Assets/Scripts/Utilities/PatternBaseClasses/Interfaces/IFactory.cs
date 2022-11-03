using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IFactory<T>
    {
        T New(Transform parent);
    }
}
