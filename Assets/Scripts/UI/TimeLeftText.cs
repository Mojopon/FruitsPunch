using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;

public class TimeLeftText : MonoBehaviour
{
    void Start()
    {
        GameTimerManager.ObservableInstance
                        .Where(x => x != null)
                        .Subscribe(x => SubscribeOnTimerManager(x))
                        .AddTo(gameObject);
    }

    void SubscribeOnTimerManager(GameTimerManager gameTimerManager)
    {
        gameTimerManager.GameTime
                        .Subscribe(x => GetComponent<Text>().text = x.ToString())
                        .AddTo(gameTimerManager.gameObject);
    }
}
