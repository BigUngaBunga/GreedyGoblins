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

namespace Greedy_Goblins
{
    class WaveLoader
    {
        public int RemainingWaves { get { return waves.Count; } }
        string fileDirectory;
        List<Wave> waves;

        public WaveLoader(LevelType levelType)
        {
            switch (levelType)
            {
                case LevelType.Shrubland:
                case LevelType.Mountain:
                    fileDirectory = @"Content\Txt\Waves\DualPathWave.txt";
                    break;
                default:
                    fileDirectory = @"Content\Txt\Waves\SinglePathWave.txt";
                    break;
            }
            
            waves = new List<Wave>();
            ReadWaves();
        }


        //Läser in nivåer
        private void ReadWaves()
        {
            List<string> streamData = new List<string>();
            StreamReader streamReader = new StreamReader(fileDirectory);
            while (!streamReader.EndOfStream)
                streamData.Add(streamReader.ReadLine());
            streamReader.Close();
            //Skippar första raden med komentar
            for (int i = 1; i < streamData.Count; i++)
            {
                Wave wave = CreateWave(streamData[i]);
                if (wave != null)
                    waves.Add(wave);
            }

        }


        //Skapar våg av en sträng
        private Wave CreateWave(string row)
        {
            List<EntityType> enemyTypes = new List<EntityType>();
            float spawnInterval = 500f;
            bool firstPart = true;

            while (row.Length > 0)
            {
                try
                {   //Första raden av
                    if (firstPart)
                    {
                        spawnInterval = (float)Convert.ToDouble(ReadPart(ref row));
                        firstPart = false;
                    }
                    else
                        enemyTypes.Add(PickEntityType(Convert.ToInt32(ReadPart(ref row))));
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return new Wave(enemyTypes.ToArray(), spawnInterval);
        }


        //Läser och tar bort en substräng
        private string ReadPart(ref string line)
        {
            string row;
            if (line.Contains(':'))
            {
                row = line.Substring(0, line.IndexOf(':'));
                line = line.Remove(0, line.IndexOf(':') + 1);
                return row;
            }
            row = line;
            line = line.Remove(0);
            return row;
        }


        //Väljer fiendetyp
        private EntityType PickEntityType(int type)
        {
            switch (type)
            {
                case 0:
                    return EntityType.Goblin;
                case 2:
                    return EntityType.Fighter;
                case 3:
                    return EntityType.Soldier;
                case 4:
                    return EntityType.King;
                default:
                    return EntityType.Miner;
            }
        }


        //Returnerar nästa våg
        public Wave GetNextWave()
        {
            Wave nextWave = waves.First();
            waves.Remove(nextWave);
            return nextWave;
        }
    }
}
