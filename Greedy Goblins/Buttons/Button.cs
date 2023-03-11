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


    abstract class Button
    {
        protected Rectangle originalHitbox, scaledHitbox;
        protected Texture2D buttonTexture;
        protected Color colour, neutralColour, focusedColour;
        public Point Location { get { return originalHitbox.Location; } }
        public bool isHidden;

        public Button(Rectangle originalHitbox, Texture2D buttonTexture)
        {
            this.originalHitbox = originalHitbox;
            this.buttonTexture = buttonTexture;
            RescaleHitbox();

            //Färger
            focusedColour = Color.White;
            colour = neutralColour = new Color(220, 220, 220);
        }

        //Ändrar färg om mus är ovanpå
        public virtual void Update()
        {
            if (InFocus())
                colour = focusedColour;
            else
                colour = neutralColour;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(buttonTexture, originalHitbox, colour);
        }

        //Kollar om användare har mus över knapp (1 overload)
        public bool InFocus()
        {
            return scaledHitbox.Contains(KeyMouseReader.mouseState.Position);
        }
        public bool InFocus(Point position)
        {
            return scaledHitbox.Contains(position);
        }

        //Skalar om "kollisions" rutan
        public void RescaleHitbox()
        {
            Vector2 position;//Skalar position
            position.X = originalHitbox.X * Main.ScreenScale.X;
            position.Y = originalHitbox.Y * Main.ScreenScale.Y;

            Vector2 size;//Skalar storlek
            size.X = originalHitbox.Width * Main.ScreenScale.X;
            size.Y = originalHitbox.Height * Main.ScreenScale.Y;

            scaledHitbox = new Rectangle(position.ToPoint(), size.ToPoint());
        }
    }
}
