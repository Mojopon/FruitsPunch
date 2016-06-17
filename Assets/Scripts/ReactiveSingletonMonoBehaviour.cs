using UnityEngine;
using System.Collections;
using UniRx;

public class ReactiveSingletonMonoBehaviour<T> : SingletonMonoBehaviour<T> where T : MonoBehaviour
{
    public static IObservable<T> ObservableInstance { get { return instanceStream.AsObservable(); } }

    private static ISubject<T> instanceStream = new BehaviorSubject<T>(null);

    protected override void Awake()
    {
        if(CheckInstance())
        {
            instanceStream.OnNext(this as T);
        }
    }
}
