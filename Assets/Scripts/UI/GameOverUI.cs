using UnityEngine;
using System.Collections;
using UniRx;

public class GameOverUI : MonoBehaviour
{
	void Start ()
    {
        GameState.Instance.GameStateReactiveProperty
                          .Where(x => x == GameStateEnum.GameOver)
                          .Subscribe(x => gameObject.SetActive(true))
                          .AddTo(gameObject);

        GameState.Instance.GameStateReactiveProperty
                          .Where(x => x != GameStateEnum.GameOver)
                          .Subscribe(x => gameObject.SetActive(false))
                          .AddTo(gameObject);
    }
}
