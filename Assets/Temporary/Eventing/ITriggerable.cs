using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface ITriggerable 
    {
        bool IsRetriggerable { get; }
        void ExecuteTriggerAction(bool triggerStatus);
    }
}
