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
    enum MapPointType
    {
        Path, LargeObstacle, SmallObstacle, TinyObstacle
    }

    class MapPoint
    {
        public Vector2 position;
        public MapPointType MapPointType { get; private set; }
        public float angle;
        public float Scale { get; private set; }
        Texture2D texture;
        Vector2 centre, numberCentre;
        float radius;
        
        

        public MapPoint(Vector2 position, MapPointType mapPointType, Texture2D texture, float angle, float scale)
        {
            this.position = position;
            this.texture = texture;
            this.angle = angle;
            Scale = scale;
            MapPointType = mapPointType;
            centre = new Vector2(texture.Width / 2, texture.Height / 2);
            numberCentre = centre * new Vector2(0.6f, 1.2f) * 0.6f;
            radius = texture.Width * scale / 2;
            angle = 0f;
        }

        //Draw med en overload
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, angle, centre, Scale, SpriteEffects.None, 0f);
        }
        public void Draw(SpriteBatch spriteBatch, int i)
        {   //Ritar positionen och vilken plats den har i listan
            spriteBatch.Draw(texture, position, null, Color.Red, 0f, centre, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(AssetManager.fonts["editor"], $"{i}", position, Color.Black, 0f, numberCentre, 1f, SpriteEffects.None, 0f);
        }


        //Kollar om punkt är innanför cirkelns radie
        public bool CircleCollision(Vector2 point)
        {
            Vector2 distance = position - point;
            return distance.Length() <= radius;
        }


        //Ändrar storlek
        public void ChangeScale(float change)
        {
            Scale += change;
            radius = texture.Width * Scale / 2;
        }
    }
}
