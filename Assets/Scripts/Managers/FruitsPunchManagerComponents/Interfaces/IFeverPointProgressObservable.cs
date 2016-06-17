using UnityEngine;
using System.Collections;
using UniRx;

public interface IFeverPointProgressObservable
{
    IObservable<float> FeverPointProgressObservable { get; }
}
