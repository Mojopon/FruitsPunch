using UnityEngine;
using System.Collections;
using UniRx;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private GameObject gameoverUIObject;

	void Start ()
    {
        GameState.Instance.GameStateReactiveProperty
                          .Where(x => x == GameStateEnum.GameOver)
                          .Subscribe(x => gameoverUIObject.SetActive(true))
                          .AddTo(gameObject);

        GameState.Instance.GameStateReactiveProperty
                          .Where(x => x != GameStateEnum.GameOver)
                          .Subscribe(x => gameoverUIObject.SetActive(false))
                          .AddTo(gameObject);
    }
}
