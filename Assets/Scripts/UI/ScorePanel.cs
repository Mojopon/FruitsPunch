using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [SerializeField]
    private int _Rank;
    [SerializeField]
    private Text _ScoreText;
    [SerializeField]
    private Text _PlayerNameText;

    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private string _playerName = "";

    public void SetScore(int rank, int score, string playerName)
    {
        _Rank = rank;
        _score = score;
        _playerName = playerName;
        UpdateScore();
    }

    public void UpdateScore()
    {
        if(_ScoreText)
            _ScoreText.text = FormattedScore;
        
        if(_PlayerNameText)
            _PlayerNameText.text = _playerName;
    }

    private string FormattedScore { get { return _Rank.ToString() + ". " + _score.ToString(); } }
}
