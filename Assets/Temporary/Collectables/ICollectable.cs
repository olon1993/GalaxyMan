using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana
{
    public interface ICollectable {
        public enum CollectableType {
            AMMO,
            HEALTH
        }
        CollectableType CollectableTypeDefinition { get; }
    }
}
