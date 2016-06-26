namespace ScoreManagerComponents
{
    public class HighscoreData
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
    }
}