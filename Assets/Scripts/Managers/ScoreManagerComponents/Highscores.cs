using System;
using System.Collections;
using System.Collections.Generic;

namespace ScoreManagerComponents
{

    public class Highscores : IHighscores
    {
        public static readonly int maxHighscoreTables = 5;

        private List<HighscoreData> highscoreDatas = new List<HighscoreData>();

        public Highscores()
        {
            CreateDefaultHighscoreTables();
        }

        private void CreateDefaultHighscoreTables()
        {
            for(int i = 0; i < maxHighscoreTables; i++)
            {
                highscoreDatas.Add(HighscoreData.Create());
            }
        }

        public HighscoreData this[int index] { get { return highscoreDatas[index];  } }

        public bool AddHighscore(HighscoreData newScore)
        {
            // never add a score already be added
            if(highscoreDatas.Contains(newScore))
            {
                return false;
            }

            // add the new score first
            highscoreDatas.Add(newScore);

            // Sort it by Highscores(its decendant)
            highscoreDatas.Sort();

            // Remove smallest Highscore from the Highscores
            highscoreDatas.RemoveAt(highscoreDatas.Count - 1);

            return highscoreDatas.Contains(newScore);
        }

        public IEnumerator<HighscoreData> GetEnumerator()
        {
            return highscoreDatas.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return highscoreDatas.GetEnumerator();
        }

        public int Count { get { return highscoreDatas.Count; } }
    }
}
