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
    class TowerManager
    {
        public List<Tower> Towers { get; private set; }
        public bool AddedATower { get; private set; }
        public bool placingTower;
        bool selectedATower;
        ButtonManager buttonManager;
        TowerInformation towerInformation;

        //Torn definitioner
        float[] towerScales, towerRanges, towerSizes;
        int[] towerFireRates, towerDamages, towerCosts;


        public TowerManager(ButtonManager buttonManager)
        {
            this.buttonManager = buttonManager;
            towerInformation = new TowerInformation();
            Towers = new List<Tower>();
            LoadTowerVariables();
        }


        public void Update(GameTime gameTime)
        {
            if (KeyMouseReader.KeyPressed(Keys.U))
            {
                Console.WriteLine(placingTower);
            }

            if (KeyMouseReader.RightClick())
                RemoveUnPlacedTowers();

            AddTower();
            SelectTower();
            foreach (Tower tower in Towers)
            {
                tower.Update(gameTime);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tower tower in Towers)
            {
                tower.Draw(spriteBatch);
            }
        }


        //Laddar in tornens standardvariabler
        private void LoadTowerVariables()
        {
                                    //Crossbowman, Cannon, Flamethrower
            towerScales = new float[] {     0.35f,   0.4f,   0.4f};
            towerRanges = new float[] {     550,    420,    280};
            towerSizes = new float[] {      50,     75,     60};
            towerFireRates = new int[] {    500,    1200,    150};
            towerDamages = new int[] {      5,      10,      3};
            towerCosts = new int[] {        75,    150,    225};
        }


        //Varje torn
        public void FireProjectiles(List<Projectile> projectiles, List<Entity> entities)
        {
            foreach (Tower tower in Towers)
            {
                if (tower.isPlaced)
                {
                    Enemy target = null;
                    foreach (Entity entity in entities)
                        if (entity is Enemy enemy)
                            if (tower.IsInRange(enemy))
                            {
                                if (target == null)
                                    target = enemy;
                                else if (target.pathPosition < enemy.pathPosition)
                                    target = enemy;
                            }

                    if (target != null)
                    {   //Vänder sig mot fienden
                        tower.RotateTowardsTarget(target.Position);
                        //Skjuter mot fienden
                        if (tower.Cooldown <= 0)
                            projectiles.Add(tower.Fire(target.Position));
                    }
                        
                }
            }
        }


        //Lägger till torn
        public void AddTower()
        {
            AddedATower = false;
            if (buttonManager.addATower)
            {
                AddedATower = true;
                buttonManager.addATower = false;
                RemoveUnPlacedTowers();
                placingTower = true;

                int index;
                //Lägger till nytt torn
                switch (buttonManager.TowerTypeToAdd)
                {
                    case TowerType.Cannon:
                        index = 1;
                        Towers.Add(new Cannon(new Vector2(towerScales[index]), towerRanges[index], towerSizes[index], towerDamages[index], towerFireRates[index], towerCosts[index]));
                        break;
                    case TowerType.FlameThrower:
                        index = 2;
                        Towers.Add(new Flamethrower(new Vector2(towerScales[index]), towerRanges[index], towerSizes[index], towerDamages[index], towerFireRates[index], towerCosts[index]));
                        break;
                    default://Armborst
                        index = 0;
                        Towers.Add(new Crossbowman(new Vector2(towerScales[index]), towerRanges[index], towerSizes[index], towerDamages[index], towerFireRates[index], towerCosts[index]));
                        break;
                }
            }
        }


        //Väljer ett torn
        private void SelectTower()
        {
            if (!buttonManager.FocusedUpgradeButton())
            {
                if (KeyMouseReader.LeftClick())
                {
                    selectedATower = false;
                    bool unselect = false;
                    foreach (Tower tower in Towers)
                    {
                        if (unselect || placingTower)
                            tower.isSelected = false;
                        else
                        {
                            tower.isSelected = tower.IsInFocus();
                            selectedATower = tower.isSelected;
                            unselect = tower.isSelected;
                        }
                    }
                }
            }
        }


        //Tar bort icke placerade torn
        private void RemoveUnPlacedTowers()
        {
            for (int i = Towers.Count - 1; i >= 0; i--)
                if (!Towers[i].isPlaced)
                    Towers.RemoveAt(i);
            placingTower = false;
        }


        //Ritar information om nya torn
        public void ShowTowerInfo(SpriteBatch spriteBatch)
        {
            if (buttonManager.FocusedTowerButton(out TowerType towerType, out Vector2 buttonPosition))
            {
                int index = (int)towerType;
                towerInformation.DrawGeneralTowerInformation(spriteBatch, buttonPosition, towerCosts[index], towerFireRates[index], towerDamages[index]);
            }

            if (selectedATower)
            {
                foreach (Tower tower in Towers)
                {
                    if (tower.isSelected && tower.isPlaced)
                    {
                        towerInformation.DrawSpecificTowerInformation(spriteBatch, tower, buttonManager.FocusedUpgradeButton());
                        buttonManager.DrawUpgradeButton(spriteBatch);
                    }
                }
            }
        }


        //Uppgraderar det valda tornet
        public int UpgradeTower(int gold)
        {
            if (selectedATower)
            {
                foreach (Tower tower in Towers)
                {
                    if (tower.isSelected && tower.UpgradeCost <= gold)
                    {
                        int cost = tower.UpgradeCost;
                        tower.UpgradeTower();
                        return cost;
                    }
                }
            }

            return 0;
        }
    }
}
