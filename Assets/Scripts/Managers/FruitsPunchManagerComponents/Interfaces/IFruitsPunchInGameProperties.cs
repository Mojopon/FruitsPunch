using UnityEngine;
using System.Collections;
using UniRx;

public interface IFruitsPunchInGameProperties
{
    IObservable<Fruits> DeleteFruitsObservable { get; }
}
