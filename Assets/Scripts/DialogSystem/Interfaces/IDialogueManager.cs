using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IDialogueManager
    {
        void Initiate(IDialogueTree dialogue);
    }
}
