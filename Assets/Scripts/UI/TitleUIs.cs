using UnityEngine;
using System.Collections;
using UniRx;
using System.Linq;

public class TitleUIs : MonoBehaviour
{
    public GameObject[] titleUIObjects;

	void Start ()
    {
        GameState.Instance.GameStateReactiveProperty
                          .Where(x => new GameStateEnum[] { GameStateEnum.BeforeGoTitle, GameStateEnum.Title }.Contains(x))
                          .Subscribe(x => SetActiveForTheObjects(true))
                          .AddTo(gameObject);

        GameState.Instance.GameStateReactiveProperty
                          .Where(x => x == GameStateEnum.BeforeGameStart)
                          .Subscribe(x => SetActiveForTheObjects(false))
                          .AddTo(gameObject);
    }

    void SetActiveForTheObjects(bool flag)
    {
        SetActiveForTheObjects(titleUIObjects, flag);
    }

    void SetActiveForTheObjects(GameObject[] objects, bool flag)
    {
        foreach(var obj in objects)
        {
            obj.SetActive(flag);
        }
    }
}
