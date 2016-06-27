using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


namespace ScoreManagerComponents
{
    public class HighscoreResource : MonoBehaviour, IHighscoreResource
    {
        #region Related to get Highscore
        private IHighscores highscores = null;

        public IHighscores GetHighscores()
        {
            if (highscores == null)
            {
                highscores = new Highscores();
            }

            return highscores;
        }

        #endregion

        #region Related to Add Highscore

        public bool AddHighscore(HighscoreData newScore)
        {
            return highscores.AddHighscore(newScore);
        }

        #endregion
    }
}