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
    class UpgradeButton : TextButton
    {
        public UpgradeButton(Rectangle originalHitbox, Texture2D buttonTexture): base(originalHitbox, buttonTexture)
        {
            buttonText = "Upgrade";
            SetTextPosition();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
