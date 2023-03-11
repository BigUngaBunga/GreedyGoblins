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
    class Goblin : Ally
    {
        Texture2D speechBubbleTexture;
        Vector2 speechBubblePosition, speechBubblePointOfOrigin, speechPosition;

        public Goblin(Vector2 scale, float speed, int value, int pathNumber) : base(AssetManager.textures["goblin"], scale, speed, value, 4, pathNumber)
        {
            speechBubbleTexture = AssetManager.textures["speech"];
            speechBubblePointOfOrigin = speechBubbleTexture.Bounds.Size.ToVector2() / 2;
        }


        public void DrawSpeechBubble(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(speechBubbleTexture, speechBubblePosition, null, Color.White, 0f, speechBubblePointOfOrigin, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(AssetManager.fonts["information"], "The Dwarves\nAre Coming!", speechPosition, Color.Black, 0f, speechBubblePointOfOrigin, scale, SpriteEffects.None, 0f);
        }

        public override void UpdatePosition(Vector2 position, float rotation)
        {
            base.UpdatePosition(position, rotation);
            speechBubblePosition = position - speechBubbleTexture.Bounds.Size.ToVector2() / 2 * scale;
            speechPosition = speechBubblePosition + new Vector2(10, 30);
        }
    }
}
