using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ScoreManagerComponents
{
    public interface IHighscores : IEnumerable<HighscoreData>
    {
        HighscoreData this[int index] { get; }
        bool AddHighscore(HighscoreData newScore);
    }
}
