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
    abstract class Ally : Entity
    {
        public Ally(Texture2D texture, Vector2 scale, float speed, int value, int numberOfImages, int pathNumber) : base(texture, scale, speed, value, numberOfImages, pathNumber)
        {

        }
    }
}
