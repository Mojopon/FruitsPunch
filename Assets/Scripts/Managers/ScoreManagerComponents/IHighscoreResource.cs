using System.Collections.Generic;
using UniRx;


namespace ScoreManagerComponents
{
    public interface IHighscoreResource : IAddHighscore
    {
        IHighscores GetHighscores();
    }
}
