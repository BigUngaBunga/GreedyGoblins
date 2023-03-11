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
    class Flamethrower : Tower
    {
        Vector2 flameSize;
        float range;

        public Flamethrower(Vector2 scale, float range, float size, int damage, int fireRate, int cost) : 
                            base(AssetManager.textures["flamethrower"], scale, range, size, damage, fireRate, cost)
        {
            this.range = range;
            flameSize = new Vector2(range / 1000, range / 1750);

            damageIncrease = 1;
            fireRateDecrease = 0;
            rangeIncrease = 20;
        }


        public override Projectile Fire(Vector2 position)
        {
            Vector2 direction = PrepareAttack(position);
            if (ButtonManager.soundIsOn)
                AssetManager.soundEffects["flamethrower"].Play(0.40f, 0, 0);
            return new Flames(this.position, direction, flameSize, range, damage);
        }
    }
}
