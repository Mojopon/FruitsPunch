using System.Collections.Generic;
using UniRx;

namespace ScoreManagerComponents
{
    public interface IObservableHighscore
    {
        IObservable<IHighscores> HighScoreDataObservable { get; }
    }
}
