using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheFrozenBanana
{
    public class DialogueTree3d : CollisionInteraction3d, IDialogueTree
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        private IDialogueManager _dialogueManager;
        [SerializeField] private GameObject _startDialogueEntry;

        // Configuration
        [SerializeField] private AudioSource _dialogueSound;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        protected override void Awake()
        {
            base.Awake();

            _dialogueManager = FindObjectsOfType<MonoBehaviour>().OfType<IDialogueManager>().FirstOrDefault();
            if (_dialogueManager == null)
            {
                Debug.LogError("No IDialogueManager found in scene. " + name + " needs an IDialogueManager to fuction properly.");
            }
        }

        public override void Interact(Collider collider)
        {
            if (_dialogueSound != null)
            {
                _dialogueSound.Play();
            }

            _dialogueManager.Initiate(this);
        }


        //**************************************************\\
        //****************** Properties ********************\\
        //**************************************************\\

        public GameObject StartDialogueEntry
        {
            get { return _startDialogueEntry; }
        }
    }
}
