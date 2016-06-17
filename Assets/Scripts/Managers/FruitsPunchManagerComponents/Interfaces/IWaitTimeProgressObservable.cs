using UnityEngine;
using System.Collections;
using UniRx;

public interface IWaitTimeProgressObservable
{
    IObservable<float> WaitTimeProgressObservable { get; }
}
