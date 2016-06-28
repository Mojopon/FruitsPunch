using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;

public class CountdownManager : ObservableSingletonMonoBehaviour<CountdownManager>
{
    private readonly ISubject<int> _countdownSubject = new Subject<int>();
    public IObservable<int> CountdownObservable { get { return _countdownSubject.AsObservable(); } }

    private readonly ISubject<Unit> _onCountdownCompleteSubject = new Subject<Unit>();
    public IObservable<Unit> OnCountdownCompleteObservable { get { return _onCountdownCompleteSubject.AsObservable(); } }

    void Start()
    {
        StartCoroutine(SequenceStart());
    }

    IEnumerator SequenceStart()
    {
        yield return StartCoroutine(SequenceCountdown());
    }

    IEnumerator SequenceCountdown()
    {
        int countdown = 3;
        _countdownSubject.OnNext(countdown);

        yield return Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
                               .TakeWhile(x => countdown > 1)
                               .Do(x =>
                               {
                                   _countdownSubject.OnNext(--countdown);
                               })
                               .StartAsCoroutine();

        CompleteCountdown();

        yield break;
    }

    void CompleteCountdown()
    {
        _countdownSubject.OnCompleted();
        _onCountdownCompleteSubject.OnNext(Unit.Default);
    }

    void OnDestroy()
    {
        CompleteCountdown();
    }
}
