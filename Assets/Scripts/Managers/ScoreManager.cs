using UnityEngine;
using System.Collections.Generic;
using UniRx;
using System.Collections;

public class ScoreManager : ReactiveSingletonMonoBehaviour<ScoreManager>, IObservableScore
{
    public IObservable<IList<ScoreData>> ScoreDataObservable { get { return _scoreDataStream.AsObservable(); } }
    private BehaviorSubject<IList<ScoreData>> _scoreDataStream = new BehaviorSubject<IList<ScoreData>>(null);

    void Start()
    {
        _scoreDataStream.OnNext(GetScores());
    }

    private IList<ScoreData> GetScores()
    {
        if(scoreResource == null)
        {
            PrepareResource();
        }

        return scoreResource.GetScores();
    }

    private IScoreResource scoreResource;
    void PrepareResource()
    {
        // requires IScoreResource Component
        scoreResource = GetComponent<IScoreResource>();

        // add ScoreResource if its still null
        if (scoreResource == null) scoreResource = gameObject.AddComponent<ScoreResource>();
    }
}
