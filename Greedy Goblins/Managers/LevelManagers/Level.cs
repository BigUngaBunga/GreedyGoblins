using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using CatmullRom;

namespace Greedy_Goblins
{
    enum Difficulty
    {
        Easy, Normal, Hard
    }
    enum PlayState
    {
        Playing, Won, Lost
    }

    class Level
    {
        public PlayState PlayState { get; private set; }
        public int Score { get { return player.Score; } }
        Difficulty difficulty;
        LevelType levelType;
        Player player;
        GraphicsDevice GraphicsDevice;
        ButtonManager buttonManager;
        SpriteBatch spriteBatch;
        MapManager mapManager;
        EntityManager entityManager;
        TowerManager towerManager;
        WaveLoader waveLoader;
        ProjectileManager projectileManager;
        bool showUI;
        float betweenWaveTimer, betweenWaveTime;
        int waveNumber, goldPerWave;
        Texture2D boardTexture;
        Vector2 boardPosition;

        public Level(Difficulty difficulty, LevelType levelType, SpriteBatch spriteBatch, ButtonManager buttonManager)
        {
            this.difficulty = difficulty;
            this.levelType = levelType;
            this.spriteBatch = spriteBatch;
            this.buttonManager = buttonManager;
            PlayState = PlayState.Playing;
            GraphicsDevice = spriteBatch.GraphicsDevice;
            mapManager = new MapManager(spriteBatch, levelType);
            entityManager = new EntityManager();
            player = new Player(difficulty);
            waveLoader = new WaveLoader(levelType);
            towerManager = new TowerManager(buttonManager);
            projectileManager = new ProjectileManager();
            showUI = true;
            betweenWaveTime = 10000;
            goldPerWave = 25;
            boardTexture = AssetManager.textures["wooden board"];
            boardPosition = new Vector2(Main.GameScreen.Width - boardTexture.Width, Main.GameScreen.Height - boardTexture.Height);
        }


        public void Update(GameTime gameTime)
        {   
            HideUI();
            CheckIfGameIsOver();
            PrepareNextWave();
            SkipTimer();
            //Uppdaterar entiteter om tiden mellan vågor är 0
            if (betweenWaveTimer > 0)
                betweenWaveTimer -= gameTime.ElapsedGameTime.Milliseconds;
            else
                entityManager.Update(gameTime, mapManager.Paths);
            //Hanterar torn
            towerManager.Update(gameTime);
            UpgradeTower();
            towerManager.FireProjectiles(projectileManager.projectiles, entityManager.Entities);
            PlaceTowers();
            //Hanterar projektiler
            projectileManager.Update(gameTime, entityManager.Entities);
            //Rensar entiteter och projektiler
            Cleanup();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            mapManager.Draw(spriteBatch);
            entityManager.Draw(spriteBatch);
            towerManager.Draw(spriteBatch);
            projectileManager.Draw(spriteBatch);
            DrawUI(spriteBatch);
        }

        //Ritar enbart karta och väg
        public void DrawPreview(SpriteBatch spriteBatch)
        {
            mapManager.Draw(spriteBatch);
        }


        //Rensar upp fiender och projektiler
        private void Cleanup()
        {   //Rensar fiender och lägger till guld eller tar bort liv beroende på hur de försvann
            entityManager.Cleanup(out int gold, out int health);
            player.health += health;
            player.GainGold(gold);
        }

        private void PrepareNextWave()
        {
            if (entityManager.WaveOver)
                if (waveLoader.RemainingWaves > 0)
                {
                    waveNumber++;
                    player.GainGold(goldPerWave);
                    entityManager.SetWave(waveLoader.GetNextWave());
                    betweenWaveTimer = betweenWaveTime;
                }
                    
        }


        private void CheckIfGameIsOver()
        {   
            if (player.health <= 0)
            {
                buttonManager.GameOver();
                PlayState = PlayState.Lost;
            }
            else if (entityManager.WaveOver)
            {   
                if (waveLoader.RemainingWaves <= 0)
                {
                    buttonManager.GameOver();
                    PlayState = PlayState.Won;
                }
            }
        }

        private void PlaceTowers()
        {
            if (!towerManager.AddedATower)
                foreach (Tower tower in towerManager.Towers)
                    if (!tower.isPlaced)
                    {
                        bool canBePlaced = false;
                        if (player.Gold >= tower.Cost)
                            canBePlaced = mapManager.CanPlaceTower(tower);
                        tower.UpdateSizeColour(canBePlaced);

                        if (KeyMouseReader.LeftClick())
                        {
                            if (canBePlaced)
                            {
                                mapManager.AddGameObject(tower);
                                tower.isPlaced = true;
                                player.SpendGold(tower.Cost);
                                towerManager.placingTower = true;
                                AssetManager.soundInstances["place tower"].Play();
                                towerManager.placingTower = false;
                            }
                            else
                            {
                                AssetManager.soundInstances["cant place tower"].Play();
                            }
                        }
                            
                    }
        }

        private void UpgradeTower()
        {
            if (KeyMouseReader.LeftClick() && buttonManager.FocusedUpgradeButton())
            {
                player.SpendGold(towerManager.UpgradeTower(player.Gold));
            }
        }

        private void HideUI()
        {
            if (KeyMouseReader.KeyPressed(Keys.Space))
            {
                buttonManager.ToggleHiddenButtons();
                showUI = !showUI;
            }
        }

        private void SkipTimer()
        {
            if (KeyMouseReader.KeyPressed(Keys.Enter))
            {
                float goldPerSecond = 3;
                player.GainGold((int)(betweenWaveTimer / 1000 * goldPerSecond));
                betweenWaveTimer = 0;
            }
            else if (waveNumber == 1)
                betweenWaveTimer = 0;
        }

        private void DrawUI(SpriteBatch spriteBatch)
        {
            if (showUI)
            {
                spriteBatch.Draw(boardTexture, boardPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(AssetManager.fonts["information"], $"HP: {player.health} \nGold: {player.Gold} \nScore: {player.Score} \nWaves: {waveLoader.RemainingWaves}",
                                        boardPosition + new Vector2(boardTexture.Width / 5, boardTexture.Height / 10), Color.LightCyan, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                towerManager.ShowTowerInfo(spriteBatch);
            }

            if (betweenWaveTimer > 0)
                spriteBatch.DrawString(AssetManager.fonts["button"], $"Next wave in: {(int)betweenWaveTimer / 1000}\nHit enter to start immediately", new Vector2(Main.GameScreen.Width / 3, 100), Color.Black);
        }
    }
}
