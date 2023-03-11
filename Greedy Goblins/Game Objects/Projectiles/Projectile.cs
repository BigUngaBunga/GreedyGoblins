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

    abstract class Projectile : GameObject
    {
        protected Vector2 direction;
        protected float speed;
        protected Color colour;
        public Vector2 Direction { get { return direction; } }
        public int Damage { get; protected set; }
        public float Rotation { get { return rotation; } }
        public bool hurtEnemies;

        public Projectile(Texture2D texture, Vector2 position, Vector2 direction, Vector2 scale, float speed, int damage)
        {
            Texture = texture;
            this.position = position;
            this.direction = direction;
            this.speed = speed;
            this.scale = scale;
            Damage = damage;
            pointOfOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            Radius = (Texture.Width < Texture.Height) ? Texture.Height / 2 * scale.Y : Texture.Width / 2 * scale.X;
            hurtEnemies = false;
            colour = Color.White;

            SetScale();
            hitbox = new Rectangle(position.ToPoint(), new Vector2(Texture.Width * scale.X, Texture.Height * scale.Y).ToPoint());
        }


        public virtual void Update(GameTime gameTime)
        {
            position += direction * speed;
            hitbox.Location = position.ToPoint();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position, null, colour, rotation, pointOfOrigin, scale, SpriteEffects.None, 0f);
        }


        //Pixelperfekt kollision med fiende
        public virtual bool HitATarget(Enemy target)
        {
            if (CircleCollision(target))
            {
                Color[] projectilePixels = new Color[Texture.Width * Texture.Height];//Sparar alla pixlar i första föremål
                Texture.GetData(projectilePixels);

                Color[] targetPixels = new Color[target.Texture.Width * target.Texture.Height];//Sparar alla pixlar i andra föremål 
                target.Texture.GetData(targetPixels);//0, Hitbox, targetPixels, 0, targetPixels.Length

                //Ramar in var kollisionen kan ske
                int top = Math.Max(hitbox.Top, target.Hitbox.Top);
                int bottom = Math.Min(hitbox.Bottom, target.Hitbox.Bottom);
                int left = Math.Max(hitbox.Left, target.Hitbox.Left);
                int right = Math.Min(hitbox.Right, target.Hitbox.Right);

                //Om två pixlar har färg och är på samma plats så betyder det att de två bilderna kolliderar
                for (int y = top; y < bottom; y++)
                    for (int x = left; x < right; x++)
                    {
                        Color projectilePixel = projectilePixels[(x - hitbox.Left) + (y - hitbox.Top) * hitbox.Width];
                        Color targetPixel = targetPixels[(x - target.Hitbox.Left) + (y - target.Hitbox.Top) * target.Hitbox.Width];

                        if (projectilePixel.A != 0 && targetPixel.A != 0)
                            return true;
                    }
            }

            return false;
        }
    }
}
