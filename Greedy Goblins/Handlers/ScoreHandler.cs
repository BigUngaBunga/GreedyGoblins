using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Greedy_Goblins
{
    class ScoreHandler
    {
        string fileDirectory;
        public List<int> ScoreList { get; private set; }

        public ScoreHandler()
        {
            fileDirectory = @"Content\Txt\Score.txt";
            ScoreList = new List<int>();
        }


        //Läser in poäng och sorterar lista
        public void ReadScores()
        {
            ScoreList.Clear();
            List<string> streamData = new List<string>();
            StreamReader streamReader = new StreamReader(fileDirectory);
            while (!streamReader.EndOfStream)
                streamData.Add(streamReader.ReadLine());
            streamReader.Close();

            foreach (string line in streamData)
            {
                try
                {
                    ScoreList.Add(Convert.ToInt32(line));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            ScoreList.Sort();
            ScoreList.Reverse();
        }

        //Skriver poäng på fil
        public void SaveScore(int score)
        {
            StreamWriter streamWriter = new StreamWriter(fileDirectory, true);
            streamWriter.WriteLine(score);
            streamWriter.Close();
        }
    }
}
