using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IDialogueEntry
    {
        string EntryName { get; }
        string Speaker { get; }
        Sprite Portrait { get; }
    }
}
