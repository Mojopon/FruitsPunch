using System.Collections.Generic;
using UniRx;

public interface IScoreResource
{
    IList<ScoreData> GetScores();
}
