using UnityEngine;
using System.Collections;

namespace ScoreManagerComponents
{
    public interface IAddHighscore
    {
        bool AddHighscore(HighscoreData newScore);
    }
}
