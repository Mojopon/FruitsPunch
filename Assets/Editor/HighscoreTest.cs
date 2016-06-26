using UnityEngine;
using System.Collections;
using NUnit.Framework;
using NSubstitute;
using ScoreManagerComponents;

[TestFixture]
public class HighscoreTest
{
    [Test]
    public void ShouldCreateHighscoreFromItsFactory()
    {
        var highscoreData = HighscoreData.Create(5000, 99);

        Assert.AreEqual(5000, highscoreData.score);
        Assert.AreEqual(99,   highscoreData.combo);
    }
}
