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
    class Soldier : Enemy
    {
        public Soldier(Vector2 scale, float speed, int value, int damage, int health, int pathNumber) : base(AssetManager.textures["soldier"], scale, speed, value, 4, damage, health, pathNumber)
        {

        }
    }
}
