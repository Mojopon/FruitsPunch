using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;

public class CountdownText : MonoBehaviour
{
    [SerializeField]
    private Text countdownText;
    void Start()
    {
        countdownText.gameObject.SetActive(false);

        CountdownManager.ObservableInstance
                        .Where(x => x != null)
                        .Subscribe(x => SubscribeOnCountdownManager(x))
                        .AddTo(gameObject);
    }

    void SubscribeOnCountdownManager(CountdownManager countdownManager)
    {
        countdownText.gameObject.SetActive(true);

        countdownManager.CountdownObservable
                        .Subscribe(x => countdownText.text = x.ToString(), () => countdownText.gameObject.SetActive(false))
                        .AddTo(countdownManager.gameObject);
    }
}
