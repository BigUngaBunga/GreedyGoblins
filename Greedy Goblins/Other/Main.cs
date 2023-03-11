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
    enum GameState
    {
        MainMenu, LevelSelect, Playing, Options, GameOver, Score, Exit, LevelEditor, EditorInformation, GameplayInformation, Credits
    }

    public class Main : Game
    {
        public static Rectangle GameScreen { get; private set; }
        public static Vector2 ScreenScale { get; private set; }
        public static Random random;
        public static Matrix ResolutionMatrix { get; private set; }
        ButtonManager buttonManager;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState gameState, oldGameState;
        LevelManager levelManager;
        LevelEditor levelEditor;
        GameOverMessage gameOverMessage;
        ScoreHandler scoreHandler;
        Vector2 targetResolution;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            random = new Random();
            targetResolution = new Vector2(1920, 1080);
            ChangeResolution(targetResolution);
            scoreHandler = new ScoreHandler();
            GameScreen = new Rectangle(0, 0, (int)targetResolution.X, (int)targetResolution.Y);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetManager.LoadContent(Content);
            buttonManager = new ButtonManager(GameState.MainMenu);
            levelEditor = new LevelEditor(spriteBatch);
            PlayMusic();
            ChangeResolution(targetResolution);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyMouseReader.Update();
            ChangeGameState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                gameState = GameState.Exit;

            buttonManager.Update();
            UpdateGameStates(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, ResolutionMatrix);

            DrawGameStates(spriteBatch);
            buttonManager.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void UpdateGameStates(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    if (buttonManager.saveScore)
                    {
                        buttonManager.saveScore = false;
                        scoreHandler.SaveScore(levelManager.Score);
                    }
                    break;
                case GameState.LevelSelect:
                    CreateLevelManager();
                    ToggleButtonsVisible();
                    break;
                case GameState.Playing:
                    levelManager.Update(gameTime);
                    break;
                case GameState.Options:
                    if (buttonManager.resolutionWasChanged)
                    {
                        buttonManager.resolutionWasChanged = false;
                        ChangeResolution(buttonManager.Resolution.ToVector2());
                        buttonManager.RescaleButtons();
                    }
                    break;
                case GameState.GameOver:
                    CreateGameOverScreen();
                    break;
                case GameState.Score:
                    if (ChangedToGameState(GameState.Score))
                        scoreHandler.ReadScores();
                    break;
                case GameState.LevelEditor:
                    levelEditor.Update(gameTime);
                    ToggleButtonsVisible();
                    break;
                case GameState.Exit:
                    Exit();
                    break;
            }
        }

        private void DrawGameStates(SpriteBatch spriteBatch)
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(AssetManager.textures["title screen"], Vector2.Zero, Color.White);
                    break;

                case GameState.Score:
                    spriteBatch.Draw(AssetManager.textures["mountain map"], GameScreen, Color.White);
                    spriteBatch.Draw(AssetManager.textures["chalk board"], GameScreen, Color.White);
                    //ritat text på tavlan
                    spriteBatch.DrawString(AssetManager.fonts["title"], "Scoreboard", new Vector2(GameScreen.Width / 3, GameScreen.Height / 5), Color.White);
                    for (int i = 0; i < 10; i++)
                        spriteBatch.DrawString(AssetManager.fonts["button"], scoreHandler.ScoreList[i].ToString(), 
                                                new Vector2(GameScreen.Width / 7 * (1 + i % 5), GameScreen.Height / 3 * (1 + i /5)), Color.White);
                    break;

                case GameState.LevelSelect:
                case GameState.Playing:
                    levelManager.Draw(spriteBatch);
                    break;

                case GameState.Options:
                    spriteBatch.Draw(AssetManager.textures["options background"], Vector2.Zero, Color.White);
                    foreach (Rectangle rectangle in buttonManager.optionsBackCovers)
                        spriteBatch.Draw(AssetManager.textures["borderless button"], rectangle, new Color(112, 128, 144, 215));
                    break;

                case GameState.GameOver:
                    spriteBatch.Draw(AssetManager.textures["shrubland map"], Vector2.Zero, Color.White);
                    gameOverMessage.Draw(spriteBatch);
                    break;

                case GameState.LevelEditor:
                    spriteBatch.Draw(AssetManager.textures["shrubland map"], Vector2.Zero, Color.White);
                    levelEditor.Draw(spriteBatch);
                    break;

                case GameState.EditorInformation:
                    spriteBatch.Draw(AssetManager.textures["editor info"], Vector2.Zero, Color.White);
                    break;

                case GameState.GameplayInformation:
                    spriteBatch.Draw(AssetManager.textures["game info"], Vector2.Zero, Color.White);
                    break;
                case GameState.Credits:
                    spriteBatch.Draw(AssetManager.textures["credits"], Vector2.Zero, Color.White);
                    break;
            }
        }

        private void ChangeGameState()
        {
            oldGameState = gameState;
            if (buttonManager.gameState != gameState)
            {
                gameState = buttonManager.gameState;
                PlayMusic();
            }
        }

        private bool ChangedToGameState(GameState gameState)
        {
            if (buttonManager.gameState == gameState && oldGameState != gameState)
                return true;            

            return false;
        }

        private void ChangeResolution(Vector2 newResolution)
        {
            if (buttonManager != null)
                graphics.IsFullScreen = buttonManager.isFullscreen;

            graphics.PreferredBackBufferWidth = (int)newResolution.X;
            graphics.PreferredBackBufferHeight = (int)newResolution.Y;

            float scaleX = graphics.PreferredBackBufferWidth / targetResolution.X;
            float scaleY = graphics.PreferredBackBufferHeight / targetResolution.Y;
            ScreenScale = new Vector2(scaleX, scaleY);

            ResolutionMatrix = Matrix.CreateScale(ScreenScale.X, ScreenScale.Y, 1f);
            graphics.ApplyChanges();
        }

        private void CreateLevelManager()
        {
            if (ChangedToGameState(GameState.LevelSelect))
                levelManager = new LevelManager(buttonManager, spriteBatch);
        }

        private void CreateGameOverScreen()
        {
            if (ChangedToGameState(GameState.GameOver))
                gameOverMessage = new GameOverMessage(levelManager.PlayState, levelManager.Score);
        }


        private void PlayMusic()
        {
            if (gameState == GameState.Playing)
            {
                AssetManager.musicInstances["menu theme"].Stop();
                AssetManager.musicInstances["battle theme"].Play();
            }
            else
            {
                AssetManager.musicInstances["menu theme"].Play();
                AssetManager.musicInstances["battle theme"].Stop();
            }
        }

        private void ToggleButtonsVisible()
        {
            if (KeyMouseReader.KeyPressed(Keys.Space))
                buttonManager.ToggleHiddenButtons();
                
        }
    }
}
