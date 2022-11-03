using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace TheFrozenBanana
{
    /// <summary>
    /// Handles all the configuration details of the dialogue system including
    /// the initial calculation of values, saving, and recalling configuration
    /// data.
    /// </summary>
    public class DialogueConfigurationManager : MonoBehaviour, IDialogueConfigurationManager
    {
        [SerializeField] private bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        private string _configurationFileDirectory;
        [SerializeField] private string _configurationFile;
        [SerializeField] private char[] _charAlphabet = ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()_-+=~`{}[]|\\:;\"'.,<>?/").ToCharArray();
        [SerializeField] private Sprite[] _spriteAlphabet;

        private Dictionary<char, TextSpriteData> _alphabetDictionary;
        private int _textHeight = 0;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        #region PublicMethods


        // TODO Move to the IDialogueConfigurationManager
        /// <summary>
        /// Calculates the width, and height of each character.
        /// </summary>
        public void Initialize()
        {
            if (_showDebugLog)
            {
                Debug.Log("Starting DialogueManager Initialize");
            }

            try
            {
                for (int i = 0; i < _charAlphabet.Length; i++)
                {
                    // Get sprite width and height
                    int width = (int)_spriteAlphabet[i].rect.size.x;
                    int height = (int)_spriteAlphabet[i].rect.size.y;

                    // Initialize positions
                    int startingPosition = -1;
                    int endingPosition = -1;
                    int leftPadding = 0;
                    int rightPadding = 0;

                    // Find starting position for search
                    int startingIndexX = (int)_spriteAlphabet[i].rect.x;
                    int startingIndexY = (int)_spriteAlphabet[i].rect.yMax;

                    // Get the left most pixel of the sprite
                    for (int searchIndexX = startingIndexX; searchIndexX < startingIndexX + width; searchIndexX++)
                    {
                        for (int searchIndexY = startingIndexY; startingIndexY - searchIndexY < height; searchIndexY--)
                        {
                            if (_showDebugLog)
                            {
                                Debug.Log("Searching pixel at " + searchIndexX + ", " + searchIndexY + " for sprite start position.");
                            }

                            if (_spriteAlphabet[i].texture.GetPixel(searchIndexX, searchIndexY).a != 0)
                            {
                                startingPosition = searchIndexX;
                                leftPadding = searchIndexX - startingIndexX;
                                break;
                            }
                        }

                        if (startingPosition > -1)
                        {
                            if (_showDebugLog)
                            {
                                Debug.Log("Start Position assigned");
                            }

                            break;
                        }
                    }

                    // Get the right most pixel of the sprite
                    for (int searchIndexX = startingIndexX + width - 1; startingIndexX - searchIndexX < width; searchIndexX--)
                    {
                        for (int searchIndexY = startingIndexY; startingIndexY - searchIndexY < height; searchIndexY--)
                        {
                            if (_showDebugLog)
                            {
                                Debug.Log("Searching pixel at " + searchIndexX + ", " + searchIndexY + " for sprite end position.");
                            }

                            if (_spriteAlphabet[i].texture.GetPixel(searchIndexX, searchIndexY).a != 0)
                            {
                                endingPosition = searchIndexX;
                                rightPadding = width + startingIndexX - endingPosition;
                                break;
                            }
                        }

                        if (endingPosition > -1)
                        {
                            if (_showDebugLog)
                            {
                                Debug.Log("End Position assigned");
                            }

                            break;
                        }
                    }

                    // No non transparent pixel's could be found
                    if (startingPosition == -1 || endingPosition == -1)
                    {
                        Debug.LogError("An error occureed while extracting sprite data for \"" + _charAlphabet[i] + "\"!\n" +
                            "The sprite width could not be determined!");
                    }

                    if (_showDebugLog)
                    {
                        Debug.Log(_charAlphabet[i] + " Sprite Data: width=" + (endingPosition - startingPosition) + " height=" + (int)_spriteAlphabet[i].rect.height + " leftPadding=" + leftPadding + " rightPadding=" + rightPadding);
                    }

                    // Create TextSpriteData
                    TextSpriteData textSpriteData = new TextSpriteData(endingPosition - startingPosition, (int)_spriteAlphabet[i].rect.height, leftPadding, rightPadding, _spriteAlphabet[i]);

                    // Add to AlphabetDictionary
                    _alphabetDictionary.Add(_charAlphabet[i], textSpriteData);
                }

                // Text height is assumed to be uniform and taken from the first character in the dictionary
                _textHeight = _alphabetDictionary[_charAlphabet[0]].Height;
            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred in DialogueManager while running Initialize!\n" + ex.Message);
            }

            if (_showDebugLog)
            {
                for (int i = 0; i < _charAlphabet.Length; i++)
                {
                    Debug.Log(_charAlphabet[i] + " has a width of " + _alphabetDictionary[_charAlphabet[i]].Width + " and a height of " + _alphabetDictionary[_charAlphabet[i]].Height);
                }

                Debug.Log("Finishing DialogueManager Initialize");
                Debug.Log("Saving Sprite Data");
            }

            Save();
        }

        // TODO Move to the IDialogueConfigurationManager
        /// <summary>
        /// Loads TextSpriteData from the Config file.
        /// </summary>
        /// <returns>Returns false if the load operation fails.</returns>
        public bool Load()
        {
            // get reference to file
            if (File.Exists(Path.Combine(_configurationFileDirectory, _configurationFile)) == false)
            {
                return false;
            }

            try
            {
                // open and read
                string[] buffer = File.ReadAllText(Path.Combine(_configurationFileDirectory, _configurationFile))
                    .Replace("\r\n", "&split")
                    .Split(new string[] { "&split" }, StringSplitOptions.None);

                // loop through lines and initialize sprite data, add to alphabet array
                for (int i = 0; i < buffer.Length / 5; i++)
                {
                    int pointer = i * 5;

                    TextSpriteData data = new TextSpriteData
                    (
                        Convert.ToInt32(buffer[pointer + 1]),
                        Convert.ToInt32(buffer[pointer + 2]),
                        Convert.ToInt32(buffer[pointer + 3]),
                        Convert.ToInt32(buffer[pointer + 4]),
                        _spriteAlphabet[i]
                    );

                    _alphabetDictionary.Add(buffer[pointer][0], data);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred while executing DialogManager.Load().\n" + ex.Message);
                return false;
            }

            // Text height is assumed to be uniform and taken from the first character in the dictionary
            _textHeight = _alphabetDictionary[_charAlphabet[0]].Height;

            return true;
        }

        // TODO Move to the IDialogueConfigurationManager
        /// <summary>
        /// Saves TextSpriteData to the Config file.
        /// </summary>
        public void Save()
        {
            StringBuilder builder = new StringBuilder();

            foreach (KeyValuePair<char, TextSpriteData> data in _alphabetDictionary)
            {
                builder.Append(data.Key.ToString() + "&split" + data.Value.ToString().Replace(",", "&split") + Environment.NewLine);
            }

            File.WriteAllText(Path.Combine(_configurationFileDirectory, _configurationFile), builder.ToString());
        }

        #endregion


        #region UnityMethods

        void Awake()
        {
            _configurationFileDirectory = Path.Combine(Application.dataPath, "Config");
        }

        private void Start()
        {
            _alphabetDictionary = new Dictionary<char, TextSpriteData>();

            if (Load() == false)
            {
                Initialize();
            }
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

        public int TextHeight
        {
            get
            {
                return _textHeight;
            }
        }

        public Dictionary<char, TextSpriteData> AlphabetDictionary
        {
            get
            {
                return _alphabetDictionary;
            }
        }

    }
}
