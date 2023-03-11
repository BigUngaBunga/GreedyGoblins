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
    class GameOverMessage
    {
        string[] messages;

        public GameOverMessage(PlayState playState, int score)
        {
            LoadMessage(playState, score);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetManager.textures["forest map"], Main.GameScreen, Color.White);

            for (int i = 0; i < messages.Length; i++)
            {
                if (i == 0)
                    spriteBatch.DrawString(AssetManager.fonts["title"], messages[i], new Vector2(Main.GameScreen.Width / 3 - messages[i].Length * 5, 0), Color.Black);
                else
                    spriteBatch.DrawString(AssetManager.fonts["button"], messages[i], new Vector2(Main.GameScreen.Width / 3 - messages[i].Length * 5, 50 + 100 * i), Color.Black);

            }
        }

        private void LoadMessage(PlayState playState, int score)
        {
            switch (playState)
            {
                case PlayState.Won:
                    messages = new string[] { "You Won!", $"And got a total of {score} points", "Well done!" };
                    break;
                case PlayState.Lost:
                    messages = new string[] { "You Lost", $"And got a total of {score} points", "Better luck next time!" };
                    break;
                default:
                    messages = new string[] { "Wait", "You shouldn't be here", "Strange"};
                    break;
            }
        }
    }
}
