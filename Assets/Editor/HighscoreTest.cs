using UnityEngine;
using System.Collections;
using NUnit.Framework;
using NSubstitute;
using ScoreManagerComponents;

[TestFixture]
public class HighscoreTest
{
    private Highscores highscores;
    [SetUp]
    public void InitializeTest()
    {
        highscores = new Highscores();
    }

    #region HighscoreData Test

    [Test]
    public void ShouldCreateHighscoreFromItsFactory()
    {
        var highscoreData = HighscoreData.Create();

        // Should be 0 by default
        Assert.AreEqual(0, highscoreData.score);
        Assert.AreEqual(0, highscoreData.combo);

        highscoreData = HighscoreData.Create(5000, 99);

        Assert.AreEqual(5000, highscoreData.score);
        Assert.AreEqual(99,   highscoreData.combo);
    }

    [Test]
    public void CanCompareHighscores()
    {
        var highscoreDataOne = HighscoreData.Create(5000, 99);
        var highscoreDataTwo = HighscoreData.Create(2500, 20);
        var highscoreDataThree = HighscoreData.Create(1000, 120);
        var highscoreDataFour = HighscoreData.Create(5000, 50);

        // parameter should be minus when left hand side is bigger than right hand side
        // so the Highscore list will be sorted by decendant
        Assert.AreEqual(-1, highscoreDataOne.CompareTo(highscoreDataTwo));
        Assert.AreEqual(-1, highscoreDataOne.CompareTo(highscoreDataThree));

        Assert.AreEqual(1, highscoreDataTwo.CompareTo(highscoreDataOne));
        Assert.AreEqual(-1, highscoreDataTwo.CompareTo(highscoreDataThree));

        Assert.AreEqual(1, highscoreDataThree.CompareTo(highscoreDataOne));
        Assert.AreEqual(1, highscoreDataThree.CompareTo(highscoreDataTwo));

        // should be equal when scores are same
        Assert.AreEqual(0, highscoreDataOne.CompareTo(highscoreDataFour));
    }

    #endregion

    #region Highscores Test

    [Test]
    public void ShouldCreateDefaultScoreTable()
    {
        for(int i = 0; i < Highscores.maxHighscoreTables; i++)
        {
            var scoreData = highscores[i];

            // score data should be 0 by default
            Assert.AreEqual(scoreData.score, 0);
            Assert.AreEqual(scoreData.combo, 0);
        }

        Assert.AreEqual(highscores.Count, Highscores.maxHighscoreTables);
    }

    [Test]
    public void ShouldSortHighscoresByScore()
    {
        var highscores = new Highscores();

        var newScoreOne = HighscoreData.Create(5000, 20);
        Assert.IsTrue(highscores.AddHighscore(newScoreOne));
        Assert.AreEqual(newScoreOne, highscores[0]);
        Assert.AreEqual(highscores.Count, Highscores.maxHighscoreTables);


        var newScoreTwo = HighscoreData.Create(2500, 10);
        Assert.IsTrue(highscores.AddHighscore(newScoreTwo));
        Assert.AreEqual(newScoreOne, highscores[0]);
        Assert.AreEqual(newScoreTwo, highscores[1]);
        Assert.AreEqual(highscores.Count, Highscores.maxHighscoreTables);

        var newScoreThree = HighscoreData.Create(10000, 10);
        Assert.IsTrue(highscores.AddHighscore(newScoreThree));
        Assert.AreEqual(newScoreThree, highscores[0]);
        Assert.AreEqual(newScoreOne, highscores[1]);
        Assert.AreEqual(newScoreTwo, highscores[2]);
        Assert.AreEqual(highscores.Count, Highscores.maxHighscoreTables);

        var newScoreFour = HighscoreData.Create(7500, 30);
        Assert.IsTrue(highscores.AddHighscore(newScoreFour));
        Assert.AreEqual(newScoreThree, highscores[0]);
        Assert.AreEqual(newScoreFour, highscores[1]);
        Assert.AreEqual(newScoreOne, highscores[2]);
        Assert.AreEqual(newScoreTwo, highscores[3]);
        Assert.AreEqual(highscores.Count, Highscores.maxHighscoreTables);

        var newScoreFive = HighscoreData.Create(2000, 30);
        Assert.IsTrue(highscores.AddHighscore(newScoreFive));
        Assert.AreEqual(newScoreThree, highscores[0]);
        Assert.AreEqual(newScoreFour, highscores[1]);
        Assert.AreEqual(newScoreOne, highscores[2]);
        Assert.AreEqual(newScoreTwo, highscores[3]);
        Assert.AreEqual(newScoreFive, highscores[4]);
        Assert.AreEqual(highscores.Count, Highscores.maxHighscoreTables);

        var newScoreSix = HighscoreData.Create(1000, 30);
        // return false when the score wasnt be added
        Assert.IsFalse(highscores.AddHighscore(newScoreFive));
        Assert.AreEqual(newScoreThree, highscores[0]);
        Assert.AreEqual(newScoreFour, highscores[1]);
        Assert.AreEqual(newScoreOne, highscores[2]);
        Assert.AreEqual(newScoreTwo, highscores[3]);
        Assert.AreEqual(newScoreFive, highscores[4]);
        Assert.AreEqual(highscores.Count, Highscores.maxHighscoreTables);
    }

    #endregion
}
