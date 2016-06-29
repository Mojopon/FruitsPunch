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

    private int _maxCombo;
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


    void ResetScore()
    {
        _scoreReactiveProperty.Value = 0;
        _maxCombo = 0;
    }

    void ObserveOnDeletedFruits(FruitsPunchManager instance)
    {
        var observable = instance as IDeleteFruitsObservable;

        var deletedCountStream = observable.DeleteFruitsObservable
                                           .Take(1)
                                           .Select(x => x.Count);

        var comboCountStream = observable.ComboReactiveProperty
                                         .Skip(1)
                                         .Take(1);

        Observable.WhenAll(deletedCountStream, comboCountStream)
                  .Do(x => OnScoreGain(x[0], x[1]))
                  .Subscribe(x => ObserveOnDeletedFruits(instance))
                  .AddTo(instance);

        /*
        observable.DeleteFruitsObservable
                  .Subscribe(x => GainScoreOnFruitsDeleted(x))
                  .AddTo(instance);
        */
    }

    void OnScoreGain(int number, int combo)
    {
        _scoreReactiveProperty.Value += (number * _ScoreGainPerFruit) + combo * 10;

        if (combo > _maxCombo) _maxCombo = combo;
    }

    /*
    void GainScoreOnFruitsDeleted(Fruits fruits)
    {
        _scoreReactiveProperty.Value += fruits.Count * _ScoreGainPerFruit;
    }
    */

    void SaveHighscore()
    {
        if(_highscoreResource.AddHighscore(HighscoreData.Create(_scoreReactiveProperty.Value, _maxCombo)))
        {
            // notify to highscore observers when the new score is added
            _highscoreDataStream.OnNext(GetHighscoresFromTheResource());
        }
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
