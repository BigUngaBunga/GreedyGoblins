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
    class TowerButton : Button
    {
        Texture2D towerTexture;
        public TowerType TowerType { get; }

        public TowerButton(Rectangle originalHitbox, Texture2D buttonTexture, TowerType towerType): base(originalHitbox, buttonTexture)
        {
            RescaleHitbox();
            TowerType = towerType;

            switch (towerType)
            {
                case TowerType.Crossbow:
                    towerTexture = AssetManager.textures["crossbowman"];
                    break;
                case TowerType.Cannon:
                    towerTexture = AssetManager.textures["cannon"];
                    break;
                case TowerType.FlameThrower:
                    towerTexture = AssetManager.textures["flamethrower"];
                    break;
            }

            focusedColour = new Color(140, 75, 65);
            colour = neutralColour = new Color(120, 65, 50);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(buttonTexture, originalHitbox, colour);
            spriteBatch.Draw(towerTexture, originalHitbox, Color.White);
        }
    }
}
