using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;
using FruitsPunchInGameScripts;

public class WaittimeRepresentingCircle : MonoBehaviour
{
    [SerializeField]
    private Image circle;

    void Start()
    {
        if (!circle)
        {
            Debug.LogError("Cant find Circle Image(WaittimeRepresentingCircle Script)");
            Destroy(this);
            return;
        }

        FruitsPunchManager.ObservableInstance
                          .Where(x => x != null)
                          .Subscribe(x => SubscribeOnFruitsManager(x))
                          .AddTo(gameObject);
    }

    void SubscribeOnFruitsManager(FruitsPunchManager instance)
    {
        instance.WaitTimeProgressObservable
                .Select(x => Mathf.Clamp(x, 0, 1))
                .Subscribe(x => 
                {
                    circle.fillAmount = 1f - x;
                })
                .AddTo(gameObject);
    }
}
