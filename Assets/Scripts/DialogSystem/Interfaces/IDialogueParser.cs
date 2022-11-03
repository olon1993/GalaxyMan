using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IDialogueParser
    {
        List<DialogueSpriteData> Parse(ref string dialogue);
    }
}
