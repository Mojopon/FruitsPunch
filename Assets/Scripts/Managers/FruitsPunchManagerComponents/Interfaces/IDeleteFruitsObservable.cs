using UnityEngine;
using System.Collections;
using UniRx;
using FruitsPunchInGameScripts;

public interface IDeleteFruitsObservable
{
    IObservable<Fruits> DeleteFruitsObservable { get; }
    ReadOnlyReactiveProperty<int> ComboObservable { get; }
}