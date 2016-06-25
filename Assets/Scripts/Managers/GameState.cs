using UnityEngine;
using System.Collections;
using UniRx;

public enum GameStateEnum
{
    Title,
    BeforeGameStart,
    InGame,
    GameOver,
    DiscardFruitsPunch,
    Scoreboard,
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

    public void GoScoreboard()
    {
        StartCoroutine(SequenceGoScoreboard());
    }

    public void GoTitle()
    {
        StartCoroutine(SequenceGoTitle());
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

        GameTimerManager.Instance.TimeUpObservable
                                 .Subscribe(x => StartCoroutine(SequenceGameEnd()))
                                 .AddTo(GameTimerManager.Instance);

        yield break;
    }

    IEnumerator SequenceGameEnd()
    {
        _gameState.Value = GameStateEnum.GameOver;

        yield return new WaitForSeconds(3);

        _gameState.Value = GameStateEnum.DiscardFruitsPunch;

        yield return null;

        _gameState.Value = GameStateEnum.Title;

        yield return null;

        StopAllCoroutines();

        yield break;
    }

    IEnumerator SequenceGoScoreboard()
    {
        _gameState.Value = GameStateEnum.Scoreboard;

        yield break;
    }

    IEnumerator SequenceGoTitle()
    {
        _gameState.Value = GameStateEnum.Title;

        yield break;
    }
}
