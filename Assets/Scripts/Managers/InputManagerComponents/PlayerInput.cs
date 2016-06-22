using UnityEngine;
using System.Collections;
using UniRx;

public enum MouseClickState
{
    Neutral,
    OnClick,
    OnRelease,
    OnDrag,
}

public struct MouseEvent
{
    public MouseClickState state;
    public Vector3 position;
}

public class PlayerInput : SingletonMonoBehaviour<PlayerInput>, IInputtable
{
    public IObservable<MouseEvent> MouseEventObservable { get
        {
            CheckObservables();
            return _mouseEventObservable;
        }
    }

    private IObservable<MouseEvent> _mouseEventObservable = null;

    void CheckObservables()
    {
        bool initialized = true;
        if (_mouseEventObservable == null) initialized = false;

        if (!initialized) InitializeObservables();  
    }

    void InitializeObservables()
    {
        _mouseEventObservable = Observable.EveryUpdate()
                                          .Where(x => Camera.main) // runs when main camera is exists
                                          .Select(x => new MouseEvent()
                                          {
                                             state = GetMouseClickState(),
                                             position = GetMouseWorldPosition()
                                          });


    }

    MouseClickState GetMouseClickState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return MouseClickState.OnClick;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            return MouseClickState.OnRelease;
        }

        if (Input.GetMouseButton(0))
        {
            return MouseClickState.OnDrag;
        }
        else
        {
            return MouseClickState.Neutral;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        var position = Input.mousePosition;
        position.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(position);
    }
}
