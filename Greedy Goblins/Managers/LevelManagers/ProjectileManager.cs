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
    

    class ProjectileManager
    {
        public List<Projectile> projectiles;
        EffectManager effectManager;

        public ProjectileManager()
        {
            projectiles = new List<Projectile>();
            effectManager = new EffectManager();
        }

        public void Update(GameTime gameTime, List<Entity> entities)
        {
            foreach (Projectile projectile in projectiles)
            {
                projectile.Update(gameTime);
            }

            CheckForHits(entities);

            RemoveProjectiles();
            effectManager.Update(gameTime);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Projectile projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }

            effectManager.Draw(spriteBatch);
        }


        //Kollar om projektier har träffat
        private void CheckForHits(List<Entity> entities)
        {
            foreach (Projectile projectile in projectiles)
            {
                if (!projectile.hurtEnemies)
                {
                    foreach (Entity entity in entities)
                    {
                        if (entity is Enemy enemy)
                        {
                            if (projectile.CircleCollision(enemy))
                            {
                                projectile.hurtEnemies = true;
                                if (projectile is Quarrel quarrel)
                                {
                                    enemy.TakeDamage(quarrel.Damage);
                                    AssetManager.soundEffects["quarrel hit"].Play(0.5f, 0, 0);
                                    effectManager.AddEffect(new BloodEffect(enemy.Position, quarrel.Direction, quarrel.Damage));
                                    break;
                                }
                                else if (projectile is Cannonball cannonball)
                                {
                                    if (!cannonball.Exploded)
                                        effectManager.AddEffect(new Explosion(enemy.Position, cannonball.ExplosionSize, cannonball.Rotation));

                                    cannonball.Explode();
                                    enemy.TakeDamage(cannonball.Damage);
                                }
                                else if (projectile is Flames flames)
                                {
                                    enemy.TakeDamage(flames.Damage);
                                    effectManager.AddEffect(new FlameEffect(flames.NozzlePosition, flames.Direction, flames.Rotation));
                                }
                                    
                            }
                        }
                    }
                }
            }
        }


        //Tar bort projektiler som gått för långt
        private void RemoveProjectiles()
        {
            for (int i = projectiles.Count -1; i >= 0; i--)
            {
                if (!Main.GameScreen.Contains(projectiles[i].Position))
                {
                    projectiles.RemoveAt(i);
                }

                else if (projectiles[i] is Flames flames && flames.Duration <= 0)
                    projectiles.RemoveAt(i);

                //Om skäkta eller kanonkula träffat, ta bort
                else if (projectiles[i].hurtEnemies)
                {
                    if (projectiles[i] is Quarrel quarrel || projectiles[i] is Cannonball cannonball)
                        projectiles.RemoveAt(i);
                }
            }
        }
    }
}
