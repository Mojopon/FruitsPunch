using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;
using FruitsPunchInGameScripts;

public class FeverProgressRepresentingCircle : MonoBehaviour
{
    void Start()
    {
        GameState.Instance
                 .GameStateReactiveProperty
                 .Where(x => x == GameStateEnum.BeforeGoTitle || x == GameStateEnum.Title)
                 .Subscribe(x => GetComponent<Image>().fillAmount = 0)
                 .AddTo(gameObject);

        FruitsPunchManager.ObservableInstance
                     .Where(x => x != null)
                     .Subscribe(x => SubscribeOnFeverPointProgress(x))
                     .AddTo(gameObject);
    }

    void SubscribeOnFeverPointProgress(FruitsPunchManager manager)
    {
        var circleImage = GetComponent<Image>();

        manager.FeverPointProgressObservable
               .Select(x => Mathf.Clamp(x, 0, 1))
               .Subscribe(x =>
               {
                   circleImage.fillAmount = x;
               })
               .AddTo(gameObject);
    }
}
