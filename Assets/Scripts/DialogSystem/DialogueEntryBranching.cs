using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class DialogueEntryBranching : MonoBehaviour, IDialogueEntryBranching
    {
        [SerializeField] private bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        [SerializeField] private string _entryName;
        [SerializeField] private string _speaker;
        [SerializeField] private Sprite _portrait;
        [SerializeField] private string _sentence;
        [SerializeField] private List<GameObject> _dialogueEntryBranches;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        #region PublicMethods

        #endregion


        #region UnityMethods

        void Awake()
        {
        
        }

        void Start()
        {
        
        }

        void Update()
        {
        
        }

        #endregion


        #region PrivateMethods

        #endregion


        //**************************************************\\
        //****************** Properties ********************\\
        //**************************************************\\

        public string Sentence
        {
            get { return _sentence; }
        }

        public List<GameObject> DialogueEntryBranches
        {
            get { return _dialogueEntryBranches; }
        }

        public string EntryName
        {
            get { return _entryName; }
        }

        public string Speaker
        {
            get { return _speaker; }
        }

        public Sprite Portrait
        {
            get { return _portrait; }
        }

    }
}
