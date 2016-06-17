using UnityEngine;
using System.Collections;
using UniRx;

namespace FruitsPunchInGameScripts
{
    public interface IWaitTimeProgressObservable
    {
        IObservable<float> WaitTimeProgressObservable { get; }
    }
}
