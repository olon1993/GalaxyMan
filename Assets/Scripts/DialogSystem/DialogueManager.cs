using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrozenBanana
{
    public class DialogueManager : MonoBehaviour, IDialogueManager
    {
        [SerializeField] private bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        private IDialogueParser _dialogueParser;
        private IDialogueRenderer _dialogueRenderer;
        private IDialogueConfigurationManager _dialogueConfigurationManager;

        // Flow options
        private bool _isDialogueActive = false;
        [SerializeField] private float _continueWaitTime = 1f;
        private float _continueTimer = 0;

        // Dialogue Visuals
        [SerializeField] private Transform _dialogueBox;
        [SerializeField] private Transform _dialoguePanel;
        [SerializeField] private Transform _speakerPanel;
        [SerializeField] private Animator _dialogueAnimator;

        // Spacing options
        [SerializeField] private int _letterSpacing = 2;
        [SerializeField] private int _wordSpacing = 8;
        [SerializeField] private int _lineSpacing = 8;
        [SerializeField] private int _panelPadding = 10;
        private float _dialoguePanelWidth = 0;
        private float _speakerPanelWidth = 0;

        [SerializeField] private Color _defaultFontColor;
        [SerializeField] private Color _currentFontColor;

        private Queue<string> _conversation;
        private string _speaker;
        [SerializeField] private Image _portrait;

        private IDialogueTree _currentDialogue;
        private IDialogueEntry _currentDialogueEntry;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        #region PublicMethods

        public void Initiate(IDialogueTree dialogue) {

            if (_showDebugLog) {
                Debug.Log("DialogueManager.Initiate");
            }
            _currentDialogue = dialogue;
            _currentDialogueEntry = dialogue.StartDialogueEntry.GetComponent<IDialogueEntry>();

            _dialogueAnimator.SetTrigger("Transition");
            _isDialogueActive = true;

            ResolveDialogueEntry(_currentDialogueEntry);
        }

        #endregion

        #region UnityMethods

        private void Awake()
        {
            _dialogueConfigurationManager = GetComponent<IDialogueConfigurationManager>();
            if (_dialogueConfigurationManager == null)
            {
                Debug.LogError("IDialogueConfigurationManager not found on " + name);
            }

            _dialogueParser = GetComponent<IDialogueParser>();
            if (_dialogueParser == null)
            {
                Debug.LogError("IDialogueParser not found on " + name);
            }

            _dialogueRenderer = GetComponent<IDialogueRenderer>();
            if (_dialogueRenderer == null)
            {
                Debug.LogError("IDialogueRenderer not found on " + name);
            }
        }

        private void Start()
        {
            _dialoguePanelWidth = _dialoguePanel.GetComponent<RectTransform>().rect.width;
            _speakerPanelWidth = _speakerPanel.GetComponent<RectTransform>().rect.width;

            _conversation = new Queue<string>();
        }

        private void Update()
        {
            if (_isDialogueActive && _continueTimer < _continueWaitTime)
            {
                _continueTimer += Time.deltaTime;
                return;
            }

            // Get rid of Input.GetkeyDown
            // TODO needs to be rethought for linked list dialogue
            if (_isDialogueActive && Input.GetKeyDown(KeyCode.E))
            {
                _continueTimer = 0;
                ResolveSentence();
            }
        }

        #endregion

        #region PrivateMethods

        private void ResolveName()
        {
            ClearSpeakerBox();

            _currentFontColor = _defaultFontColor;
            TextSpriteData textSpriteData = new TextSpriteData(0, 0, 0, 0, null);
            int initialPositionX = (int)(_speakerPanel.transform.localPosition.x + (_speakerPanelWidth / 2) * -1);
            int positionX = initialPositionX + _panelPadding;
            int positionY = (int)(_speakerPanel.localPosition.y + (_dialogueConfigurationManager.TextHeight * -2) - _panelPadding);

            List<DialogueSpriteData> bufferedDialogueSpriteData = new List<DialogueSpriteData>();
            bufferedDialogueSpriteData = _dialogueParser.Parse(ref _speaker);

            for (int i = 0; i < _speaker.Length; i++)
            {
                DialogueSpriteData spriteData = new DialogueSpriteData();

                if (bufferedDialogueSpriteData.Count > 0)
                {
                    if (i == bufferedDialogueSpriteData[0].Index)
                    {
                        _currentFontColor = bufferedDialogueSpriteData[0].Color;
                        bufferedDialogueSpriteData.RemoveAt(0);
                    }
                }

                // Check if the sprite for the character is in the dictionary
                if (_dialogueConfigurationManager.AlphabetDictionary.TryGetValue(_speaker[i], out textSpriteData) == false)
                {
                    positionX += _wordSpacing;
                    continue;
                }

                spriteData.PositionX = positionX;
                spriteData.PositionY = positionY;
                spriteData.TextSpriteData = textSpriteData;
                spriteData.Value = _speaker[i];
                spriteData.Name = "Speaker";
                spriteData.Color = _currentFontColor;
                spriteData.Parent = _dialogueBox.transform;

                _dialogueRenderer.RenderCharacter(spriteData);
                positionX += spriteData.TextSpriteData.Width + _letterSpacing;
            }
        }

        private void RenderChoices()
        {

        }

        private void ResolveSentence()
        {
            _currentFontColor = _defaultFontColor;

            // Exit if the conversation is over
            if (_conversation.Count == 0)
            {
                if ((_currentDialogueEntry as IDialogueEntrySimple).NextDialogueEntry == null)
                {
                    EndDialogue();
                    ClearDialogueBox();
                    return;
                }
                else
                {
                    _currentDialogueEntry = (_currentDialogueEntry as IDialogueEntrySimple).NextDialogueEntry.GetComponent<IDialogueEntry>();
                }

                ResolveDialogueEntry(_currentDialogueEntry);
                return;
            }

            ClearDialogueBox();

            // Dequeue the next sentence and display it
            string sentence = _conversation.Dequeue();
            TextSpriteData textSpriteData = new TextSpriteData(0, 0, 0, 0, null);
            int initialPositionX = (int)(_dialoguePanelWidth / 2) * -1;
            int positionX = initialPositionX + _panelPadding;
            int positionY = (int)(_dialoguePanel.localPosition.y + (_dialogueConfigurationManager.TextHeight * -2) - _panelPadding);

            List<DialogueSpriteData> bufferedDialogueSpriteData = new List<DialogueSpriteData>();
            bufferedDialogueSpriteData = _dialogueParser.Parse(ref sentence);

            for (int i = 0; i < sentence.Length; i++)
            {
                DialogueSpriteData spriteData = new DialogueSpriteData();
                if (bufferedDialogueSpriteData.Count > 0)
                {
                    if (i == bufferedDialogueSpriteData[0].Index)
                    {
                        _currentFontColor = bufferedDialogueSpriteData[0].Color;
                        bufferedDialogueSpriteData.RemoveAt(0);
                    }
                }

                // Set positioning for new line
                if (sentence[i] == '\n')
                {
                    positionY -= _dialogueConfigurationManager.TextHeight + _lineSpacing;
                    positionX = initialPositionX;
                    continue;
                }

                // Check if the sprite for the character is in the dictionary
                if (_dialogueConfigurationManager.AlphabetDictionary.TryGetValue(sentence[i], out textSpriteData) == false)
                {
                    positionX += _wordSpacing;
                    continue;
                }

                spriteData.PositionX = positionX;
                spriteData.PositionY = positionY;
                spriteData.TextSpriteData = textSpriteData;
                spriteData.Value = sentence[i];
                spriteData.Name = string.Empty;
                spriteData.Color = _currentFontColor;
                spriteData.Parent = _dialogueBox.transform;

                _dialogueRenderer.RenderCharacter(spriteData);
                positionX += spriteData.TextSpriteData.Width + _letterSpacing;
            }
        }

        private void ClearDialogueBox()
        {
            // Clear previous dialogue
            foreach (Transform child in _dialogueBox)
            {
                if (child == _dialoguePanel || child == _speakerPanel || child.name == "Speaker" || child.name == "Portrait")
                {
                    continue;
                }

                Destroy(child.gameObject);
            }
        }

        private void ClearSpeakerBox()
        {
            // Clear previous dialogue
            foreach (Transform child in _dialogueBox)
            {
                if (child.name == "Speaker")
                {
                    Destroy(child.gameObject);
                }
            }
        }

        private void EndDialogue()
        {
            // Play transition animation to hide dialogue box
            _dialogueAnimator.SetTrigger("Transition");
            _isDialogueActive = false;
        }

        private void ResolveDialogueEntry(IDialogueEntry dialogueEntry)
        {
            if (dialogueEntry is IDialogueEntryBranching)
            {
                ResolveDialogueEntry((dialogueEntry as IDialogueEntryBranching));
            }
            else
            {
                ResolveDialogueEntry((dialogueEntry as IDialogueEntrySimple));
            }
        }

        private void ResolveDialogueEntry(IDialogueEntrySimple dialogueEntry)
        {
            foreach (string sentence in dialogueEntry.Sentences)
            {
                _conversation.Enqueue(sentence);
            }

            // Write Name
            _speaker = _currentDialogueEntry.Speaker;
            _portrait.sprite = _currentDialogueEntry.Portrait;

            ResolveName();
            ResolveSentence();
        }

        private void ResolveDialogueEntry(IDialogueEntryBranching dialogueEntry)
        {
            _conversation.Enqueue(dialogueEntry.Sentence);

            // Write Name
            _speaker = _currentDialogueEntry.Speaker;
            _portrait.sprite = _currentDialogueEntry.Portrait;

            ResolveName();
            RenderChoices();
            ResolveSentence();
        }

        #endregion


        //**************************************************\\
        //****************** Properties ********************\\
        //**************************************************\\


    }
}
