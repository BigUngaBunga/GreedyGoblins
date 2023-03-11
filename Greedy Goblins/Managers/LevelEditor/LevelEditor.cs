using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Spline;
using SaveMap;
using CatmullRom;

namespace Greedy_Goblins
{
    class LevelEditor
    {
        SaveMapForm saveMapForm;
        List<MapPoint> points;
        SimplePath path;
        CatmullRomPath[] catmullRomPaths;
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;
        float doubleClickTimer, doubleClickTime, rotationSpeed, scaleSpeed;
        public static bool openForm;
        bool removeFormatingErrors;
        MapType mapType;
        MapPointType selectedType;

        public LevelEditor(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            graphics = spriteBatch.GraphicsDevice;
            points = new List<MapPoint>();
            renderTarget = new RenderTarget2D(graphics, Main.GameScreen.Width, Main.GameScreen.Height);
            mapType = MapType.Path;
            selectedType = MapPointType.LargeObstacle;
            openForm = false;
            removeFormatingErrors = false;
            doubleClickTime = 300;
            rotationSpeed = 0.05f;
            scaleSpeed = 0.025f;

            path = new SimplePath(graphics);
            path.Clean();
        }


        public void Update(GameTime gameTime)
        {

            Edit();

            if (KeyMouseReader.KeyPressed(Keys.R))
            {
                Zoom();
            }

            if (saveMapForm != null && !saveMapForm.Visible)
            {
                if (mapType != MapType.Path)
                {
                    SelectObjectType();
                    RotateObject();
                    ScaleObject();
                }

                MoveMapPoint();
                AddMapPoint(gameTime);
                RemoveMapPoint();
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (mapType == MapType.Path)
            {
                //Går inte att rita en spline med mindre än 3 punkter
                if (path.AntalPunkter >= 3)
                    path.Draw(spriteBatch);

                for (int i = 0; i < points.Count; i++)
                    points[i].Draw(spriteBatch, i);
            }
            else
            {
                spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Main.ScreenScale, SpriteEffects.None, 0f);//

                foreach (MapPoint point in points)
                    point.Draw(spriteBatch);

                spriteBatch.DrawString(AssetManager.fonts["editor"], $"Currently selected: {selectedType}", Vector2.Zero, Color.Black);
            }
        }


        //Hanterar redigeringsmenyn
        private void Edit()
        {
            //För upp redigerings dialogruta
            if (openForm)
            {
                openForm = false;
                saveMapForm = new SaveMapForm();
                saveMapForm.Show();
            }
            //Hanterar knapptrycken från menyn
            if (saveMapForm != null)
            {
                switch (saveMapForm.editAction)
                {
                    case EditAction.Save:
                        SaveCurrentMap();
                        break;
                    case EditAction.Load:
                        mapType = saveMapForm.MapType;
                        LoadMap();
                        break;
                }
                saveMapForm.editAction = EditAction.Nothing;
            }
        }


        //Sparar ned den aktiva banan till en fil
        private void SaveCurrentMap()
        {   
            //Formaterar vektorer till strängar
            List<string> strings = TurnToStringList(points);
            //Rensar fil och skriver ned alla vektorer
            StreamWriter streamWriter = new StreamWriter(saveMapForm.FileDirectory, false);
            foreach (string point in strings)
                streamWriter.WriteLine(point);
            streamWriter.Close();
        }


        //Läser från fil och skapar bana
        private void LoadMap()
        {   //Läser in strängar från fil
            List<string> strings = new List<string>();
            StreamReader streamReader = new StreamReader(saveMapForm.FileDirectory);
            while (!streamReader.EndOfStream)
                strings.Add(streamReader.ReadLine());
            streamReader.Close();

            if (mapType != MapType.Path)
                LoadPaths();

            //Gör om strängar till vektorer
            points = TurnToMapObjects(strings);

            //Sparar på nytt så att felformateringar försvinner
            if (removeFormatingErrors)
                SaveCurrentMap();

            ResetPath();
        }


        //Returnerar en formaterad stränglista
        private List<string> TurnToStringList(List<MapPoint> points)
        {
            List<string> textPoints = new List<string>();

            switch (mapType)
            {
                case MapType.Path://Returnerar strängslista för stigar
                    for (int i = 0; i < points.Count; i++)
                        textPoints.Add($"{points[i].position.X}:{points[i].position.Y}");
                    return textPoints;

                default://Returnerar stränglista för kartföremål
                    for (int i = 0; i < points.Count; i++)
                        textPoints.Add($"{points[i].position.X}:{points[i].position.Y}:{points[i].angle}:{points[i].Scale}:{(int)points[i].MapPointType}");
                    return textPoints;
            }
            
        }


        //Returnerar en kartobjektslista från en stränglista
        private List<MapPoint> TurnToMapObjects(List<string> strings)
        {
            List<MapPoint> mapPoints = new List<MapPoint>();
            foreach (string row in strings)
            {
                string line = row;
                bool rowIsFine = true;
                float angle = 0f;
                float scale = 1f;
                Vector2 position = Vector2.Zero;
                MapPointType mapPointType = MapPointType.Path;
                Texture2D texture = AssetManager.textures["circle"];

                switch (mapType)
                {
                    case MapType.Path:
                        try
                        {
                            position.X = (float)Convert.ToDouble(row.Substring(0, row.IndexOf(':')));
                            position.Y = (float)Convert.ToDouble(row.Substring(row.IndexOf(':') + 1));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"The format of {row} is wrong");
                            rowIsFine = false;
                            removeFormatingErrors = true;
                        }
                        break;
                    default:
                        try
                        {
                            int itemsToRead = 5;
                            for (int i = 0; i < itemsToRead; i++)
                            {
                                if (i == 0)
                                    position.X = (float)Convert.ToDouble(line.Substring(0, line.IndexOf(':')));
                                else if (i == 1)
                                    position.Y = (float)Convert.ToDouble(line.Substring(0, line.IndexOf(':')));
                                else if (i == 2)
                                    angle = (float)Convert.ToDouble(line.Substring(0, line.IndexOf(':')));
                                else if (i == 3)
                                    scale = (float)Convert.ToDouble(line.Substring(0, line.IndexOf(':')));
                                else if (i == 4)
                                    mapPointType = (MapPointType)Convert.ToInt32(line);

                                if (line.Contains(':'))
                                    line = line.Remove(0, line.IndexOf(':') + 1);
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"The format of {row} is wrong");
                            rowIsFine = false;
                            removeFormatingErrors = true;
                        }
                        break;
                }
                //Försöker göra sträng till vektor 
                

                if (rowIsFine)
                    mapPoints.Add(new MapPoint(position, mapPointType, SetTexture(mapPointType), angle, scale));
            }

            return mapPoints;
        }//SPLIT THIS ONE UP


        //Flyttar på punkter med musen
        private void MoveMapPoint()
        {
            if (KeyMouseReader.LeftMouseDown())
                foreach (MapPoint point in points)
                {   //Flyttar på punkt och spline punkt
                    if (point.CircleCollision(ScaledMousePosition()))
                    {
                        point.position = ScaledMousePosition();

                        if (mapType == MapType.Path)
                            path.SetPos(points.IndexOf(point), ScaledMousePosition());
                        break;
                    }
                }
        }


        //Dubbelklicka för ny punkt
        private void AddMapPoint(GameTime gameTime)
        {
            //Om klicka tidigare och klicka igen, skapa ny punkt
            if (KeyMouseReader.LeftClick() && doubleClickTimer > 0)
            {   //Skapar kartobjekt
                points.Add(new MapPoint(ScaledMousePosition(), selectedType, SetTexture(selectedType), 0f, 1f));

                //Om väg skapas så läggs punkten till stigen
                if (mapType == MapType.Path)
                    path.AddPoint(ScaledMousePosition());
            }
                
            //Om inte klickat tidigare, starta timer
            if (KeyMouseReader.LeftClick() && doubleClickTimer <= 0)
                doubleClickTimer = doubleClickTime;
            //Om timer är större än noll, minska med tid som passerat
            if (doubleClickTimer > 0)
                doubleClickTimer -= gameTime.ElapsedGameTime.Milliseconds;          
        }


        //Högerklicka på punkt för att ta bort
        private void RemoveMapPoint()
        {   //Om högerklick
            if (KeyMouseReader.RightClick())
            {
                int i = points.Count;
                while (i > 0)
                {
                    i--;//Ta bort punkt och splinepunkt
                    if (points[i].CircleCollision(ScaledMousePosition()))
                    {
                        points.RemoveAt(i);
                        if (mapType == MapType.Path)
                            path.RemovePoint(i);

                        break;
                    }
                }
            }
        }


        //Returnerar musens skalade position
        private Vector2 ScaledMousePosition()
        {
            return KeyMouseReader.MousePosition() / Main.ScreenScale;
        }


        //Återställer banan
        private void ResetPath()
        {
            path.Clean();
            foreach (MapPoint point in points)
            {
                path.AddPoint(point.position);
            }
        }


        //Returnerar textur baserat på kart typen och kartföremålstypen
        private Texture2D SetTexture(MapPointType type)
        {
            switch (mapType)//Beror på vilken sorts karta som redigeras
            {
                case MapType.Forest:
                    switch (type)
                    {
                        case MapPointType.LargeObstacle:
                            return AssetManager.textures["large tree"];
                        case MapPointType.SmallObstacle:
                            return AssetManager.textures["small tree"];
                        case MapPointType.TinyObstacle:
                            return AssetManager.textures["tiny rock 1"];
                        default:
                            return AssetManager.textures["dirt path"];
                    }
                case MapType.Mountain:
                    switch (type)
                    {
                        case MapPointType.LargeObstacle:
                            return AssetManager.textures["large rock"];
                        case MapPointType.SmallObstacle:
                            return AssetManager.textures["small rock"];
                        case MapPointType.TinyObstacle:
                            return AssetManager.textures["tiny rock 2"];
                        default:
                            return AssetManager.textures["dirt path"];
                    }
                case MapType.Shrubland:
                    switch (type)
                    {
                        case MapPointType.LargeObstacle:
                            return AssetManager.textures["large shrub"];
                        case MapPointType.SmallObstacle:
                            return AssetManager.textures["small shrub"];
                        case MapPointType.TinyObstacle:
                            return AssetManager.textures["tiny rock 2"];
                        default:
                            return AssetManager.textures["dirt path"];
                    }
                case MapType.Snow:
                    switch (type)
                    {
                        case MapPointType.LargeObstacle:
                            return AssetManager.textures["large pinetree"];
                        case MapPointType.SmallObstacle:
                            return AssetManager.textures["small rock"];
                        case MapPointType.TinyObstacle:
                            return AssetManager.textures["tiny rock 3"];
                        default:
                            return AssetManager.textures["snow path"];
                    }
            }
            return AssetManager.textures["circle"];
        }


        //Välj vilken textur som skall placeras
        private void SelectObjectType()
        {
            if (KeyMouseReader.KeyPressed(Keys.NumPad1))
                selectedType = MapPointType.LargeObstacle;
            if (KeyMouseReader.KeyPressed(Keys.NumPad2))
                selectedType = MapPointType.SmallObstacle;
            if (KeyMouseReader.KeyPressed(Keys.NumPad3))
                selectedType = MapPointType.TinyObstacle;
        }


        //Roterar föremål
        private void RotateObject()
        {
            if (KeyMouseReader.KeyDown(Keys.D))
                foreach (MapPoint point in points)
                {   //Roterar medurs
                    if (point.CircleCollision(ScaledMousePosition()))
                    {
                        point.angle += rotationSpeed;
                        break;
                    }
                        
                }

            else if (KeyMouseReader.KeyDown(Keys.A))
                foreach (MapPoint point in points)
                {   //Roterar moturs
                    if (point.CircleCollision(ScaledMousePosition()))
                    {
                        point.angle -= rotationSpeed;
                        break;
                    }
                        
                }
        }


        //Ändrar föremåls skala
        private void ScaleObject()
        {
            if (KeyMouseReader.KeyDown(Keys.W))
                foreach (MapPoint point in points)
                {   //Gör större
                    if (point.CircleCollision(ScaledMousePosition()))
                    {
                        point.ChangeScale(scaleSpeed);
                        break;
                    }
                }

            else if (KeyMouseReader.KeyDown(Keys.S))
                foreach (MapPoint point in points)
                {   //Gör mindre
                    if (point.CircleCollision(ScaledMousePosition()))
                    {
                        point.ChangeScale(-scaleSpeed);
                        break;
                    }
                }
        }


        //Laddar in vägar
        private void LoadPaths()
        {
            switch (mapType)
            {
                case MapType.Forest:
                    catmullRomPaths = new CatmullRomPath[] { CreatePath(@"Content\Txt\Paths\ForestPath.txt") };
                    break;
                case MapType.Mountain:
                    catmullRomPaths = new CatmullRomPath[] { CreatePath(@"Content\Txt\Paths\MountainPathLower.txt"), CreatePath(@"Content\Txt\Paths\MountainPathUpper.txt") };
                    break;
                case MapType.Shrubland:
                    catmullRomPaths = new CatmullRomPath[] { CreatePath(@"Content\Txt\Paths\ShrublandPathRight.txt"), CreatePath(@"Content\Txt\Paths\ShrublandPathLeft.txt") };
                    break;
                case MapType.Snow:
                    catmullRomPaths = new CatmullRomPath[] { CreatePath(@"Content\Txt\Paths\SnowPath.txt") };
                    break;
            }

            DrawOnRenderTarget(spriteBatch);
        }


        //Läser in väg
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

            foreach (Vector2 point in points)
                path.AddPoint(point);

            //Sätter hur den ritas ut
            uint pathWidth = 40;
            uint pathRepetitions = 8;
            uint pathSubdivisions = 512;
            path.DrawFillSetup(graphics, pathWidth, pathRepetitions, pathSubdivisions);

            return path;
        }


        //Ritar på rendertarget
        private void DrawOnRenderTarget(SpriteBatch spriteBatch)
        {
            graphics.SetRenderTarget(renderTarget);
            graphics.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.ResolutionMatrix);

            foreach (CatmullRomPath path in catmullRomPaths)
                path.DrawFill(graphics, SetTexture(MapPointType.Path));

            spriteBatch.End();
            graphics.SetRenderTarget(null);
        }


        private void Zoom()
        {
            renderTarget = new RenderTarget2D(graphics, Main.GameScreen.Width, Main.GameScreen.Height);
            DrawOnRenderTarget(spriteBatch);
        }
    }
}
