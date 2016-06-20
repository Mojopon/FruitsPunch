using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class InputManager : SingletonMonoBehaviour<InputManager>, IInputtable
{
    public IObservable<MouseEvent> MouseEventObservable
    {
        get { return PlayerInput.Instance.MouseEventObservable; }
    }
}
