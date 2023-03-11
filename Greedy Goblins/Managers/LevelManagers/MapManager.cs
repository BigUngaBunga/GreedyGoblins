using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using CatmullRom;

namespace Greedy_Goblins
{
    class MapManager
    {
        public RenderTarget2D RenderTarget { get; private set; }
        public CatmullRomPath[] Paths { get; private set; }
        List<GameObject> gameObjects;
        SpriteBatch spriteBatch;
        GraphicsDevice graphics;
        Texture2D pathTexture, largeObstacleTexture, smallObstacleTexture, mapTexture;
        LevelType levelType;

        public MapManager(SpriteBatch spriteBatch, LevelType levelType)
        {
            this.levelType = levelType;
            this.spriteBatch = spriteBatch;
            graphics = spriteBatch.GraphicsDevice;
            gameObjects = new List<GameObject>();
            RenderTarget = new RenderTarget2D(spriteBatch.GraphicsDevice, Main.GameScreen.Width, Main.GameScreen.Height);

            LoadLevel();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mapTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Main.ScreenScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(RenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Main.ScreenScale, SpriteEffects.None, 0f);
        }

        //Ritar på rendertarget
        private void DrawOnRenderTarget()
        {   //Förbereder render target
            graphics.SetRenderTarget(RenderTarget);
            graphics.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.ResolutionMatrix);

            //Ritar vägen
            foreach (CatmullRomPath path in Paths)
                path.DrawFill(graphics, pathTexture);

            //Ritar kartobjekt och torn
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject is MapObject mapObject)
                    mapObject.Draw(spriteBatch);

                //Lägg till logik

                else if (gameObject is Tower tower)
                    tower.DrawSize(spriteBatch, true);
            }

            spriteBatch.End();
            graphics.SetRenderTarget(null);
        }


        //Lägger till ett spelobjekt
        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            DrawOnRenderTarget();
        }


        //Tar bort ett spelobjekt
        public void RemoveGameObject(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
            DrawOnRenderTarget();
        }


        //Läser in nivå från fil
        private void LoadLevel()
        {
            SetLevelTextures();
            SetFileDirectories(out string[] pathDirectories, out string mapDirectory);//Sätter filnamn
            Paths = new CatmullRomPath[pathDirectories.Length];
            for (int i = 0; i < Paths.Length; i++)
                Paths[i] = CreatePath(pathDirectories[i]);

            LoadMapObjects(mapDirectory);
            DrawOnRenderTarget();
        }


        //Skapar kartföremål
        private void LoadMapObjects(string fileDirectory)
        {
            //Läser in strängar från fil
            List<string> strings = new List<string>();
            StreamReader streamReader = new StreamReader(fileDirectory);
            while (!streamReader.EndOfStream)
                strings.Add(streamReader.ReadLine());
            streamReader.Close();

            foreach (string line in strings)
            {
                GameObject gameObject = FromFileToMapObject(line);
                if (gameObject != null)
                    gameObjects.Add(gameObject);
            }
        }


        //Kollar efter kollision med rendertarget
        public bool CanPlaceTower(Tower tower)
        {
            Color[] towerPixels = new Color[tower.Hitbox.Width * tower.Hitbox.Height];
            Color[] renderTargetPixels = new Color[(tower.CircleTexture.Width * tower.CircleTexture.Height)];

            try
            {   //Försöker spara data från rendertarget och 
                tower.CircleTexture.GetData(renderTargetPixels);
                RenderTarget.GetData(0, tower.Hitbox, towerPixels, 0, towerPixels.Length);
            }
            catch (Exception)
            {   //Om det uppstår fel returnera fel
                return false;
            }

            //Om alfavärde inte är "rent"
            for (int i = 0; i < towerPixels.Length; i++)
                if (towerPixels[i].A > 0.0f && renderTargetPixels[i].A > 0.0f)
                    return false;

            return true;
        }


        //Väljer textur baserat på number
        private Texture2D PickTexture(int textureNumber)
        {
            switch (textureNumber)
            {
                case 1://Stort föremål
                    return largeObstacleTexture;
                case 2://Litet föremål
                    return smallObstacleTexture;
                default://Småstenar
                    Texture2D[] rockTextures = new Texture2D[] { AssetManager.textures["tiny rock 1"], AssetManager.textures["tiny rock 2"], AssetManager.textures["tiny rock 3"] };
                    return rockTextures[Main.random.Next(0, rockTextures.Length)];
            }
        }


        //Läser in en sträng och returnerar ett spelobjekt
        private MapObject FromFileToMapObject(string line)
        {
            bool lineIsFine = true;
            float scale = 1f, angle = 0f;
            Vector2 position = Vector2.Zero;
            int textureType = 0;

            string ReadLine()//Läser och tar bort en substräng
            {
                string row = line.Substring(0, line.IndexOf(':'));
                line = line.Remove(0, line.IndexOf(':') + 1);

                return row;
            }

            try
            {
                int itemsToRead = 5;
                for (int i = 0; i < itemsToRead; i++)
                {
                    if (i == 0)
                        position.X = (float)Convert.ToDouble(ReadLine());
                    else if (i == 1)
                        position.Y = (float)Convert.ToDouble(ReadLine());
                    else if (i == 2)
                        angle = (float)Convert.ToDouble(ReadLine());
                    else if (i == 3)
                        scale = (float)Convert.ToDouble(ReadLine());
                    else if (i == 4)
                        textureType = Convert.ToInt32(line);
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"The format of {line} is wrong");
                lineIsFine = false;
            }

            if (lineIsFine)
                return new MapObject(position, PickTexture(textureType), scale, angle);

            return null;
        }


        //Bestämmer nivåkomponenter och returnerar ursprungsfil
        private void SetLevelTextures()
        {
            switch (levelType)
            {
                case LevelType.Shrubland://Buskskog
                    mapTexture = AssetManager.textures["shrubland map"];
                    pathTexture = AssetManager.textures["dirt path"];
                    largeObstacleTexture = AssetManager.textures["large shrub"];
                    smallObstacleTexture = AssetManager.textures["small shrub"];
                    break;

                case LevelType.Mountain://Berg
                    mapTexture = AssetManager.textures["mountain map"];
                    pathTexture = AssetManager.textures["dirt path"];
                    largeObstacleTexture = AssetManager.textures["large rock"];
                    smallObstacleTexture = AssetManager.textures["small rock"];
                    break;

                case LevelType.Snow://Snö
                    mapTexture = AssetManager.textures["snow map"];
                    pathTexture = AssetManager.textures["snow path"];
                    largeObstacleTexture = AssetManager.textures["large pinetree"];
                    smallObstacleTexture = AssetManager.textures["small rock"];
                    break;

                default://Skog
                    mapTexture = AssetManager.textures["forest map"];
                    pathTexture = AssetManager.textures["dirt path"];
                    largeObstacleTexture = AssetManager.textures["large tree"];
                    smallObstacleTexture = AssetManager.textures["small tree"];
                    break;
            }
        }


        //Returnerar relevanta filnamn
        private void SetFileDirectories(out string[] pathDirectories, out string mapDirectory)
        {
            switch (levelType)
            {
                case LevelType.Shrubland:
                    pathDirectories = new string[] { @"Content\Txt\Paths\ShrublandPathRight.txt", @"Content\Txt\Paths\ShrublandPathLeft.txt" };
                    mapDirectory = @"Content\Txt\Maps\ShrublandMap.txt";
                    break;
                case LevelType.Mountain:
                    pathDirectories = new string[] { @"Content\Txt\Paths\MountainPathLower.txt", @"Content\Txt\Paths\MountainPathUpper.txt" };
                    mapDirectory = @"Content\Txt\Maps\MountainMap.txt";
                    break;
                case LevelType.Snow:
                    pathDirectories = new string[] { @"Content\Txt\Paths\SnowPath.txt" };
                    mapDirectory = @"Content\Txt\Maps\SnowMap.txt";
                    break;
                default:
                    pathDirectories = new string[] { @"Content\Txt\Paths\ForestPath.txt" };
                    mapDirectory = @"Content\Txt\Maps\ForestMap.txt";
                    break;
            }
        }


        //Läser in vägen
        private CatmullRomPath CreatePath(string fileDirectory)
        {
            CatmullRomPath path = new CatmullRomPath(graphics);
            List<Vector2> points = new List<Vector2>();

            List<string> streamData = new List<string>();
            StreamReader streamReader = new StreamReader(fileDirectory);
            while (!streamReader.EndOfStream)
                streamData.Add(streamReader.ReadLine());
            streamReader.Close();

            foreach (string vectorText in streamData)
            {
                Vector2 position;

                try
                {
                    position.X = (float)Convert.ToDouble(vectorText.Substring(0, vectorText.IndexOf(':')));
                    position.Y = (float)Convert.ToDouble(vectorText.Substring(vectorText.IndexOf(':') + 1));
                    points.Add(position);
                }
                catch (Exception)
                {
                    Console.WriteLine($"The string \"{vectorText}\" is not compatible");
                }
            }

            //Förlänger ändarna
            ExtendPathEnds(ref points);


            foreach (Vector2 point in points)
                path.AddPoint(point);

            //Sätter hur den ritas ut
            uint pathWidth = 40;
            uint pathRepetitions = 8;
            uint pathSubdivisions = 512;
            path.DrawFillSetup(graphics, pathWidth, pathRepetitions, pathSubdivisions);

            return path;
        }


        //Förlänger vägens ändar för att få dem utanför kartan
        private void ExtendPathEnds(ref List<Vector2> points)
        {
            float extension = 50;
            //Första punkten
            Vector2 distance = points[0] - points[1];
            distance.Normalize();
            points[0] += distance * extension;

            //Sista punkten
            int lastPosition = points.IndexOf(points.Last());
            distance = points[lastPosition] - points[lastPosition - 1];
            distance.Normalize();
            points[lastPosition] += distance * extension;
        }
    }
}
