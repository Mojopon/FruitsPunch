using UnityEngine;
using System.Collections;
using UniRx;

namespace FruitsPunchInGameScripts
{
    public interface IFeverPointProgressObservable
    {
        IObservable<float> FeverPointProgressObservable { get; }
        bool IsOnFever { get; }

        float CalculateRadius(float originalRadius);
    }
}