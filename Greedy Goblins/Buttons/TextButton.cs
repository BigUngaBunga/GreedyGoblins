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
    abstract class TextButton : Button
    {
        protected SpriteFont font;
        protected Vector2 textPosition;
        protected string buttonText;

        public TextButton(Rectangle originalHitbox, Texture2D buttonTexture) : base(originalHitbox, buttonTexture)
        {
            font = AssetManager.fonts["button"];
        }

        //Sätter text positionen
        protected void SetTextPosition()
        {
            int textSize = 17, textHeight = 10;
            textPosition = new Vector2(originalHitbox.X + originalHitbox.Width / 2 - buttonText.Length * textSize, originalHitbox.Y + textHeight);
        }


        //Ritar knapp och knapptext
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(font, buttonText, textPosition, Color.Black);
        }
    }
}
