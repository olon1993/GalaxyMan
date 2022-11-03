using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IDialogueEntryBranch 
    {
        string Sentence { get; }
        GameObject NextDialogueEntry { get; }
    }
}