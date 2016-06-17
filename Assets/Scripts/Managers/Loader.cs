using UnityEngine;
using System.Collections;
using UniRx;
using FruitsPunchInGameScripts;

public class Loader : MonoBehaviour
{
    public FruitsPunchManager fruitsManagerPrefab;
    public DeletedFruitsParticle fruitsPunchParticleManagerPrefab;
    public GameTimerManager gameTimerManagerPrefab;
    public CountdownManager countdownManagerPrefab;

    private Transform managers;
    void Start()
    {
        GameState.Instance.GameStateReactiveProperty
                          .Where(x => x == GameStateEnum.BeforeGameStart)
                          .Subscribe(x => InstantiateManagers())
                          .AddTo(gameObject);

        GameState.Instance.GameStateReactiveProperty
                  .Where(x => x == GameStateEnum.GameOver)
                  .Subscribe(x => DestroyManagers())
                  .AddTo(gameObject);
    }

    void InstantiateManagers()
    {
        if(managers == null)
            managers = new GameObject("ManagersHolder").transform;

        Instantiate(fruitsManagerPrefab).transform.SetParent(managers);
        Instantiate(fruitsPunchParticleManagerPrefab).transform.SetParent(managers);
        Instantiate(gameTimerManagerPrefab).transform.SetParent(managers);
        Instantiate(countdownManagerPrefab).transform.SetParent(managers);
    }

    void DestroyManagers()
    {
        if (managers != null)
        {
            Destroy(managers.gameObject);
            managers = null;
        }
    }
}
