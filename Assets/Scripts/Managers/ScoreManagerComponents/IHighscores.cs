using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ScoreManagerComponents
{
    public interface IHighscores : IEnumerable<HighscoreData>, IAddHighscore
    {
        HighscoreData this[int index] { get; }
        int Count { get; }
    }
}
