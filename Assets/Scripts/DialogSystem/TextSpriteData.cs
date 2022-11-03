using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class TextSpriteData
    {
        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        public Sprite CharSprite;
        public int LeftPadding;
        public int RightPadding;
        public int Width;
        public int Height;

        public TextSpriteData(int width, int height, int leftPadding, int rightPadding, Sprite charSprite)
        {
            Width = width;
            Height = height;
            LeftPadding = leftPadding;
            RightPadding = rightPadding;
            CharSprite = charSprite;
        }

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        public override string ToString()
        {
            return Width + "," + Height + "," + LeftPadding + "," + RightPadding;
        }
    }
}
