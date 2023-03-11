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
    class Explosion : Effect
    {
        public bool finishedExplosion;
        Vector2 scale;
        Rectangle sourceRectangle;
        Point imageSize, numberOfImages, currentImage;
        float rotation;

        public Explosion(Vector2 position, float explosionSize, float rotation) : base(position, AssetManager.textures["explosion spritesheet"])
        {
            
            this.rotation = rotation;
            currentImage = Point.Zero;
            numberOfImages = new Point(4, 3);
            imageSize = texture.Bounds.Size / numberOfImages;
            sourceRectangle = new Rectangle(currentImage, imageSize);
            scale = new Vector2(explosionSize / sourceRectangle.Height);
            finishedExplosion = false;
        }

        public override void Update(GameTime gameTime)
        {
            currentImage.X++;
            if (currentImage.X >= numberOfImages.X)
            {
                currentImage.X = 0;
                currentImage.Y++;
                if (currentImage.Y >= numberOfImages.Y)
                    finishedExplosion = true;
            }
            sourceRectangle.Location = currentImage * imageSize;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White, rotation, sourceRectangle.Size.ToVector2() / 2, scale, SpriteEffects.None, 0f);
        }
    }
}
