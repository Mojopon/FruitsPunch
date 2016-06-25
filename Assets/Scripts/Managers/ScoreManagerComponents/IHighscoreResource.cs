using System.Collections.Generic;
using UniRx;

public interface IHighscoreResource
{
    IList<HighscoreData> GetHighscores();
}
