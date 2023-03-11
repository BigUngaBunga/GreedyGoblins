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
    class Cursor : GameObject
    {
        public Cursor()
        {
            Texture = AssetManager.textures["knife"];
            scale = Vector2.One;
            pointOfOrigin = new Vector2(Texture.Width / 2);
            hitbox = new Rectangle(position.ToPoint(), Texture.Bounds.Size);
            Radius = (float)Math.Sqrt(Math.Pow(Texture.Width * scale.X, 2) + Math.Pow(Texture.Height * scale.Y, 2)) / 2;
        }


        public void Update()
        {
            position = KeyMouseReader.MousePosition();
            hitbox.Location = position.ToPoint();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(AssetManager.textures["pixel"], hitbox, Color.Green);//HITBOX DEBUG
            spriteBatch.Draw(Texture, position, null, Color.White, 0f, pointOfOrigin, scale, SpriteEffects.None, 0f);
        }
    }
}
