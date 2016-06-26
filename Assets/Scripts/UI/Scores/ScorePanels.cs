using UnityEngine;
using System.Collections;
using UniRx;
using System;
using System.Collections.Generic;

public class ScorePanels : MonoBehaviour
{
    // it should has 5 scorepanels
    private int scorePanelsNumber = 5;

    [SerializeField]
    private GameObject[] onScoreboardObjects;

    [SerializeField]
    private ScorePanel[] scorePanelObjects;

    void Start()
    {
        SetActiveForScoreboardObjects(false);

        GameState.Instance.GameStateReactiveProperty
                          .Where(x => x == GameStateEnum.Scoreboard)
                          .Subscribe(x => SetActiveForScoreboardObjects(true))
                          .AddTo(gameObject);

        GameState.Instance.GameStateReactiveProperty
                          .Where(x => isActive && x != GameStateEnum.Scoreboard)
                          .Subscribe(x => SetActiveForScoreboardObjects(false))
                          .AddTo(gameObject);

        ScoreManager.ObservableInstance
                    .Where(x => x != null)
                    .Subscribe(x => SubscribeOnScoreManager(x))
                    .AddTo(gameObject);
    }

    private bool isActive = false;
    void SetActiveForScoreboardObjects(bool flag)
    {
        switch(flag)
        {
            case true:
                {
                    foreach(var obj in onScoreboardObjects)
                    {
                        obj.SetActive(false);
                        obj.SetActive(true);
                    }
                }
                break;
            case false:
                {
                    foreach (var obj in onScoreboardObjects)
                    {
                        obj.SetActive(true);
                        obj.SetActive(false);
                    }
                }
                break;
        }

        isActive = flag;
    }

    private IDisposable subscription = null;
    void SubscribeOnScoreManager(ScoreManager scoreManager)
    {
        if (subscription != null) subscription.Dispose();

        subscription = scoreManager.HighScoreDataObservable
                                   .Where(x => x != null)
                                   .Subscribe(x => SetScores(x))
                                   .AddTo(gameObject);
    }

    void SetScores(IList<HighscoreData> scoreDatas)
    {
        if(scorePanelsNumber > scoreDatas.Count)
        {
            // it should has more scorepanels than the count given from the score datas
            Debug.LogError("something weird is happenned in ScorePanels");
        }

        for(int i = 0; i < scorePanelsNumber; i++)
        {
            SetScore(i, scoreDatas[i]);
        }
    }

    void SetScore(int id, HighscoreData scoreData)
    {
        var scorePanel = scorePanelObjects[id];

        scorePanel.SetScore(id + 1, scoreData.score, scoreData.combo);
    }
}
