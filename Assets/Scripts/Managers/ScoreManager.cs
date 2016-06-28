using UnityEngine;
using System.Collections.Generic;
using UniRx;
using System.Collections;
using FruitsPunchInGameScripts;
using ScoreManagerComponents;

public class ScoreManager : ObservableSingletonMonoBehaviour<ScoreManager>, IObservableHighscore
{
    [SerializeField]
    private int _ScoreGainPerFruit = 100;

    public IObservable<IHighscores> HighScoreDataObservable { get { return _highscoreDataStream.AsObservable(); } }
    private BehaviorSubject<IHighscores> _highscoreDataStream = new BehaviorSubject<IHighscores>(null);

    public ReadOnlyReactiveProperty<int> ScoreReactiveProperty { get { return _scoreReactiveProperty.ToReadOnlyReactiveProperty(); } }
    private ReactiveProperty<int> _scoreReactiveProperty = new ReactiveProperty<int>(0);

    void Start()
    {
        ResetScore();
        _highscoreDataStream.OnNext(GetHighscoresFromTheResource());

        GameState.Instance.GameStateReactiveProperty
                          .Where(x => x == GameStateEnum.GameOver)
                          .Subscribe(x => SaveHighscore())
                          .AddTo(gameObject);

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

    void SaveHighscore()
    {
        if(_highscoreResource.AddHighscore(HighscoreData.Create(_scoreReactiveProperty.Value, 10)))
        {
            // notify to highscore observers when the new score is added
            _highscoreDataStream.OnNext(GetHighscoresFromTheResource());
        }
    }

    void ResetScore()
    {
        _scoreReactiveProperty.Value = 0;
    }

    private IHighscores GetHighscoresFromTheResource()
    {
        if(_highscoreResource == null)
        {
            PrepareResource();
        }

        return _highscoreResource.GetHighscores();
    }

    private IHighscoreResource _highscoreResource;
    void PrepareResource()
    {
        // requires IScoreResource Component
        _highscoreResource = GetComponent<IHighscoreResource>();

        // add ScoreResource if its still null
        if (_highscoreResource == null) _highscoreResource = gameObject.AddComponent<HighscoreResource>();
    }
}
