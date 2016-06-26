using UnityEngine;
using System.Collections.Generic;
using UniRx;
using System.Collections;
using FruitsPunchInGameScripts;

public class ScoreManager : ReactiveSingletonMonoBehaviour<ScoreManager>, IObservableHighscore
{
    [SerializeField]
    private int _ScoreGainPerFruit = 100;

    public IObservable<IList<HighscoreData>> HighScoreDataObservable { get { return _highscoreDataStream.AsObservable(); } }
    private BehaviorSubject<IList<HighscoreData>> _highscoreDataStream = new BehaviorSubject<IList<HighscoreData>>(null);

    public ReadOnlyReactiveProperty<int> ScoreReactiveProperty { get { return _scoreReactiveProperty.ToReadOnlyReactiveProperty(); } }
    private ReactiveProperty<int> _scoreReactiveProperty = new ReactiveProperty<int>(0);

    void Start()
    {
        ResetScore();
        _highscoreDataStream.OnNext(GetHighscores());

        GameState.Instance.GameStateReactiveProperty
                          .Where(x => x == GameStateEnum.DiscardFruitsPunch)
                          .Subscribe(x => ResetScore())
                          .AddTo(gameObject);

        FruitsPunchManager.ObservableInstance
                          .Where(x => x != null)
                          .Subscribe(x => ObserveOnDeletedFruits(x))
                          .AddTo(gameObject);
    }

    void ObserveOnDeletedFruits(FruitsPunchManager instance)
    {
        var observable = instance as IDeletedFruitsObservable;

        observable.DeleteFruitsObservable
                  .Subscribe(x => GainScoreOnFruitsDeleted(x))
                  .AddTo(instance);
    }

    void GainScoreOnFruitsDeleted(Fruits fruits)
    {
        _scoreReactiveProperty.Value += fruits.Count * _ScoreGainPerFruit;
    }

    void ResetScore()
    {
        _scoreReactiveProperty.Value = 0;
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
