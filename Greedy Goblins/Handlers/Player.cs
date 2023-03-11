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
    class Player
    {
        public int health;
        public int Gold { get; private set; }
        int totalGold;
        float scorePerGold, scorePerHealth;
        public int Score { get { return (int)(totalGold * scorePerGold + health * scorePerHealth); } }
        //public Cursor cursor;
        private float goldModifier;

        public Player(Difficulty difficulty)
        {   //Hur mycket liv som spelare börjar med
            switch (difficulty)
            {
                case Difficulty.Easy:
                    health = 100;
                    goldModifier = 1.25f;
                    scorePerHealth = 0.5f;
                    scorePerGold = 0.028f;
                    break;
                case Difficulty.Normal:
                    health = 75;
                    goldModifier = 1;
                    scorePerHealth = 1f;
                    scorePerGold = 0.04f;
                    break;
                case Difficulty.Hard:
                    health = 50;
                    goldModifier = 0.75f;
                    scorePerHealth = 2.5f;
                    scorePerGold = 0.05f;
                    break;

                    
            }
        }

        //Få guld med guldmodifierare
        public void GainGold(int change)
        {
            Gold += (int)(change * goldModifier);
            totalGold += (int)(change * goldModifier);
        }

        public void SpendGold(int change)
        {
            Gold -= change;
        }
    }
}
