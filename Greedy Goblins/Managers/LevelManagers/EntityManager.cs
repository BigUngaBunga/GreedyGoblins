using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using CatmullRom;

namespace Greedy_Goblins
{
    class EntityManager
    {

        public List<Entity> Entities { get; private set; }
        public bool WaveOver { get { return IsWaveOver(); } }
        float spawnTimer;
        int waveNumber;
        Wave wave;



        //Entitetsdefinitioner
        float[] entityScale;
        float[] entitySpeed;
        int[] entityValue;
        int[] enemyHealth;
        int[] enemyDamage;


        public EntityManager()
        {
            Entities = new List<Entity>();
            //Startvärden för             Goblin    Miner   Fighter Soldier King
            entityScale = new float[]   { 0.33f,    0.33f,  0.42f,  0.33f,  0.33f};
            entitySpeed = new float[]   { 10,       35,     24,     40,     45 };
            entityValue = new int[]     { 100,      4,      10,     15,     35 };
            enemyDamage = new int[]     { 0,        1,      3,      5,      15 };
            enemyHealth = new int[]     { 0,        40,     125,    330,    1000};
        }

        public void Update(GameTime gameTime, CatmullRomPath[] paths)
        {
                SpawnEntities(gameTime, paths);

                foreach (Entity entity in Entities)
                {
                    entity.Update(gameTime);
                    MoveEntity(entity, paths);
                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in Entities)
            {
                entity.Draw(spriteBatch);
                if (entity is Goblin goblin && waveNumber == 1)
                    goblin.DrawSpeechBubble(spriteBatch);
            }
        }

        
        //Laddar in nästa våg
        public void SetWave(Wave wave)
        {
            this.wave = wave;
            waveNumber++;
        }


        //Hanterar när entiteter skall skapas
        private void SpawnEntities(GameTime gameTime, CatmullRomPath[] paths)
        {
            if (wave.EnemiesLeft > 0)
            {
                spawnTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (spawnTimer >= wave.SpawnInterval)
                {
                    spawnTimer -= wave.SpawnInterval;
                    CreateEntity(paths);
                }
            }
        }


        //Skapar nästa entitet
        private void CreateEntity(CatmullRomPath[] paths)
        {   //new Vector2(0.33f);
            Entity entity;
            int index;
            for (int path = 0; path < paths.Length; path++)
            {
                EntityType entityType = wave.EntityTypes[wave.enemiesSpawned];
                index = (int)entityType;
                Vector2 scale = new Vector2(entityScale[index]);
                float speed = entitySpeed[index];
                int value = entityValue[index];
                int damage = enemyDamage[index];
                int health = enemyHealth[index];


                switch (entityType)
                {
                    case EntityType.Goblin:
                        entity = new Goblin(scale, speed, value, path);
                        break;
                    case EntityType.Fighter:
                        entity = new Fighter(scale, speed, value, damage, health, path);
                        break;
                    case EntityType.Soldier:
                        entity = new Soldier(scale, speed, value, damage, health, path);
                        break;
                    case EntityType.King:
                        entity = new King(scale, speed, value, damage, health, path);
                        break;
                    default:
                        entity = new Miner(scale, speed, value, damage, health, path);
                        break;
                }
                Entities.Add(entity);
            }
            
            wave.enemiesSpawned++;
        }


        //Flyttar entitet längs banan
        private void MoveEntity(Entity entity, CatmullRomPath[] paths)
        {
            entity.Move();

            //Hindrar entitet från att lämna banan
            if (entity.pathPosition >= 1)
                entity.pathPosition = 1;

            Vector2 position = paths[entity.PathNumber].EvaluateAt(entity.pathPosition);
            Vector2 tangent = paths[entity.PathNumber].EvaluateTangentAt(entity.pathPosition);
            float rotation = (float)-Math.Atan2(tangent.X, tangent.Y);
            entity.UpdatePosition(position, rotation);
        }


        //Rensar upp fiender och allierade
        public void Cleanup(out int gold, out int health)
        {
            gold = 0;
            health = 0;

            int i = Entities.Count;
            while (i > 0)
            {
                i--;
                if (Entities[i] is Entity entity)
                {   //Nått slutet av banan
                    if (entity.pathPosition >= 1)
                    {
                        if (entity is Enemy enemy)
                            health -= enemy.Damage;
                        else if (entity is Ally ally)
                            gold += ally.Value;

                        Entities.RemoveAt(i);
                        break;
                    }//Blir besegrad
                    else if (entity is Enemy enemy)
                        if (enemy.Health <= 0)
                        {
                            gold += enemy.Value;
                            Entities.RemoveAt(i);
                            break;
                        }
                }
            }
        }


        //Kollar om vågen är avklarad
        private bool IsWaveOver()
        {
            if (wave == null)
                return true;
            else if (Entities.Count > 0 || wave.EnemiesLeft > 0)
                return false;

            return true;
        }
    }
}
