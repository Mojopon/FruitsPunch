using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{

    [SerializeField]
    private Text _RankText;
    [SerializeField]
    private Text _ScoreText;
    [SerializeField]
    private Text _ComboText;

    [SerializeField]
    private int _Rank = 0;
    [SerializeField]
    private int _Score = 0;
    [SerializeField]
    private int _Combo = 0;

    public void SetScore(int rank, int score, int combo)
    {
        _Rank = rank;
        _Score = score;
        _Combo = combo;
        UpdateScore();
    }

    public void UpdateScore()
    {
        if (_RankText)
            _RankText.text = _Rank.ToString();

        if (_ScoreText)
            _ScoreText.text = _Score.ToString();
        
        if(_ComboText)
            _ComboText.text = _Combo.ToString();
    }
}
