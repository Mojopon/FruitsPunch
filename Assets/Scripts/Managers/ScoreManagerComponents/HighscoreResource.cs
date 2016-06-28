using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


namespace ScoreManagerComponents
{
    public class HighscoreResource : MonoBehaviour, IHighscoreResource
    {
        private readonly static string SCORE_PLAYERPREFS_KEY = "Score";
        private string GetScoreKey(int id) { return SCORE_PLAYERPREFS_KEY + id.ToString(); }

        private readonly static string COMBO_PLAYERPREFS_KEY = "Combo";
        private string GetComboKey(int id) { return COMBO_PLAYERPREFS_KEY + id.ToString(); }

        #region Related to get Highscore
        private IHighscores _highscores = null;

        public IHighscores GetHighscores()
        {
            if (_highscores == null)
            {
                _highscores = new Highscores();
                LoadHighscoresFromPlayerPrefs();
            }

            return _highscores;
        }

        void LoadHighscoresFromPlayerPrefs()
        {
            for(int i = 0; i < _highscores.Count; i++)
            {
                LoadHighscoreTable(i);
            }
        }

        void LoadHighscoreTable(int id)
        {
            var score = PlayerPrefs.GetInt(GetScoreKey(id));
            var combo = PlayerPrefs.GetInt(GetComboKey(id));

            _highscores.AddHighscore(HighscoreData.Create(score, combo));
        }

        #endregion

        #region Related to Add Highscore

        // Add new score to Highscores class.
        // then going to save the scores to player prefs
        public bool AddHighscore(HighscoreData newScore)
        {
            if(_highscores.AddHighscore(newScore))
            {
                SaveHighscoresToPlayerPrefs();
                return true;
            }

            return false;
        }

        void SaveHighscoresToPlayerPrefs()
        {
            for(int i = 0; i < _highscores.Count; i++)
            {
                SaveHighscoreTable(i);
            }

            PlayerPrefs.Save();
        }

        void SaveHighscoreTable(int id)
        {
            var highscoreTable = _highscores[id];

            PlayerPrefs.SetInt(GetScoreKey(id), highscoreTable.score);
            PlayerPrefs.SetInt(GetComboKey(id), highscoreTable.combo);
        }

        #endregion
    }
}