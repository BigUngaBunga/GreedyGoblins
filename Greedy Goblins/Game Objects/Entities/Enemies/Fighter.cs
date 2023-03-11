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
    class Fighter : Enemy
    {
        float positionFix, healthBarFix;


        public Fighter(Vector2 scale, float speed, int value, int damage, int health, int pathNumber) : base(AssetManager.textures["fighter"], scale, speed, value, 4, damage, health, pathNumber)
        {   //Korrigerar texturens mittposition och health bar
            positionFix = sourceRectangle.Width / 5;
            healthBarFix = sourceRectangle.Width / 8;
            pointOfOrigin.X -= positionFix;
        }

        protected override void UpdateHealthBar()
        {
            base.UpdateHealthBar();
            healthBar.X += (int)healthBarFix; 
        }
    }
}
