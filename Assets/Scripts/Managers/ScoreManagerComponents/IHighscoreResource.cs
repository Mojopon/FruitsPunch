using System.Collections.Generic;
using UniRx;


namespace ScoreManagerComponents
{
    public interface IHighscoreResource
    {
        IHighscores GetHighscores();
        bool AddHighscore(HighscoreData newScore);
    }
}
