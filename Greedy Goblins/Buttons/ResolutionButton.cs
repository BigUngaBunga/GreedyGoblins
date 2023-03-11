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
    class ResolutionButton : OptionButton
    {
        Point[] Resolutions;
        int currentResolution;
        public Point CurrentResolution { get { return Resolutions[currentResolution]; } }
        public ResolutionButton(ButtonType buttonType, Rectangle originalHitbox, Texture2D buttonTexture) : base(buttonType, originalHitbox, buttonTexture)
        {
            Resolutions = new Point[] {new Point(1920, 1080), new Point(1680, 1050), new Point(1600, 900), new Point(1440, 900), new Point(1366, 768), new Point(1280, 1024), new Point(1024, 768)};
            currentResolution = 0;
            SetText();
        }

        //Går till nästa upplösning
        public void NextResolution()
        {
            currentResolution++;
            currentResolution %= Resolutions.Length;
            SetText();
        }

        //Uppdaterar texten på knappen
        protected override void SetText()
        {
            if (Resolutions != null)
            {
                buttonText = $"{Resolutions[currentResolution].X} x {Resolutions[currentResolution].Y}";
                SetTextPosition();
            }   
        }
    }
}
