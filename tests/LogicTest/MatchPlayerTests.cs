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
      private Mock<IMatchPlayerRandomizer> _mock;

      [TestInitialize]
      public void TestIntialize()
      {
         _mock = new Mock<IMatchPlayerRandomizer>(MockBehavior.Strict);
      }

      [TestCleanup]
      public void TestCleanup()
      {
         _mock.VerifyAll();
      }

      [TestMethod]
      public void Play_HomeAndAwayScoreAreDetermined()
      {
         _mock.Setup(x => x.HomeTeamWins(It.IsAny<Dictionary<bool, float>>())).Returns(true);
         _mock.Setup(x => x.HomeScore(It.IsAny<Dictionary<int, float>>())).Returns(7);
         _mock.Setup(x => x.AwayScore(It.IsAny<Dictionary<int, float>>())).Returns(3);

         var match = Helper.GetValidMatch();

         var matchPlayer = new MatchPlayer(_mock.Object);
         matchPlayer.Play(match);

         Assert.AreEqual(7, match.HomeScore);
         Assert.AreEqual(3, match.AwayScore);
         Assert.AreEqual(MatchStatus.Ended, match.MatchStatus);
      }

      [TestMethod]
      public void Play_HomeTeamWins_WhenAwayScoreIsBiggerButHomeTeamWins()
      {
         _mock.Setup(x => x.HomeTeamWins(It.IsAny<Dictionary<bool, float>>())).Returns(true);
         _mock.Setup(x => x.HomeScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _mock.Setup(x => x.AwayScore(It.IsAny<Dictionary<int, float>>())).Returns(1);

         var match = Helper.GetValidMatch();

         var matchPlayer = new MatchPlayer(_mock.Object);
         matchPlayer.Play(match);

         Assert.AreEqual(1, match.HomeScore);
         Assert.AreEqual(0, match.AwayScore);
         Assert.AreEqual(MatchStatus.Ended, match.MatchStatus);
      }

      [TestMethod]
      public void Play_AwayTeamWins_WhenHomeScoreIsBiggerButAwayTeamWins()
      {
         _mock.Setup(x => x.HomeTeamWins(It.IsAny<Dictionary<bool, float>>())).Returns(false);
         _mock.Setup(x => x.HomeScore(It.IsAny<Dictionary<int, float>>())).Returns(1);
         _mock.Setup(x => x.AwayScore(It.IsAny<Dictionary<int, float>>())).Returns(0);

         var match = Helper.GetValidMatch();

         var matchPlayer = new MatchPlayer(_mock.Object);
         matchPlayer.Play(match);

         Assert.AreEqual(0, match.HomeScore);
         Assert.AreEqual(1, match.AwayScore);
         Assert.AreEqual(MatchStatus.Ended, match.MatchStatus);
      }

      [TestMethod]
      public void Play_NoPenaltiesTaken_WhenDrawPermitted()
      {
         _mock.Setup(x => x.HomeTeamWins(It.IsAny<Dictionary<bool, float>>())).Returns(true);
         _mock.Setup(x => x.HomeScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _mock.Setup(x => x.AwayScore(It.IsAny<Dictionary<int, float>>())).Returns(0);

         var match = Helper.GetValidMatch();
         match.DrawPermitted = true;

         var matchPlayer = new MatchPlayer(_mock.Object);
         matchPlayer.Play(match);

         Assert.IsFalse(match.PenaltiesTaken);
         Assert.AreEqual(0, match.HomePenaltyScore);
         Assert.AreEqual(0, match.AwayPenaltyScore);
         Assert.AreEqual(MatchStatus.Ended, match.MatchStatus);
      }

      [TestMethod]
      public void Play_PenaltiesTaken_WhenDrawNotPermitted()
      {
         _mock.Setup(x => x.HomeTeamWins(It.IsAny<Dictionary<bool, float>>())).Returns(true);
         _mock.Setup(x => x.HomeScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _mock.Setup(x => x.AwayScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _mock.Setup(x => x.HomePenaltyScore(It.IsAny<Dictionary<int, float>>())).Returns(2);
         _mock.Setup(x => x.AwayPenaltyScore(It.IsAny<Dictionary<int, float>>())).Returns(1);

         var match = Helper.GetValidMatch();
         match.DrawPermitted = false;

         var matchPlayer = new MatchPlayer(_mock.Object);
         matchPlayer.Play(match);

         Assert.IsTrue(match.PenaltiesTaken);
         Assert.AreEqual(2, match.HomePenaltyScore);
         Assert.AreEqual(1, match.AwayPenaltyScore);
         Assert.AreEqual(MatchStatus.Ended, match.MatchStatus);
      }

      [TestMethod]
      public void Play_HomeTeamWinsPenalties_WhenRandomizerLetsPenaltiesEndInDraw()
      {
         _mock.Setup(x => x.HomeTeamWins(It.IsAny<Dictionary<bool, float>>())).Returns(false);
         _mock.Setup(x => x.HomeScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _mock.Setup(x => x.AwayScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _mock.Setup(x => x.HomePenaltyScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _mock.Setup(x => x.AwayPenaltyScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _mock.Setup(x => x.HomeTeamWinsPenalties()).Returns(true);

         var match = Helper.GetValidMatch();
         match.DrawPermitted = false;

         var matchPlayer = new MatchPlayer(_mock.Object);
         matchPlayer.Play(match);

         Assert.IsTrue(match.PenaltiesTaken);
         Assert.AreEqual(1, match.HomePenaltyScore);
         Assert.AreEqual(0, match.AwayPenaltyScore);
         Assert.AreEqual(MatchStatus.Ended, match.MatchStatus);
      }

      [TestMethod]
      public void Play_AwayTeamWinsPenalties_WhenRandomizerLetsPenaltiesEndInDraw()
      {
         _mock.Setup(x => x.HomeTeamWins(It.IsAny<Dictionary<bool, float>>())).Returns(false);
         _mock.Setup(x => x.HomeScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _mock.Setup(x => x.AwayScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _mock.Setup(x => x.HomePenaltyScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _mock.Setup(x => x.AwayPenaltyScore(It.IsAny<Dictionary<int, float>>())).Returns(0);
         _mock.Setup(x => x.HomeTeamWinsPenalties()).Returns(false);

         var match = Helper.GetValidMatch();
         match.DrawPermitted = false;

         var matchPlayer = new MatchPlayer(_mock.Object);
         matchPlayer.Play(match);

         Assert.IsTrue(match.PenaltiesTaken);
         Assert.AreEqual(0, match.HomePenaltyScore);
         Assert.AreEqual(1, match.AwayPenaltyScore);
         Assert.AreEqual(MatchStatus.Ended, match.MatchStatus);
      }
   }
}
