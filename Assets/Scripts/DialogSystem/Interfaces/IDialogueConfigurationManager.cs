using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IDialogueConfigurationManager 
    {
        public void Initialize();
        public bool Load();
        public void Save();
        public int TextHeight { get; }
        public Dictionary<char, TextSpriteData> AlphabetDictionary { get; }
    }
}