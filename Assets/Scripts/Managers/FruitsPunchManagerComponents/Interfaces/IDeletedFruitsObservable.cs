using UnityEngine;
using System.Collections;
using UniRx;
using FruitsPunchInGameScripts;

public interface IDeletedFruitsObservable
{
    IObservable<Fruits> DeleteFruitsObservable { get; }
}
