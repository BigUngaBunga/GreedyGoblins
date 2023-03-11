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
    class ToggleButton: OptionButton
    {
        public bool isActive;
        Color activeColour, focusedActiveColour;

        public ToggleButton(ButtonType buttonType, Rectangle originalHitbox, Texture2D buttonTexture): base(buttonType, originalHitbox, buttonTexture)
        {
            isActive = false;
            activeColour = new Color(69, 210, 42);
            focusedActiveColour = new Color(89, 230, 62);
        }

        public override void Update()
        {
            if (isActive && InFocus())//Är aktiv och mus är över
                colour = focusedActiveColour;
            else if (isActive)//Är aktiv
                colour = activeColour;
            else
                base.Update();
        }
    }
}
