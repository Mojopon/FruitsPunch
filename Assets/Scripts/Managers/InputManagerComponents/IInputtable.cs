using UnityEngine;
using System.Collections;
using UniRx;

public interface IInputtable
{
    IObservable<MouseEvent> MouseEventObservable { get; }
}
