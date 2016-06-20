using UnityEngine;
using System.Collections;
using UniRx;

namespace FruitsPunchInGameScripts
{
    public interface IFruitsPunchInGameProperties
    {
        IObservable<Fruits> DeleteFruitsObservable { get; }

        float   FruitsDeleteRadius     { get; }
        Vector3 FruitsSpawnPoint       { get; }
        float   GapTimeBetweenDelete   { get; }
        float   TimeToFinishFever      { get; }
        float   GainFeverForEachDelete { get; }
    }
}
