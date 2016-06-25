using System.Collections.Generic;
using UniRx;

public interface IObservableHighscore
{
    IObservable<IList<HighscoreData>> HighScoreDataObservable { get; }
}
