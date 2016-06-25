using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class HighscoreResource : MonoBehaviour, IHighscoreResource
{
    private List<HighscoreData> scores = null;

    public IList<HighscoreData> GetHighscores()
    {
        if(scores == null)
        {
            InitializeScores();
        }

        return scores;
    }

    private void InitializeScores()
    {
        scores = DefaultTable();
    }

    private HighscoreData GetRankedScore(int rank)
    {
        var nameParam = "aaa";
        var scoreParam = 999;

        return new HighscoreData() { name = nameParam, score = scoreParam };
    }

    private List<HighscoreData> DefaultTable()
    {
        return new List<HighscoreData>
        {
            new HighscoreData() { name = "aaaaa", score = 5000 },
            new HighscoreData() { name = "bbbbb", score = 4000 },
            new HighscoreData() { name = "ccccc", score = 3000 },
            new HighscoreData() { name = "ddddd", score = 2000 },
            new HighscoreData() { name = "eeeee", score = 1000 },
        };
    }
}
