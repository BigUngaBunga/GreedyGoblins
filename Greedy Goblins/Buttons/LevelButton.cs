using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Greedy_Goblins
{
    enum LevelType
    {
        Forest, Shrubland, Mountain, Snow
    }

    class LevelButton: TextButton
    {
        public LevelType LevelType { get; }

        public LevelButton(Rectangle originalHitbox, Texture2D buttonTexture, LevelType levelType) : base(originalHitbox, buttonTexture)
        {
            LevelType = levelType;
            font = AssetManager.fonts["button"];

            RescaleHitbox();
            SetLevelType();
        }
        
        //Laddar in färg och text till knapp
        private void SetLevelType()
        {
            int[] colourValues;
            int increase = 30;
            switch (LevelType)
            {
                case LevelType.Forest:
                    colourValues = new int[] { 10, 120, 2};//Grön
                    break;
                case LevelType.Shrubland:
                    colourValues = new int[] { 70, 200, 60 };//Ljusgrön
                    break;
                case LevelType.Mountain:
                    colourValues = new int[] { 110, 115, 100 };//Grå
                    break;
                case LevelType.Snow:
                    colourValues = new int[] { 200, 200, 225 };//Vit
                    break;
                default:
                    colourValues = new int[] { 13, 30, 7 };//Svart
                    break;
            }
            //Färg
            neutralColour = new Color(colourValues[0], colourValues[1], colourValues[2]);

            for (int i = 0; i < colourValues.Length; i++)//Gör färg ljusare
                colourValues[i] += increase;
            focusedColour = new Color(colourValues[0], colourValues[1], colourValues[2]);
            colour = neutralColour;
            //Text
            buttonText = LevelType.ToString();
            SetTextPosition();
        }
    }
}
