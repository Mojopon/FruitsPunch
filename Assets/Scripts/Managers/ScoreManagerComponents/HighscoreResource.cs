using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


namespace ScoreManagerComponents
{
    public class HighscoreResource : MonoBehaviour, IHighscoreResource
    {
        #region Related to get Highscore
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

            return HighscoreData.Create(scoreParam, comboParam);
        }

        private List<HighscoreData> DefaultTable()
        {
            return new List<HighscoreData>
            {
                HighscoreData.Create(5000, 10),
                HighscoreData.Create(2500, 8),
                HighscoreData.Create(1000, 6),
                HighscoreData.Create(500, 4),
                HighscoreData.Create(100, 2),
            };
        }

        #endregion

        #region Related to Add Highscore

        public bool AddHighscore(HighscoreData newScore)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}