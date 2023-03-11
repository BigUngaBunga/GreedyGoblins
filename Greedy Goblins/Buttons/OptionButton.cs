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
    enum ButtonType
    {
        Play, Back, Menu, Options, Scoreboard, SaveScore, Exit, Easy, Normal, Hard, Sound, Music, SetResolution, Resolution, EditMap, FormEdit, Info, Fullscreen, Credits
    }
    class OptionButton : TextButton
    {
        public ButtonType ButtonType { get; }

        public OptionButton(ButtonType buttonType, Rectangle originalHitbox, Texture2D buttonTexture) : base(originalHitbox, buttonTexture)
        {
            ButtonType = buttonType;
            RescaleHitbox();
            //Text
            SetText();
        }


        //Sätter texten på knappen
        protected virtual void SetText()
        {
            switch (ButtonType)
            {
                case ButtonType.Scoreboard:
                    buttonText = "Score";
                    break;

                case ButtonType.SetResolution:
                    buttonText = "Update";
                    break;

                case ButtonType.SaveScore:
                    buttonText = "Save Score";
                    break;

                case ButtonType.EditMap:
                    buttonText = "Edit Map";
                    break;

                case ButtonType.FormEdit:
                    buttonText = "Edit";
                    break;

                default:
                    buttonText = ButtonType.ToString();
                    break;
            }

            SetTextPosition();
        }
    }
}
