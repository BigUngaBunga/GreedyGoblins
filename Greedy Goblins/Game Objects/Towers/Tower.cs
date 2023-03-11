using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Greedy_Goblins
{
    enum TowerType
    {
        Crossbow, Cannon, FlameThrower
    }

    abstract class Tower : GameObject
    {
        public bool isPlaced, highlight;
        public bool isSelected;
        public float Range { get; protected set; }
        public float Size { get; protected set; }
        public int Cooldown { get; protected set; }
        public int Cost { get;}
        public int UpgradeCost { get { return (int)(Cost *0.4f * Math.Pow(priceIncrease, towerLevel)); } }
        public Texture2D CircleTexture { get; }
        public int[] Stats { get { return new int[] {towerLevel, damage, (int)Range, fireRate }; } }
        public int[] UpgradedStats { get { return new int[] { towerLevel + 1, damage + damageIncrease, (int)Range + rangeIncrease, fireRate - fireRateDecrease }; } }
        protected int towerLevel, fireRate, damage;
        protected int damageIncrease, fireRateDecrease, rangeIncrease;
        protected float priceIncrease;
        


        //Används för att rita ut storlek och räkvidd
        protected float sizeScale, rangeScale;
        protected Color sizeColour, rangeColour, hiddenColour;


        public Tower(Texture2D texture, Vector2 scale, float range, float size, int damage, int fireRate, int cost)
        {
            Texture = texture;
            Range = range;
            Size = size;
            Cost = cost;
            this.scale = scale;
            this.damage = damage;
            this.fireRate = fireRate;
            position = KeyMouseReader.MousePosition();//Börjar vid musen
            towerLevel = 1;
            highlight = isPlaced = false;
            CircleTexture = AssetManager.textures["borderless circle"];
            Radius = size;
            priceIncrease = 1.2f;

            //Rotation
            pointOfOrigin = Texture.Bounds.Size.ToVector2() / 2;
            rotation = 0;

            //Storlek och räckvidd
            UpdateRangeAndSize();
            sizeColour = new Color(50, 220, 50, 50);
            hiddenColour = new Color(1, 1, 1, 1);
            rangeColour = new Color(0, 0, 0, 100);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (isPlaced)
            {
                if (Cooldown > 0)
                    Cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                position = KeyMouseReader.MousePosition();
                UpdateRangeAndSize();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            void DrawRange()
            {
                spriteBatch.Draw(CircleTexture, position, null, rangeColour, rotation, CircleTexture.Bounds.Size.ToVector2() / 2, rangeScale, SpriteEffects.None, 0f);
            }
            void Highlight()
            {
                spriteBatch.Draw(CircleTexture, position, null, Color.White, rotation, CircleTexture.Bounds.Size.ToVector2() / 2, sizeScale, SpriteEffects.None, 0f);
            }

            //Ej placerat
            if (!isPlaced)
            {
                DrawRange();
                DrawSize(spriteBatch, false);
            }

            //Torn har blivit valt
            else if (isSelected)
            {
                DrawRange();
                Highlight();
            }
            
            //Muspekare på torn
            else if (IsInFocus())
                Highlight();
                
            spriteBatch.Draw(Texture, position, null, Color.White, rotation, pointOfOrigin, scale, SpriteEffects.None, 0f);
        }

        //Ritar tornets storlek
        public virtual void DrawSize(SpriteBatch spriteBatch, bool hidden)
        {
            if (hidden)
                spriteBatch.Draw(CircleTexture, position, null, hiddenColour, rotation, CircleTexture.Bounds.Size.ToVector2() / 2, sizeScale, SpriteEffects.None, 0f);
            else
                spriteBatch.Draw(CircleTexture, position, null, sizeColour, rotation, CircleTexture.Bounds.Size.ToVector2() / 2, sizeScale, SpriteEffects.None, 0f);

        }


        //Uppdaterar räckvidd och storlek
        protected void UpdateRangeAndSize()
        {
            rangeScale = Range / CircleTexture.Width;
            if (!isPlaced)
            {
                sizeScale = Size / CircleTexture.Width;
                hitbox = new Rectangle(position.ToPoint() - new Point((int)Size / 2), new Point((int)Size));
            }
        }


        //Ändrar färg beroende på om placering av torn är möjligt eller ej
        public void UpdateSizeColour(bool canBePlaced)
        {
            if (canBePlaced)
                sizeColour = new Color(50, 220, 50, 100);
            else
                sizeColour = new Color(220, 50, 50, 100);
        }


        //Roterar tornet mot punkt
        public virtual void RotateTowardsTarget(Vector2 position)
        {
            Vector2 distance = position - this.position;
            rotation = (float)Math.Atan2(distance.Y, distance.X);
        }


        //Attackerar
        public abstract Projectile Fire(Vector2 position);


        //Förbereder för attack
        protected virtual Vector2 PrepareAttack(Vector2 position)
        {
            StartCooldown();
            Vector2 direction = position - this.position;
            direction.Normalize();
            return direction;
        }


        //Sätter på nedkylning av eldgivning
        protected void StartCooldown()
        {
            Cooldown = fireRate;
        }


        //Ökar level, skada, räckvidd och eldhastighet
        public void UpgradeTower()
        {
            towerLevel++;
            damage += damageIncrease;
            fireRate -= fireRateDecrease;
            Range += rangeIncrease;
            UpdateRangeAndSize();
        }


        //Kollar efter cirkelkollision mer räckvidden
        public bool IsInRange(GameObject gameObject)
        {
            Vector2 distance = gameObject.Position - position;
            return distance.Length() <= Range / 2 + gameObject.Radius;
        }


        //Returnerarn om musen är över tornet
        public bool IsInFocus()
        {
            return CircleCollision(KeyMouseReader.MousePosition());
        }
    }
}
