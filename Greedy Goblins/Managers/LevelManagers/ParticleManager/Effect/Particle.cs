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
    class Particle
    {
        public int Lifespan { get; private set; }
        public Vector2 Position { get; private set; }
        public float Scale { get; private set; }
        public float Rotation { get;}
        Vector2 velocity;
        


        public Particle(Vector2 position, Vector2 velocity, float rotation, float scale, int lifespan)
        {
            Position = position;
            Lifespan = lifespan;
            Scale = scale;
            Rotation = rotation;
            this.velocity = velocity;
            
        }

        public void Update(GameTime gameTime)
        {
            Position += velocity;
            Lifespan -= gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
