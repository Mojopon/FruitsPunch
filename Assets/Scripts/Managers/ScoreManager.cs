using UnityEngine;
using System.Collections.Generic;
using UniRx;
using System.Collections;

public class ScoreManager : ReactiveSingletonMonoBehaviour<ScoreManager>, IObservableHighscore
{
    public IObservable<IList<HighscoreData>> HighScoreDataObservable { get { return _highscoreDataStream.AsObservable(); } }
    private BehaviorSubject<IList<HighscoreData>> _highscoreDataStream = new BehaviorSubject<IList<HighscoreData>>(null);

    void Start()
    {
        _highscoreDataStream.OnNext(GetHighscores());
    }

    private IList<HighscoreData> GetHighscores()
    {
        if(highscoreResource == null)
        {
            PrepareResource();
        }

        return highscoreResource.GetHighscores();
    }

    private IHighscoreResource highscoreResource;
    void PrepareResource()
    {
        // requires IScoreResource Component
        highscoreResource = GetComponent<IHighscoreResource>();

        // add ScoreResource if its still null
        if (highscoreResource == null) highscoreResource = gameObject.AddComponent<HighscoreResource>();
    }
}
