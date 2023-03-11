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
    abstract class Effect
    {
        protected Vector2 position;
        protected Texture2D texture;

        public Effect(Vector2 position, Texture2D texture)
        {
            this.texture = texture;
            this.position = position;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
