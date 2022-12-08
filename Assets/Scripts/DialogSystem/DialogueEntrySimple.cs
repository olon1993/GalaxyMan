using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class DialogueEntrySimple : MonoBehaviour, IDialogueEntrySimple {
        [SerializeField] private bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        [SerializeField] private string _entryName;
        [SerializeField] private string _speaker;
        [SerializeField] private Sprite _portrait;
        //[SerializeField] private Color _fontColor;

        [TextArea(3, 10)]
        [SerializeField] private string[] _sentences;
        [SerializeField] public GameObject _nextDialogueEntry;

		//**************************************************\\
		//****************** Properties ********************\\
		//**************************************************\\

		private void Awake() {

            if (_showDebugLog) {
                Debug.Log("DialogueEntrySimple");
            }
        }

		public string Speaker {
            get { return _speaker; }
        }

        public Sprite Portrait
        {
            get { return _portrait; }
        }

        public string[] Sentences
        {
            get { return _sentences; }
        }

        public GameObject NextDialogueEntry
        {
            get { return _nextDialogueEntry; }
        }

        public string EntryName { get { return _entryName; } }



        //public Color FontColor
        //{
        //    get { return _fontColor; }
        //}
    }

}
