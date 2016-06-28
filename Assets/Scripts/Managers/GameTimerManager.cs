using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UnityEngine.UI;

public class GameTimerManager : ObservableSingletonMonoBehaviour<GameTimerManager>
{
    [SerializeField]
    private int limitTime = 60;

    private IDisposable _timerDisposable;

    private readonly ReactiveProperty<int> _timerReactiveProperty = new IntReactiveProperty(0);
    public ReadOnlyReactiveProperty<int> GameTime { get { return _timerReactiveProperty.ToReadOnlyReactiveProperty(); } }

    private ISubject<Unit> _timeUpStream = new Subject<Unit>();
    public IObservable<Unit> TimeUpObservable { get { return _timeUpStream.AsObservable(); } }

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
                                     .TakeWhile(x => _timerReactiveProperty.Value > 0)
                                     .Subscribe(x => 
                                     {
                                         _timerReactiveProperty.Value--;
                                     }) 
                                     .AddTo(gameObject);

        GameTime.Where(x => x <= 0)
                .First()
                .Subscribe(x => 
                {
                    _timeUpStream.OnNext(Unit.Default);
                    _timeUpStream.OnCompleted();
                })
                .AddTo(gameObject);
    }
}