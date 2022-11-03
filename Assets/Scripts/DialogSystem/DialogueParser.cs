using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class DialogueParser : MonoBehaviour, IDialogueParser
    {

        public List<DialogueSpriteData> Parse(ref string dialogue)
        {
            List<DialogueSpriteData> bufferedDialogueSpriteData = new List<DialogueSpriteData>();

            while (dialogue.Contains("<color:"))
            {
                DialogueSpriteData buffer = new DialogueSpriteData();
                buffer.Index = dialogue.IndexOf("<color:");

                string color = dialogue.Substring(buffer.Index, dialogue.IndexOf(">") - buffer.Index + 1);
                int optionLength = 0;

                Debug.Log(color);

                switch (color)
                {
                    case "<color:Red>":
                        buffer.Color = Color.red;
                        optionLength = 3;
                        break;
                    case "<color:Blue>":
                        buffer.Color = Color.blue;
                        optionLength = 4;
                        break;
                    case "<color:Green>":
                        buffer.Color = Color.green;
                        optionLength = 5;
                        break;
                    case "<color:Yellow>":
                        buffer.Color = Color.yellow;
                        optionLength = 6;
                        break;
                    case "<color:Black>":
                        buffer.Color = Color.black;
                        optionLength = 5;
                        break;
                    case "<color:White>":
                        buffer.Color = Color.white;
                        optionLength = 5;
                        break;
                }
                bufferedDialogueSpriteData.Add(buffer);
                dialogue = dialogue.Remove(buffer.Index, 8 + optionLength);
            }

            Debug.Log(dialogue);
            return bufferedDialogueSpriteData;
        }
    }
}
