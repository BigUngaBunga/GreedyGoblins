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

    class MapObject : GameObject
    {
        public MapObject(Vector2 position, Texture2D texture, float scale, float rotation)
        {
            this.position = position;
            this.rotation = rotation ;
            this.scale = new Vector2(scale);
            Texture = texture;
            SetScale();

            pointOfOrigin = Texture.Bounds.Size.ToVector2() / 2;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(AssetManager.textures["button"], Hitbox, Color.Wheat);
            spriteBatch.Draw(Texture, position, null, Color.White, rotation, pointOfOrigin, scale, SpriteEffects.None, 0f);
        }
    }
}
