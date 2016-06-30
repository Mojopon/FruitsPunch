using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;
using FruitsPunchInGameScripts;

public class FeverProgressRepresentingCircle : MonoBehaviour
{
    [SerializeField]
    private Image circle;

    void Start()
    {
        if(!circle)
        {
            Debug.LogError("Cant find Fever Progress Circle image");
            Destroy(this);
            return;
        }

        GameState.Instance
                 .GameStateReactiveProperty
                 .Where(x => x == GameStateEnum.DiscardFruitsPunch || x == GameStateEnum.Title)
                 .Subscribe(x => circle.fillAmount = 0)
                 .AddTo(gameObject);

        FruitsPunchManager.ObservableInstance
                     .Where(x => x != null)
                     .Subscribe(x => SubscribeOnFeverPointProgress(x))
                     .AddTo(gameObject);
    }

    void SubscribeOnFeverPointProgress(FruitsPunchManager instance)
    {
        instance.FeverPointProgressObservable
               .Select(x => Mathf.Clamp(x, 0, 1))
               .Subscribe(x =>
               {
                   circle.fillAmount = x;
               })
               .AddTo(gameObject);
    }
}
