using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;

public class CountdownText : MonoBehaviour
{
    private Text countdownText;
    void Start()
    {
        countdownText = GetComponent<Text>();
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
                        .Subscribe(x => GetComponent<Text>().text = x.ToString(), () => countdownText.gameObject.SetActive(false))
                        .AddTo(countdownManager.gameObject);
    }
}
