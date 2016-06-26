using System;
using System.Collections;
using System.Collections.Generic;

namespace ScoreManagerComponents
{

    public class Highscores : IHighscores
    {
        private readonly int maxHighscores = 5;

        private List<HighscoreData> highscoreDatas = new List<HighscoreData>();

        public Highscores()
        {

        }

        public HighscoreData this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool AddHighscore(HighscoreData newScore)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<HighscoreData> GetEnumerator()
        {
            return highscoreDatas.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return highscoreDatas.GetEnumerator();
        }
    }
}
