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
    class BloodEffect : Effect
    {
        public int ParticlesLeft { get { return particles.Count; } }
        int particlesToSpawn;
        List<Particle> particles;

        public BloodEffect(Vector2 pointOfOrigin, Vector2 originalDirection, int damage): base(pointOfOrigin, AssetManager.textures["pixel"])
        {
            particles = new List<Particle>();
            particlesToSpawn = 20 + damage * 2;
            CreateParticles(originalDirection);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update(gameTime);
                if (particles[i].Lifespan <= 0)
                    particles.RemoveAt(i);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in particles)
            {
                spriteBatch.Draw(texture, particle.Position, null, Color.Red, particle.Rotation, texture.Bounds.Size.ToVector2() / 2, particle.Scale, SpriteEffects.None, 0f);
            }
        }


        //Skapar nya partiklar
        public void CreateParticles(Vector2 direction)
        {
            for (int i = 0; i < particlesToSpawn; i++)
            {
                int lifespan = Main.random.Next(300, 1000);
                Vector2 velocity = new Vector2(direction.X * Main.random.Next(-100, 100) * 0.1f, direction.Y * Main.random.Next(-100, 100) * 0.1f);
                float rotation = MathHelper.ToDegrees(Main.random.Next(0, 360));
                float scale = Main.random.Next(20, 40) / 10f;

                particles.Add(new Particle(position, velocity, rotation, scale, lifespan));
            }
        }
    }
}
