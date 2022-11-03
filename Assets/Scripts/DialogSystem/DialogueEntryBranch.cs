using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class DialogueEntryBranch : MonoBehaviour, IDialogueEntryBranch
    {
        [SerializeField] private bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        [SerializeField] private string _sentence;
        [SerializeField] private GameObject _nextDialogueEntry;

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

        public GameObject NextDialogueEntry
        {
            get { return _nextDialogueEntry; }
        }

    }
}
