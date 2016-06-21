using System.Collections.Generic;
using UniRx;

public interface IObservableScore
{
    IObservable<IList<ScoreData>> ScoreDataObservable { get; }
}
