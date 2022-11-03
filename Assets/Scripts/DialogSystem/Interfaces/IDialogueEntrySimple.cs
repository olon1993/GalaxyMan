using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IDialogueEntrySimple : IDialogueEntry
    {
        string[] Sentences { get; }
        GameObject NextDialogueEntry { get; }
    }
}