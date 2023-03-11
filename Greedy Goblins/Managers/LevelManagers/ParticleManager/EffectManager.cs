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
    class EffectManager
    {
        List<Effect> effects;

        public EffectManager()
        {
            effects = new List<Effect>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Effect effect in effects)
            {
                effect.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = effects.Count - 1; i >= 0; i--)
            {
                effects[i].Update(gameTime);

                if (effects[i] is BloodEffect blood)
                    if (blood.ParticlesLeft <= 0)
                        effects.RemoveAt(i);

                else if(effects[i] is FlameEffect flame)
                    if (flame.ParticlesLeft <= 0)
                        effects.RemoveAt(i);

                else if (effects[i] is Explosion explosion)
                    if (explosion.finishedExplosion)
                        effects.RemoveAt(i);
            }
        }


        public void AddEffect(Effect effect)
        {
            effects.Add(effect);
        }
    }
}
