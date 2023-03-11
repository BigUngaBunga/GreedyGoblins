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
    class TowerInformation
    {
        Texture2D PaperTexture => AssetManager.textures["paper"];
        SpriteFont Font => AssetManager.fonts["information"];

        public TowerInformation()
        {
        }
        
        
        public void DrawGeneralTowerInformation(SpriteBatch spriteBatch, Vector2 position, int cost, int fireRate, int damage)
        {
            string fireRateString;
            if (fireRate <= 250)
                fireRateString = "fast";
            else if (fireRate <= 1000)
                fireRateString = "medium";
            else
                fireRateString = "slow";

            Vector2 scale = new Vector2(0.75f, 0.6f);

            position.Y -= PaperTexture.Height * scale.Y;

            spriteBatch.Draw(PaperTexture, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(Font, $"Cost: {cost}\nDamage: {damage}\nFire rate: {fireRateString}", position, Color.Black);
        }


        //Ritar information om specifikt torn
        public void DrawSpecificTowerInformation(SpriteBatch spriteBatch, Tower tower, bool focusingUpgradeButton)
        {
            Vector2 position = new Vector2(Main.GameScreen.Width - PaperTexture.Width, 0);
            int level, damage, range, fireRate;

            if (focusingUpgradeButton)
            {
                level = tower.UpgradedStats[0];
                damage = tower.UpgradedStats[1];
                range = tower.UpgradedStats[2];
                fireRate = tower.UpgradedStats[3];
            }
            else
            {
                level = tower.Stats[0];
                damage = tower.Stats[1];
                range = tower.Stats[2];
                fireRate = tower.Stats[3];
            }

            spriteBatch.Draw(PaperTexture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Font, $"Tower level: {level}, Damage: {damage} \nRange: {range}, Cooldown: {fireRate}\nCost to upgrade: { tower.UpgradeCost}",
                                    position, Color.Black, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
        }
    }
}
