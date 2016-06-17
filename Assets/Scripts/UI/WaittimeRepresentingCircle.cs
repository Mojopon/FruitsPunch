using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;

public class WaittimeRepresentingCircle : MonoBehaviour
{
    void Start()
    {
        FruitsPunchManager.ObservableInstance
                     .Where(x => x != null)
                     .Subscribe(x => SubscribeOnFruitsManager(x))
                     .AddTo(gameObject);
    }

    void SubscribeOnFruitsManager(FruitsPunchManager instance)
    {
        var circleImage = GetComponent<Image>();

        instance.WaitTimeProgressObservable
                .Select(x => Mathf.Clamp(x, 0, 1))
                .Subscribe(x => circleImage.fillAmount = 1f - x)
                .AddTo(gameObject);
    }
}
