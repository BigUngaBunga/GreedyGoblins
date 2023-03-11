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
    class Cannonball : Projectile
    {
        public bool Exploded { get; private set; }
        public float ExplosionSize { get; private set; }

        public Cannonball(Vector2 position, Vector2 direction, float explosionSize, int damage) : base(AssetManager.textures["cannonball"], position, direction, new Vector2(0.4f), 25, damage)
        {
            ExplosionSize = explosionSize;
        }

        public void Explode()
        {
            Radius = ExplosionSize;
            Exploded = true;
            if (ButtonManager.soundIsOn)
                AssetManager.soundEffects["cannonball explosion"].Play(0.6f, 0, 0);
        }
    }
}
