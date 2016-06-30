using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;

public class DisplayCurrentScore : MonoBehaviour
{
    [SerializeField]
    private Text displayCurrentScoreText;

    void Start()
    {
        ScoreManager.ObservableInstance
                    .Where(x => x != null)
                    .Subscribe(x => SubscribeOnScoreManager(x))
                    .AddTo(gameObject);

    }

    void SubscribeOnScoreManager(ScoreManager instance)
    {
        instance.ScoreReactiveProperty
                .Subscribe(x => OnScoreChange(x))
                .AddTo(instance);
    }

    void OnScoreChange(int score)
    {
        displayCurrentScoreText.text = score.ToString();
    }
}
