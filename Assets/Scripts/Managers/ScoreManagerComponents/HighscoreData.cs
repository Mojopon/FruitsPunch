using System;

namespace ScoreManagerComponents
{
    public class HighscoreData : IComparable
    {
        public int score = 0;
        public int combo = 0;

        private HighscoreData() { }

        private HighscoreData(int score, int combo)
        {
            this.score = score;
            this.combo = combo;
        }

        public static HighscoreData Create(int score, int combo)
        {
            return new HighscoreData(score, combo); 
        }

        public static HighscoreData Create()
        {
            return new HighscoreData(0, 0);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            HighscoreData other = obj as HighscoreData;
            if (other != null)
                return -this.score.CompareTo(other.score);
            else
                throw new ArgumentException("Object is not a Highscore Data");
        }
    }
}