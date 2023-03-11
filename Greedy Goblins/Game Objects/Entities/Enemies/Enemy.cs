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

    abstract class Enemy : Entity
    {
        public int Damage { get; protected set; }
        public int Health { get; protected set; }
        protected int initialHealth; 
        protected Rectangle healthBar;
        protected Point healthBarSize;

        public Enemy(Texture2D texture, Vector2 scale, float speed, int value, int numberOfImages, int damage, int health, int pathNumber) : base(texture, scale, speed, value, numberOfImages, pathNumber)
        {
            Damage = damage;
            Health = initialHealth = health;

            healthBarSize = new Point(50, 10);
            healthBar = new Rectangle(Point.Zero, healthBarSize);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateHealthBar();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(AssetManager.textures["pixel"], healthBar, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        //Sänker liv på fiende
        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
        }

        protected virtual void UpdateHealthBar()
        {
            healthBar.Width = healthBarSize.X * Health / initialHealth;
            healthBar.Location = (position - new Vector2(sourceRectangle.Width / 2 * scale.X, sourceRectangle.Height / 2 * scale.Y)).ToPoint();
        }
    }
}
