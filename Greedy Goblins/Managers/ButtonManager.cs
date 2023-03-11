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
    class ButtonManager
    {
        public GameState gameState;
        public List<GameState> previousGameStates;
        public Difficulty Difficulty { get; private set; }
        public LevelType levelType, focusedLevel;
        public List<Rectangle> optionsBackCovers { get; private set; }
        public TowerType TowerTypeToAdd { get; private set; }
        Dictionary<GameState, Button[]> buttons;
        bool hasSound, hasMusic;
        public static bool soundIsOn;
        public bool resolutionWasChanged, pickedALevel, isFullscreen, addATower, saveScore;
        public Point Resolution { get; private set; }
        
        private readonly int buttonWidth, buttonHeight;

        public ButtonManager(GameState gameState)
        {
            this.gameState = gameState;
            previousGameStates = new List<GameState>();
            Difficulty = Difficulty.Normal;
            focusedLevel = levelType = LevelType.Forest;
            InitiateBools();
            buttonWidth = Main.GameScreen.Width / 5;
            buttonHeight = 100;
            LoadButtons();
        }


        public void Update()
        {
            if (buttons.ContainsKey(gameState))
                foreach (Button button in buttons[gameState])
                {
                    button.Update();
                    if (!button.isHidden)
                    {   
                        if (button.InFocus() && !button.InFocus(KeyMouseReader.oldMouseState.Position))
                        {
                            AssetManager.soundInstances["select button"].Stop();
                            AssetManager.soundInstances["select button"].Play();
                        }

                        
                        if (KeyMouseReader.LeftClick() && button.InFocus())
                        {   
                            if (button is OptionButton optionButton)
                                PressedAButton(optionButton.ButtonType);
                            else if (button is LevelButton levelButton)
                                PressedAButton(levelButton.LevelType);
                            else if (button is TowerButton towerButton)
                                PressedAButton(towerButton.TowerType);

                            AssetManager.soundInstances["click button"].Play();
                        }
                    }
                }
        }

        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (buttons.ContainsKey(gameState))
                foreach (Button button in buttons[gameState])
                    if (!button.isHidden && !(button is UpgradeButton))
                        button.Draw(spriteBatch);
        }


        public void DrawUpgradeButton(SpriteBatch spriteBatch)
        {
            if (buttons.ContainsKey(gameState))
                foreach (Button button in buttons[gameState])
                    if (!button.isHidden && button is UpgradeButton upgradeButton)
                        upgradeButton.Draw(spriteBatch);
        }

        private void InitiateBools()
        {
            soundIsOn = hasSound = true;
            hasMusic = true;
            resolutionWasChanged = false;
            pickedALevel = false;
            isFullscreen = true;
        }

        private void LoadButtons()
        {
            optionsBackCovers = new List<Rectangle>();
            buttons = new Dictionary<GameState, Button[]>();
            GameState[] gameStates = new GameState[] { GameState.Options, GameState.MainMenu, GameState.Score, GameState.GameOver, GameState.LevelEditor,
                                                        GameState.EditorInformation, GameState.GameplayInformation, GameState.Credits};

            List<ButtonType[]> menus = new List<ButtonType[]>();
            menus.Add(new ButtonType[] { ButtonType.Back, ButtonType.EditMap, ButtonType.Info, ButtonType.Easy, ButtonType.Normal, ButtonType.Hard,
                                          ButtonType.SetResolution, ButtonType.Resolution, ButtonType.Fullscreen, ButtonType.Sound, ButtonType.Music, ButtonType.Credits});//Options

            menus.Add(new ButtonType[] { ButtonType.Play, ButtonType.Scoreboard, ButtonType.Options, ButtonType.Exit });//Main menu
            menus.Add(new ButtonType[] { ButtonType.Back});//Score
            menus.Add(new ButtonType[] { ButtonType.Play, ButtonType.Back, ButtonType.SaveScore, ButtonType.Exit});//Game over
            menus.Add(new ButtonType[] { ButtonType.Back, ButtonType.Info, ButtonType.FormEdit });//Map editor
            menus.Add(new ButtonType[] { ButtonType.Back });//Mapeditor information
            menus.Add(new ButtonType[] { ButtonType.Back });//Game information
            menus.Add(new ButtonType[] { ButtonType.Back });//Credits

            foreach (ButtonType[] buttonTypes in menus)//Skapar knappar för varje meny
            {
                Button[] buttonArray = new Button[buttonTypes.Length];
                if (menus.IndexOf(buttonTypes) == 0)
                    for (int i = 0; i < buttonTypes.Length; i++)
                        buttonArray[i] = CreateOptionButton(i, buttonTypes[i]);
                else
                    for (int i = 0; i < buttonTypes.Length; i++)
                        buttonArray[i] = CreateButton(i, buttonTypes[i]);

                //Lägger till array i dictionary med spelläge som nyckel
                buttons.Add(gameStates[menus.IndexOf(buttonTypes)], buttonArray);
            }
            buttons.Add(GameState.LevelSelect, LoadLevelButtons());
            buttons.Add(GameState.Playing, LoadTowerButtons());
        }


        //Skapar knappar för att välja nivå
        private Button[] LoadLevelButtons()
        {
            LevelType[] levelTypes = new LevelType[] { LevelType.Forest, LevelType.Shrubland, LevelType.Mountain, LevelType.Snow };

            Button[] buttonArray = new Button[levelTypes.Length + 2];

            for (int i = 0; i < buttonArray.Length; i++)
            {
                if (i == levelTypes.Length)
                    buttonArray[i] = CreateButton(i, ButtonType.Back);
                else if (i == levelTypes.Length + 1)
                    buttonArray[i] = CreateButton(i, ButtonType.Info);
                else
                    buttonArray[i] = CreateButton(i, levelTypes[i]);

            }
            return buttonArray;
        }

        private Button[] LoadTowerButtons()
        {
            TowerType[] towerTypes = new TowerType[] { TowerType.Crossbow, TowerType.Cannon, TowerType.FlameThrower};

            List<Button> buttonList = new List<Button>();

            for (int i = 0; i < towerTypes.Length; i++)
                buttonList.Add(CreateButton(i, towerTypes[i]));

            buttonList.Add(CreateButton());
            return buttonList.ToArray();
        }


        private Button CreateButton(int i, ButtonType buttonType)
        {
            Point location;
            int buttonsPerRow = 4;
            int widthBuffer = 15, heightBuffer = 40;

            location.X = (Main.GameScreen.Width / buttonsPerRow + widthBuffer) * (i % buttonsPerRow) + widthBuffer;
            location.Y = Main.GameScreen.Bottom - (buttonHeight + heightBuffer) * (1 + i / buttonsPerRow);

            Rectangle hitbox = new Rectangle(location, new Point(buttonWidth, buttonHeight));

            return NewOptionButton(buttonType, hitbox);
        }
        private Button CreateButton(int i, LevelType levelType)
        {
            int buttonsPerRow = 4;
            int widthBuffer = 15, heightBuffer = 40;

            Point location = Point.Zero;
            location.X = (Main.GameScreen.Width / buttonsPerRow + widthBuffer) * (i % buttonsPerRow) + widthBuffer;
            location.Y = Main.GameScreen.Bottom - (buttonHeight + heightBuffer) * (1 + i / buttonsPerRow);

            Rectangle hitbox = new Rectangle(location, new Point(buttonWidth, buttonHeight));

            Texture2D texture = AssetManager.textures["button"];

            return new LevelButton(hitbox, texture, levelType);
        }
        private Button CreateButton(int i, TowerType towerType)
        {
            int buttonsPerRow = 4;
            int side = Main.GameScreen.Width / 10;
            int widthBuffer = 20, heightBuffer = 20;

            Point location = Point.Zero;
            location.X = (side + widthBuffer) * (i % buttonsPerRow) + widthBuffer;
            location.Y = Main.GameScreen.Bottom - (side + heightBuffer) * (1 + i / buttonsPerRow);

            Rectangle hitbox = new Rectangle(location, new Point(side));

            Texture2D texture = AssetManager.textures["large circle"];

            return new TowerButton(hitbox, texture, towerType);
        }
        private Button CreateButton()
        {
            int heightBuffer = 20;

            Point location = Point.Zero;
            location.X = Main.GameScreen.Width - AssetManager.textures["paper"].Width / 2 - buttonWidth / 2;
            location.Y = AssetManager.textures["paper"].Height - buttonHeight - heightBuffer;

            Rectangle hitbox = new Rectangle(location, new Point(buttonWidth, buttonHeight));

            Texture2D texture = AssetManager.textures["button"];

            return new UpgradeButton(hitbox, texture);
        }


        private Button CreateOptionButton(int i, ButtonType buttonType)
        {
            Point location;
            int buttonsPerRow = 3;
            int widthBuffer = buttonWidth / 3, heightBuffer = 100;

            location.X = (Main.GameScreen.Width / 3) * (i % buttonsPerRow) + widthBuffer;
            location.Y = Main.GameScreen.Bottom - (buttonHeight + heightBuffer) * (1 + i / buttonsPerRow);

            Rectangle hitbox = new Rectangle(location, new Point(buttonWidth, buttonHeight));

            if (i % buttonsPerRow == 0)
            {
                Point positionOffset = new Point(30, 25);
                optionsBackCovers.Add(new Rectangle(location - positionOffset, new Point((Main.GameScreen.Width / 3 * 2 + widthBuffer * 3) + positionOffset.X * 2 , buttonHeight + positionOffset.Y * 2)));//(buttonWidth + widthBuffer / 2) * 3 + positionOffset.X * 2
            }

            return NewOptionButton(buttonType, hitbox);
        }

        private Button NewOptionButton(ButtonType buttonType, Rectangle hitbox)
        {
            Texture2D texture = AssetManager.textures["button"];

            switch (buttonType)
            {
                case ButtonType.Easy:
                case ButtonType.Normal:
                case ButtonType.Hard:
                case ButtonType.Sound:
                case ButtonType.Music:
                case ButtonType.Fullscreen:
                    ToggleButton toggleButton = new ToggleButton(buttonType, hitbox, texture);

                    if (buttonType == ButtonType.Normal || buttonType == ButtonType.Sound || buttonType == ButtonType.Music || buttonType == ButtonType.Fullscreen)
                        toggleButton.isActive = true;
                    return toggleButton;

                case ButtonType.Resolution:
                    ResolutionButton resolutionButton = new ResolutionButton(buttonType, hitbox, texture);
                    Resolution = resolutionButton.CurrentResolution;
                    return resolutionButton;

                default:
                    return new OptionButton(buttonType, hitbox, texture);
            }
        }

        private void PressedAButton(ButtonType buttonType)
        {
            PressedOptionButton(buttonType);
            PressedToggleButton(buttonType);
            HandleToggleButtons(buttonType);
        }
        private void PressedAButton(LevelType levelType)
        {   
            this.levelType = levelType;
            gameState = GameState.Playing;
            pickedALevel = true;
        }
        private void PressedAButton(TowerType towerType)
        {   
            TowerTypeToAdd = towerType;
            addATower = true;
        }


        private void PressedOptionButton(ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.Play:
                    ChangeGameState(GameState.LevelSelect);
                    break;
                case ButtonType.Back:
                    if (gameState == previousGameStates.Last())
                    {
                        previousGameStates.Remove(previousGameStates.Last());
                        PressedOptionButton(ButtonType.Back);
                    }
                    else
                    {
                        gameState = previousGameStates.Last();
                        previousGameStates.Remove(previousGameStates.Last());
                    }
                    break;
                case ButtonType.Menu:
                    ChangeGameState(GameState.MainMenu);
                    break;
                case ButtonType.Options:
                    ChangeGameState(GameState.Options);
                    break;
                case ButtonType.Scoreboard:
                    ChangeGameState(GameState.Score);
                    break;
                case ButtonType.Exit:
                    ChangeGameState(GameState.Exit);
                    break;
                case ButtonType.EditMap:
                    ChangeGameState(GameState.LevelEditor);
                    break;
                case ButtonType.Easy:
                    Difficulty = Difficulty.Easy;
                    break;
                case ButtonType.Normal:
                    Difficulty = Difficulty.Normal;
                    break;
                case ButtonType.Hard:
                    Difficulty = Difficulty.Hard;
                    break;
                case ButtonType.FormEdit:
                    LevelEditor.openForm = true;
                    break;
                case ButtonType.Info:
                    if (gameState == GameState.LevelEditor)
                        ChangeGameState(GameState.EditorInformation);
                    else
                        ChangeGameState(GameState.GameplayInformation);
                    break;
                case ButtonType.Credits:
                    ChangeGameState(GameState.Credits);
                    break;
                case ButtonType.SaveScore:
                    saveScore = true;
                    break;
            }
        }


        //När växelknapp trycks
        private void PressedToggleButton(ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.Sound://Slå på eller av ljudeffekter
                    if (hasSound)//Tystar alla ljud
                        foreach (var sounds in AssetManager.soundInstances)
                            sounds.Value.Volume = 0;

                    else//Återställer alla ljud
                        foreach (var sounds in AssetManager.soundInstances)
                            sounds.Value.Volume = AssetManager.audioVolume[sounds.Key];

                    soundIsOn = hasSound = !hasSound;
                    break;

                case ButtonType.Music://Slå på eller av musik
                    if (hasMusic)//Tystar all musik
                        foreach (var music in AssetManager.musicInstances)
                            music.Value.Volume = 0;

                    else//Återställer all musik
                        foreach (var music in AssetManager.musicInstances)
                            music.Value.Volume = AssetManager.audioVolume[music.Key];

                    hasMusic = !hasMusic;
                    break;

                case ButtonType.SetResolution://Ändrar upplösningen
                    foreach (Button button in buttons[gameState])
                        if (button is ResolutionButton resolutionButton)
                        {
                            Resolution = resolutionButton.CurrentResolution;
                            resolutionWasChanged = true;
                        }
                    break;

                case ButtonType.Resolution://Växlar mellan upplösningar
                    foreach (Button button in buttons[gameState])
                        if (button is ResolutionButton resolutionButton)
                            resolutionButton.NextResolution();
                    break;
                case ButtonType.Fullscreen:
                    isFullscreen = !isFullscreen;
                    break;
        }
    }


        //Aktiverar och avaktiverar växelknappar
        private void HandleToggleButtons(ButtonType buttonType)
        {
            switch (buttonType)
            {   //Svårighetsgradsknappar
                case ButtonType.Easy:
                case ButtonType.Normal:
                case ButtonType.Hard:
                    foreach (Button button in buttons[gameState])
                    {
                        if (button is ToggleButton toggleButton)
                        {
                            if (toggleButton.ButtonType == ButtonType.Easy || toggleButton.ButtonType == ButtonType.Normal || toggleButton.ButtonType == ButtonType.Hard)
                            {   
                                if (toggleButton.ButtonType == buttonType)
                                    toggleButton.isActive = true;
                                else
                                    toggleButton.isActive = false;
                            }
                        }
                    }
                    break;
                case ButtonType.Sound:
                    foreach (Button button in buttons[gameState])
                    {
                        if (button is ToggleButton toggleButton && toggleButton.ButtonType == ButtonType.Sound)
                            toggleButton.isActive = hasSound;

                    }
                    break;

                case ButtonType.Music:
                    foreach (Button button in buttons[gameState])
                    {
                        if (button is ToggleButton toggleButton && toggleButton.ButtonType == ButtonType.Music)
                            toggleButton.isActive = hasMusic;

                    }
                    break;
                case ButtonType.Fullscreen:
                    foreach (Button button in buttons[gameState])
                    {
                        if (button is ToggleButton toggleButton && toggleButton.ButtonType == ButtonType.Fullscreen)
                            toggleButton.isActive = isFullscreen;
                    }
                    break;
            }
        }


        //Skalar om knappar
        public void RescaleButtons()
        {
            foreach (Button[] buttonArray in buttons.Values)
            {
                foreach (Button button in buttonArray)
                {
                    button.RescaleHitbox();
                }
            }
        }


        //Gör knappar osynliga/synliga
        public void ToggleHiddenButtons()
        {
            if (buttons.ContainsKey(gameState))
                foreach (Button button in buttons[gameState])
                    button.isHidden = !button.isHidden;
        }


        //Ändrar gameState och lägger till i lista
        private void ChangeGameState(GameState gameState)
        {
            previousGameStates.Add(this.gameState);
            this.gameState = gameState;
        }


        //Returnerar nivåtyp som är i fokus
        public LevelType FocusedLevelButton()
        {
            foreach (Button button in buttons[gameState])
            {
                if (button is LevelButton levelButton && !levelButton.isHidden && levelButton.InFocus())
                    focusedLevel = levelButton.LevelType;
            }

            return focusedLevel;
        }


        //Returnerar torntyp om dens knapp är i fokus
        public bool FocusedTowerButton(out TowerType towerType, out Vector2 buttonPosition)
        {
            towerType = TowerType.Crossbow;
            buttonPosition = Vector2.Zero;

            foreach (Button button in buttons[gameState])
            {
                if (button is TowerButton towerButton)
                    if (!towerButton.isHidden && towerButton.InFocus())
                    {
                        towerType = towerButton.TowerType;
                        buttonPosition = towerButton.Location.ToVector2();
                        return true;
                    }
            }

            return false;
        }

        public bool FocusedUpgradeButton()
        {
            foreach (Button button in buttons[gameState])
            {
                if (!button.isHidden && button is UpgradeButton upgradeButton)
                    return upgradeButton.InFocus();
            }
            return false;
        }

        public void GameOver()
        {
            if (gameState == GameState.Playing)
                gameState = GameState.GameOver;
        }
    }
}
