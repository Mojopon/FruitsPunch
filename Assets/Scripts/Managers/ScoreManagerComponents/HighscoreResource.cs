using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


namespace ScoreManagerComponents
{
    public class HighscoreResource : MonoBehaviour, IHighscoreResource
    {
        private List<HighscoreData> scores = null;

        public IList<HighscoreData> GetHighscores()
        {
            if (scores == null)
            {
                InitializeScores();
            }

            return scores;
        }

        private void InitializeScores()
        {
            scores = DefaultTable();
        }

        private HighscoreData GetRankedScore(int rank)
        {
            var comboParam = 10;
            var scoreParam = 999;

            return new HighscoreData() { combo = comboParam, score = scoreParam };
        }

        private List<HighscoreData> DefaultTable()
        {
            return new List<HighscoreData>
        {
            new HighscoreData() { combo = 10, score = 5000 },
            new HighscoreData() { combo = 8, score = 4000 },
            new HighscoreData() { combo = 6, score = 3000 },
            new HighscoreData() { combo = 4, score = 2000 },
            new HighscoreData() { combo = 2, score = 1000 },
        };
        }
    }
}