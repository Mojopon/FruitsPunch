using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;

public class TimeLeftText : MonoBehaviour
{
    [SerializeField]
    private Text timeleftText;

    void Start()
    {
        GameTimerManager.ObservableInstance
                        .Where(x => x != null)
                        .Subscribe(x => SubscribeOnTimerManager(x))
                        .AddTo(gameObject);
    }

    void SubscribeOnTimerManager(GameTimerManager instance)
    {
        instance.GameTime
                .Subscribe(x => timeleftText.text = x.ToString())
                .AddTo(instance);
    }
}
