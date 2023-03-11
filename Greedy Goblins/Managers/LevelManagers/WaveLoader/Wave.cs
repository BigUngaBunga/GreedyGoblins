using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greedy_Goblins
{
    class Wave
    {
        public EntityType[] EntityTypes { get; }
        public float SpawnInterval { get; }
        public int EnemiesLeft { get { return EntityTypes.Length - enemiesSpawned; } }
        public int enemiesSpawned;

        public Wave(EntityType[] entityTypes, float spawnInterval)
        {
            enemiesSpawned = 0;
            EntityTypes = entityTypes;
            SpawnInterval = spawnInterval;
        }
    }
}
