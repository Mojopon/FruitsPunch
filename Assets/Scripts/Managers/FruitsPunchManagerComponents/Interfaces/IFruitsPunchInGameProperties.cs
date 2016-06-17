using UnityEngine;
using System.Collections;
using UniRx;

namespace FruitsPunchInGameScripts
{
    public interface IFruitsPunchInGameProperties
    {
        IObservable<Fruits> DeleteFruitsObservable { get; }
    }
}
