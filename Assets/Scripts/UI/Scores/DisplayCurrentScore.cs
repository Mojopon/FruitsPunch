using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;

public class DisplayCurrentScore : MonoBehaviour
{
    void Start()
    {
        ScoreManager.ObservableInstance
                    .Where(x => x != null)
                    .Subscribe(x => SubscribeOnScoreManager(x))
                    .AddTo(gameObject);

    }

    private IDisposable subscription;
    void SubscribeOnScoreManager(ScoreManager instance)
    {
        subscription = instance.ScoreReactiveProperty
                               .Subscribe(x => OnScoreChange(x))
                               .AddTo(gameObject);
    }

    void OnScoreChange(int score)
    {
        GetComponent<Text>().text = score.ToString();
    }
}
