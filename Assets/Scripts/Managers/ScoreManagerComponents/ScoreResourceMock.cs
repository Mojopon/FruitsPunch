using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreResourceMock : MonoBehaviour, IScoreResource
{
    private List<ScoreData> scores = null;

    public IList<ScoreData> GetScores()
    {
        if (scores == null)
        {
            InitializeScores();
        }

        return scores;
    }

    private void InitializeScores()
    {
        scores = DefaultTable();
    }

    private ScoreData GetRankedScore(int rank)
    {
        var nameParam = "aaa";
        var scoreParam = 999;

        return new ScoreData() { name = nameParam, score = scoreParam };
    }

    private List<ScoreData> DefaultTable()
    {
        return new List<ScoreData>
        {
            new ScoreData() { name = "aaaaa", score = 5000 },
            new ScoreData() { name = "bbbbb", score = 4000 },
            new ScoreData() { name = "ccccc", score = 3000 },
            new ScoreData() { name = "ddddd", score = 2000 },
            new ScoreData() { name = "eeeee", score = 1000 },
        };
    }
}
