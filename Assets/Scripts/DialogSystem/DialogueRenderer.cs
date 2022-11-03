using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrozenBanana
{

    public class DialogueRenderer : MonoBehaviour, IDialogueRenderer
    {

        public void RenderCharacter(DialogueSpriteData spriteData)
        {
            // Create and add the letter sprite to the dialogue panel
            string name = spriteData.Name.Length == 0 ? spriteData.Value.ToString() : spriteData.Name;
            GameObject imgObject = new GameObject(name);

            RectTransform trans = imgObject.AddComponent<RectTransform>();
            trans.transform.SetParent(spriteData.Parent);
            trans.localScale = Vector3.one;
            trans.pivot = new Vector2(0, 0);
            trans.anchoredPosition = new Vector2(spriteData.PositionX - spriteData.TextSpriteData.LeftPadding, spriteData.PositionY);

            Image image = imgObject.AddComponent<Image>();
            image.sprite = spriteData.TextSpriteData.CharSprite;
            image.color = spriteData.Color;
            imgObject.transform.SetParent(spriteData.Parent);
        }
    }
}
