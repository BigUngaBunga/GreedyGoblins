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
    abstract class GameObject
    {
        public Texture2D Texture { get; protected set; }
        public Vector2 Position { get { return position; } }
        public float Radius { get; protected set; }
        public Rectangle Hitbox { get { return hitbox; } }
        protected Rectangle hitbox;
        protected Vector2 position, scale, pointOfOrigin;
        protected float rotation;
        

        public abstract void Draw(SpriteBatch spriteBatch);

        //Uppdaterar föremålsskalan med skärmskalan
        protected void SetScale()
        {
            scale *= Main.ScreenScale;
        }


        //Kollision med cirkel, eller om punkt är i cirkel
        public bool CircleCollision(GameObject gameObject)
        {
            Vector2 distance = position - gameObject.Position;
            return distance.Length() <= Radius + gameObject.Radius;
        }
        //Kollar om punkt är innanför cirkelns radie
        public bool CircleCollision(Vector2 point)
        {
            Vector2 distance = position - point;
            return distance.Length() <= Radius;
        }
    }
}
