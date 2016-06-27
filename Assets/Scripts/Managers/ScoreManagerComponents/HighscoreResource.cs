using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


namespace ScoreManagerComponents
{
    public class HighscoreResource : MonoBehaviour, IHighscoreResource
    {
        #region Related to get Highscore
        private IHighscores scores = null;

        public IHighscores GetHighscores()
        {
            if (scores == null)
            {
                scores = new Highscores();
            }

            return scores;
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