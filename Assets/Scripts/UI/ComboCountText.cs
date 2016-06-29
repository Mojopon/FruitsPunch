using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FruitsPunchInGameScripts;
using UniRx;

public class ComboCountText : MonoBehaviour
{
    void Start()
    {
        FruitsPunchManager.ObservableInstance
                          .Where(x => x != null)
                          .Subscribe(x => ObserveOnComboObservable(x))
                          .AddTo(gameObject);
    }

    void ObserveOnComboObservable(FruitsPunchManager instance)
    {
        var observable = instance as IDeleteFruitsObservable;

        observable.ComboReactiveProperty
                  .Subscribe(x => OnComboChange(x))
                  .AddTo(instance);
    }
	
    void OnComboChange(int combo)
    {
        GetComponent<Text>().text = combo.ToString();
    }
}
