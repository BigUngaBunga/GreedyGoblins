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
    class FlameEffect : Effect
    {
        public int ParticlesLeft { get { return particles.Count; } }
        int particlesToSpawn;
        float rotation;
        List<Particle> particles;
        List<Point> particleImage;
        Point imageSize, numberOfImages;
        Vector2 rotationOrigin;

        public FlameEffect(Vector2 pointOfOrigin, Vector2 originalDirection, float rotation) : base(pointOfOrigin, AssetManager.textures["fire spritesheet"])
        {
            this.rotation = rotation + MathHelper.ToDegrees(90);
            
            particles = new List<Particle>();
            particleImage = new List<Point>();
            particlesToSpawn = 13;
            numberOfImages = new Point(4, 3);
            imageSize = texture.Bounds.Size / numberOfImages;
            rotationOrigin = imageSize.ToVector2() / 2;
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
            //10% sanolikhet att byta bild
            if (Main.random.NextDouble() <= 0.1)
            {
                for (int i = 0; i < particleImage.Count; i++)
                {
                    Point point = particleImage[i];
                    point.X++;
                    if (point.X >= numberOfImages.X)
                        point.Y++;

                    point.X %= numberOfImages.X;
                    point.Y %= numberOfImages.Y;
                    particleImage[i] = point;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                spriteBatch.Draw(texture, particles[i].Position, new Rectangle(particleImage[i] * imageSize, imageSize),
                                Color.White, particles[i].Rotation, rotationOrigin, particles[i].Scale, SpriteEffects.None, 0f);
            }
        }


        //Skapar nya partiklar
        public void CreateParticles(Vector2 direction)
        {
            for (int i = 0; i < particlesToSpawn; i++)
            {
                int lifespan = Main.random.Next(500, 1000);//Slumpar livslängd
                Vector2 velocity = new Vector2(direction.X * Main.random.Next(40, 200) * 0.02f, direction.Y * Main.random.Next(40, 200) * 0.02f) * Main.random.Next(10, 30) / 10f;//Slumpar hastighet
                float scale = Main.random.Next(10, 20) / 69f;//Slumpar skalan

                particles.Add(new Particle(position, velocity, rotation, scale, lifespan));
                particleImage.Add(new Point(Main.random.Next(0, numberOfImages.X), Main.random.Next(0, numberOfImages.Y)));
            }
        }
    }
}
