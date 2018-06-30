using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoNil.Logic.Functionality.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic
{
   [TestClass]
   public class MatchPlayerTests
   {
      private Mock<IMatchPlayerRandomizer> _randomizer;
      private Mock<IPenaltyTaker> _penaltyTaker;

      [TestInitialize]
      public void TestIntialize()
      {
         _randomizer = new Mock<IMatchPlayerRandomizer>(MockBehavior.Strict);
         _penaltyTaker = new Mock<IPenaltyTaker>(MockBehavior.Strict);
      }

      [TestCleanup]
      public void TestCleanup()
      {
         _randomizer.VerifyAll();
      }

      [TestMethod]
      public void Play_HomeAndAwayScoreAreDetermined()
      {
         _randomizer.Setup(x => x.HomeTeamWins(It.IsAny<Dictionary<bool, float>>())).Returns(true);
         _randomizer.Setup(x => x.HomeScore(It.IsAny<Dictionary<int, float>>())).Returns(7);
         _randomizer.Setup(x => x.AwayScore(It.IsAny<Dictionary<int, float>>())).Returns(3);

         var match = Helper.GetValidMatch();

         var matchPlayer = new MatchPlayer(_randomizer.Object, _penaltyTaker.Object);
         matchPlayer.Play(match);

         Assert.AreEqual(7, match.HomeScore);
         Assert.AreEqual(3, match.AwayScore);
         Assert.AreEqual(MatchStatus.Ended, match.MatchStatus);
      }

      [TestMethod]
      public void Play_HomeTeamWins_WhenAwayScoreIsBiggerButHomeTeamWins()
      {
         _randomizer.Setup(x => x.HomeTeamWins(It.IsAny<Dictionary<bool, float>>())).Returns(true);
         _randomizer.Setup(x => x.HomeScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _randomizer.Setup(x => x.AwayScore(It.IsAny<Dictionary<int, float>>())).Returns(1);

         var match = Helper.GetValidMatch();

         var matchPlayer = new MatchPlayer(_randomizer.Object, _penaltyTaker.Object);
         matchPlayer.Play(match);

         Assert.AreEqual(1, match.HomeScore);
         Assert.AreEqual(0, match.AwayScore);
         Assert.AreEqual(MatchStatus.Ended, match.MatchStatus);
      }

      [TestMethod]
      public void Play_AwayTeamWins_WhenHomeScoreIsBiggerButAwayTeamWins()
      {
         _randomizer.Setup(x => x.HomeTeamWins(It.IsAny<Dictionary<bool, float>>())).Returns(false);
         _randomizer.Setup(x => x.HomeScore(It.IsAny<Dictionary<int, float>>())).Returns(1);
         _randomizer.Setup(x => x.AwayScore(It.IsAny<Dictionary<int, float>>())).Returns(0);

         var match = Helper.GetValidMatch();

         var matchPlayer = new MatchPlayer(_randomizer.Object, _penaltyTaker.Object);
         matchPlayer.Play(match);

         Assert.AreEqual(0, match.HomeScore);
         Assert.AreEqual(1, match.AwayScore);
         Assert.AreEqual(MatchStatus.Ended, match.MatchStatus);
      }

      [TestMethod]
      public void Play_NoPenaltiesTaken_WhenDrawPermitted()
      {
         _randomizer.Setup(x => x.HomeTeamWins(It.IsAny<Dictionary<bool, float>>())).Returns(true);
         _randomizer.Setup(x => x.HomeScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _randomizer.Setup(x => x.AwayScore(It.IsAny<Dictionary<int, float>>())).Returns(0);

         var match = Helper.GetValidMatch();
         match.DrawPermitted = true;

         var matchPlayer = new MatchPlayer(_randomizer.Object, _penaltyTaker.Object);
         matchPlayer.Play(match);

         Assert.IsFalse(match.PenaltiesTaken);
         Assert.AreEqual(0, match.HomePenaltyScore);
         Assert.AreEqual(0, match.AwayPenaltyScore);
         Assert.AreEqual(MatchStatus.Ended, match.MatchStatus);
      }

      [TestMethod]
      public void Play_PenaltiesTaken_WhenDrawNotPermitted()
      {
         _randomizer.Setup(x => x.HomeTeamWins(It.IsAny<Dictionary<bool, float>>())).Returns(true);
         _randomizer.Setup(x => x.HomeScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _randomizer.Setup(x => x.AwayScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _penaltyTaker.Setup(x => x.TakePenalties(It.IsAny<Shared.DomainObjects.Match>()));

         var match = Helper.GetValidMatch();
         match.DrawPermitted = false;

         var matchPlayer = new MatchPlayer(_randomizer.Object, _penaltyTaker.Object);
         matchPlayer.Play(match);

         _penaltyTaker.Verify(x => x.TakePenalties(It.IsAny<Shared.DomainObjects.Match>()), Times.Once);
         Assert.AreEqual(MatchStatus.Ended, match.MatchStatus);
      }
   }
}
