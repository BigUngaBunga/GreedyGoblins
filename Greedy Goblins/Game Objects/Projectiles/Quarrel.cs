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
    class Quarrel : Projectile
    {
        public Quarrel(Vector2 position, Vector2 direction, int damage) : base(AssetManager.textures["quarrel"], position, direction, new Vector2(0.8f), 25, damage)
        {
            rotation -= MathHelper.ToRadians(45);
        }
    }
}
