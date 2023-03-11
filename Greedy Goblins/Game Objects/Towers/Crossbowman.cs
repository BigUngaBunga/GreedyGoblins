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
    class Crossbowman : Tower
    {
        public Crossbowman(Vector2 scale, float range, float size, int damage, int fireRate, int cost) : 
                            base(AssetManager.textures["crossbowman"], scale, range, size, damage, fireRate, cost)
        {
            damageIncrease = 2;
            fireRateDecrease = 10;
            rangeIncrease = 50;

        }

        public override Projectile Fire(Vector2 position)
        {
            Vector2 direction = PrepareAttack(position);
            
            if (ButtonManager.soundIsOn)
                AssetManager.soundEffects["crossbowman"].Play(0.75f, 0, 0);
            return new Quarrel(this.position, direction, damage);
        }

        public override void RotateTowardsTarget(Vector2 position)
        {
            base.RotateTowardsTarget(position);
            rotation += MathHelper.ToRadians(20);
        }
    }
}
