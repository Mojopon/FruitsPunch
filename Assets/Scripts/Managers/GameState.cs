using UnityEngine;
using System.Collections;
using UniRx;

public enum GameStateEnum
{
    Title,
    BeforeGameStart,
    InGame,
    GameOver,
    BeforeGoTitle,
}

public class GameState : SingletonMonoBehaviour<GameState>
{
    private ReactiveProperty<GameStateEnum> _gameState = new ReactiveProperty<GameStateEnum>();
    public IReadOnlyReactiveProperty<GameStateEnum> GameStateReactiveProperty { get { return _gameState.ToReadOnlyReactiveProperty(); } }

    void Start()
    {
        _gameState.Value = GameStateEnum.Title;

    }

    public void StartGame()
    {
        StartCoroutine(SequenceGameStart());
    }


    public void EndGame()
    {
        StartCoroutine(SequenceGameEnd());
    }

    IEnumerator SequenceGameStart()
    {
        _gameState.Value = GameStateEnum.BeforeGameStart;

        yield return null;

        yield return CountdownManager.Instance
                                     .OnCountdownCompleteObservable
                                     .First()
                                     .StartAsCoroutine();

        _gameState.Value = GameStateEnum.InGame;

        yield return null;

        yield break;
    }

    IEnumerator SequenceGameEnd()
    {
        _gameState.Value = GameStateEnum.GameOver;

        yield return null;

        _gameState.Value = GameStateEnum.BeforeGoTitle;

        yield return null;

        _gameState.Value = GameStateEnum.Title;

        yield return null;

        yield break;
    }
}
