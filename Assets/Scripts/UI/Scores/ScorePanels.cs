using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class ScorePanels : MonoBehaviour
{
    [SerializeField]
    private ScorePanel[] scorePanelObjects;

    void Start()
    {
        ScoreManager.ObservableInstance
                    .Where(x => x != null)
                    .Subscribe(x => SubscribeOnScoreManager(x))
                    .AddTo(gameObject);
    }

    private IDisposable subscription = null;
    void SubscribeOnScoreManager(ScoreManager scoreManager)
    {
        if (subscription != null) subscription.Dispose();

        subscription = scoreManager.ScoreDataObservable
                                   .Where(x => x != null)
                                   .Subscribe(x =>
                                   {
                                       foreach (var scoreData in x)
                                       {
                                           Debug.Log(string.Format("{0}, {1}", scoreData.name, scoreData.score));
                                       }
                                   })
                                   .AddTo(gameObject);
    }
}
