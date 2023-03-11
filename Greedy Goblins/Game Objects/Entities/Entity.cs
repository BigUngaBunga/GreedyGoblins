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
    enum EntityType
    {
        Goblin, Miner, Fighter, Soldier, King
    }

    abstract class Entity : GameObject
    {
        public Rectangle sourceRectangle;
        protected Point imageSize;
        protected int numberOfImages, currentImage;
        protected float speed;
        protected float animationTimer, animationSpeed;
        public int PathNumber { get; protected set; }
        public int Value { get; protected set; }
        public float pathPosition;

        public Entity(Texture2D texture, Vector2 scale, float speed, int value, int numberOfImages, int pathNumber)
        {   
            PathNumber = pathNumber;
            Texture = texture;
            Value = value;
            this.speed = 1 / (speed * 60);//speed = sekunder som det tar att nå banans slut
            this.numberOfImages = numberOfImages;
            this.scale = scale;
            pathPosition = 0;

            imageSize = new Point(Texture.Width / numberOfImages, Texture.Height);
            sourceRectangle = new Rectangle(new Point(imageSize.X * currentImage, 0), imageSize);
            animationSpeed = 0.175f/this.speed;
            pointOfOrigin = sourceRectangle.Size.ToVector2() / 2f;
            Radius = (Texture.Width < Texture.Height) ? imageSize.Y / 2 * scale.Y: imageSize.X / 2 * scale.X;

            hitbox = new Rectangle(position.ToPoint(), new Point((int)(imageSize.X * scale.X), (int)(imageSize.Y * scale.Y)));
        }


        public virtual void Update(GameTime gameTime)
        {
            Animate(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(AssetManager.textures["pixel"], hitbox, Color.Blue);//HITBOX DEBUG
            spriteBatch.Draw(Texture, position, sourceRectangle, Color.White, rotation, pointOfOrigin, scale, SpriteEffects.None, 0f);
        }


        //Loopar igenom animationen
        protected virtual void Animate(GameTime gameTime)
        {
            animationTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (animationTimer >= animationSpeed)
            {
                animationTimer -= animationSpeed;
                currentImage++;
                currentImage %= numberOfImages;
            }

            sourceRectangle.X = imageSize.X * currentImage;

        }


        //Flyttar till position
        public virtual void UpdatePosition(Vector2 position, float rotation)
        {
            this.rotation = rotation + MathHelper.ToRadians(180);
            this.position = position;
            hitbox.Location = (position - pointOfOrigin * scale).ToPoint();
        }


        //Flyttar längs banan och returnerar position
        public void Move()
        {
            pathPosition += speed;
        }


        //Jämför om pixlar överlappar varandra
        public bool PixelPerffectCollision(GameObject otherObject)
        {
            Color[] thisTextureData = new Color[sourceRectangle.Width * sourceRectangle.Height];//Sparar alla pixlar i första föremåls source rektangel
            Texture.GetData(0, sourceRectangle, thisTextureData, 0, thisTextureData.Length);

            Color[] otherTextureData = new Color[otherObject.Texture.Width * otherObject.Texture.Height];//Sparar alla pixlar i andra föremål 
            otherObject.Texture.GetData(otherTextureData);

            //Ramar in var kollisionen kan ske
            int top = Math.Max(Hitbox.Top, otherObject.Hitbox.Top);
            int bottom = Math.Min(Hitbox.Bottom, otherObject.Hitbox.Bottom);
            int left = Math.Max(Hitbox.Left, otherObject.Hitbox.Left);
            int right = Math.Min(Hitbox.Right, otherObject.Hitbox.Right);

            //Om två pixlar har färg och är på samma plats så betyder det att de två bilderna kolliderar

            try
            {
                for (int y = top; y < bottom; y++)
                    for (int x = left; x < right; x++)
                    {
                        Color colorA = thisTextureData[(x - Hitbox.Left) + (y - Hitbox.Top) * Hitbox.Width];
                        Color colorB = otherTextureData[(x - otherObject.Hitbox.Left) + (y - otherObject.Hitbox.Top) * otherObject.Hitbox.Width];

                        if (colorA.A != 0 && colorB.A != 0)
                            return true;
                    }
            }
            catch (Exception)
            {

                return false;
            }
            

            return false;
        }
    }
}
