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
    class Flames : Projectile
    {
        public float Duration { get; private set; }
        public Vector2 NozzlePosition { get; }
        float range;

        public Flames(Vector2 position, Vector2 direction, Vector2 flameSize, float range, int damage) : 
                    base(AssetManager.textures["borderless circle"], position, direction, flameSize, 0, damage)
        {
            scale = flameSize;
            NozzlePosition = position + direction * range / 5;
            this.range = range;
            this.position += direction * range / 2;
            colour = Color.Transparent;
            Duration = 100;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Duration -= gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
