using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IDialogueEntryBranching : IDialogueEntry
    {
        string Sentence { get; }
        List<GameObject> DialogueEntryBranches { get; }
    }
}