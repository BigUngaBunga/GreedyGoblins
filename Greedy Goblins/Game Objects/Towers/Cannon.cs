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
    class Cannon : Tower
    {
        public Cannon(Vector2 scale, float range, float size, int damage, int fireRate, int cost) : 
                        base(AssetManager.textures["cannon"], scale, range, size, damage, fireRate, cost)
        {
            damageIncrease = 2;
            fireRateDecrease = 50;
            rangeIncrease = 25;
        }

        public override Projectile Fire(Vector2 position)
        {
            Vector2 direction = PrepareAttack(position);
            if (ButtonManager.soundIsOn)
                AssetManager.soundEffects["cannon"].Play(0.6f, 0, 0);

            return new Cannonball(this.position, direction, Range / 3, damage);
        }
    }
}
