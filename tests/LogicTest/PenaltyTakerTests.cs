using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoNil.Logic.Matches;
using TwoNil.Logic.Matches.MatchPlay;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic
{
   [TestClass]
   public class PenaltyTakerTests
   {
      private Mock<IMatchPlayerRandomizer> _randomizer;
      private PenaltyTaker _subject;

      [TestInitialize]
      public void TestInitialize()
      {
         _randomizer = new Mock<IMatchPlayerRandomizer>(MockBehavior.Strict);
         _subject = new PenaltyTaker(_randomizer.Object);
      }

      [TestCleanup]
      public void TestCleanup()
      {
         _randomizer.VerifyAll();
      }

      [TestMethod]
      public void TakePenalties_ChoosesHomeTeamAsWinner_WhenPenaltyShootoutEndsInDraw()
      {
         // Arrange
         var match = CreateMatch();
         _randomizer.Setup(x => x.HomePenaltyScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _randomizer.Setup(x => x.AwayPenaltyScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _randomizer.Setup(x => x.HomeTeamWinsPenalties()).Returns(true);

         // Act
         _subject.TakePenalties(match);

         // Assert
         Assert.IsTrue(match.PenaltiesTaken);
         Assert.IsTrue(match.HomePenaltyScore > match.AwayPenaltyScore);
      }

      [TestMethod]
      public void TakePenalties_ChoosesAwayTeamAsWinner_WhenPenaltyShootoutEndsInDraw()
      {
         // Arrange
         var match = CreateMatch();
         _randomizer.Setup(x => x.HomePenaltyScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _randomizer.Setup(x => x.AwayPenaltyScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _randomizer.Setup(x => x.HomeTeamWinsPenalties()).Returns(false);

         // Act
         _subject.TakePenalties(match);

         // Assert
         Assert.IsTrue(match.PenaltiesTaken);
         Assert.IsTrue(match.HomePenaltyScore < match.AwayPenaltyScore);
      }

      [TestMethod]
      public void TakePenalties_CorrectsPenaltyScore_WhenItsUnrealistic()
      {
         //Arrange
         var penaltyScores = new(
            int HomeScore,
            int AwayScore,
            int ExpectedHomeScore,
            int ExpectedAwayScore)[]
         {
            // A difference of 2 is only possible if the winner scores a maximum of 5 penalties.
            // Otherwise, the score will be corrected.
            (2, 0, 2, 0), // i.e. the score 2-0 stays 2-0
            (3, 1, 3, 1),
            (4, 2, 4, 2),
            (5, 3, 5, 3),
            (6, 4, 6, 5), // i.e. the score 6-4 is corrected to 6-5
            (8, 6, 8, 7),
            (12, 10, 12, 11),
            // A difference of 3 is only possible if the winner scores a maximum of 4 penalties.
            // Otherwise, the score will be corrected.
            (3, 0, 3, 0),
            (4, 1, 4, 1),
            (5, 2, 5, 4),
            (13, 10, 13, 12),
            // A difference of more than 3 is never possible and corrected.
            (5, 0, 5, 4),
            (13, 0, 13, 12),
            // NOTE: A correction means the losing team will score the winners score minus 1.
         };

         foreach (var score in penaltyScores)
         {
            // Arrange (per match)
            var match = CreateMatch();
            _randomizer.Setup(x => x.HomePenaltyScore(It.IsAny<Dictionary<int, float>>())).Returns(score.HomeScore);
            _randomizer.Setup(x => x.AwayPenaltyScore(It.IsAny<Dictionary<int, float>>())).Returns(score.AwayScore);

            // Act
            _subject.TakePenalties(match);

            // Assert
            Assert.AreEqual(score.ExpectedHomeScore, match.HomePenaltyScore, $"{score.HomeScore}-{score.AwayScore} => {score.ExpectedHomeScore}-{score.ExpectedAwayScore}");
            Assert.AreEqual(score.ExpectedAwayScore, match.AwayPenaltyScore, $"{score.HomeScore}-{score.AwayScore} => {score.ExpectedHomeScore}-{score.ExpectedAwayScore}");
         }
      }

      [TestMethod]
      public void Test123()
      {
         var stuff = new List<string>();

         for (int i = 0; i < 1000; i++)
         {
            var match = CreateMatch();
            new PenaltyTaker().TakePenalties(match);
            stuff.Add($"{match.HomePenaltyScore} - {match.AwayPenaltyScore}");
         }

         const string path = @"D:\Temp\penalties.txt";
         File.Delete(path);
         File.WriteAllLines(path, stuff);
      }

      private Shared.DomainObjects.Match CreateMatch()
      {
         return MatchFactory.CreateMatch(new Team(), new Team());
      }
   }
}
