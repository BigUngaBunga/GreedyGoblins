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
    class King : Enemy
    {
        public King(Vector2 scale, float speed, int value, int damage, int health, int pathNumber) : base(AssetManager.textures["king"], scale, speed, value, 4, damage, health, pathNumber)
        {

        }
    }
}
