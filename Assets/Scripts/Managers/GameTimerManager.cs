using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UnityEngine.UI;

public class GameTimerManager : ReactiveSingletonMonoBehaviour<GameTimerManager>
{
    [SerializeField]
    private int limitTime = 60;

    private IDisposable _timerDisposable;

    private readonly ReactiveProperty<int> _timerReactiveProperty = new IntReactiveProperty(0);

    public ReadOnlyReactiveProperty<int> GameTime
    {
        get { return _timerReactiveProperty.ToReadOnlyReactiveProperty(); }
    }

    void Start()
    {
        _timerReactiveProperty.Value = limitTime;
        
        GameState.Instance.GameStateReactiveProperty
                          .Where(x => x == GameStateEnum.InGame)
                          .First()
                          .Subscribe(x => TimerStart())
                          .AddTo(gameObject);
       
    }

    void TimerStart()
    {
        _timerDisposable = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
                                     .Subscribe(_ => 
                                     {
                                         _timerReactiveProperty.Value--;
                                     }) 
                                     .AddTo(gameObject);
    }
}