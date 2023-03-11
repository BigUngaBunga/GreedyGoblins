using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Greedy_Goblins
{
    class LevelManager
    {
        public PlayState PlayState { get { return levels[pickedLevel].PlayState; } }
        public int Score { get { return levels[pickedLevel].Score; } }
        ButtonManager buttonManager;
        LevelType pickedLevel;
        Dictionary<LevelType, Level> levels;

        public LevelManager(ButtonManager buttonManager, SpriteBatch spriteBatch)
        {
            this.buttonManager = buttonManager;
            
            levels = new Dictionary<LevelType, Level>();
            CreateLevels(spriteBatch);
        }


        public void Update(GameTime gameTime)
        {
            PickALevel();
            if (levels.ContainsKey(pickedLevel))
                levels[pickedLevel].Update(gameTime);
        }


        public void Draw(SpriteBatch spriteBatch)
        {   
            if (buttonManager.gameState == GameState.Playing)
                levels[pickedLevel].Draw(spriteBatch);
            else if (buttonManager.gameState == GameState.LevelSelect)
                levels[buttonManager.FocusedLevelButton()].DrawPreview(spriteBatch);
        }


        //Skapar nivåer
        private void CreateLevels(SpriteBatch spriteBatch)
        {
            Difficulty difficulty = buttonManager.Difficulty;

            LevelType[] levelTypes = new LevelType[] { LevelType.Forest, LevelType.Mountain, LevelType.Shrubland, LevelType.Snow };
            foreach (LevelType levelType in levelTypes)
            {
                levels.Add(levelType, new Level(difficulty, levelType, spriteBatch, buttonManager));
            }
        }


        //Väljer nivå
        private void PickALevel()
        {
            if (buttonManager.pickedALevel)
            {
                buttonManager.pickedALevel = false;
                pickedLevel = buttonManager.levelType;
            }
        }
    }
}
