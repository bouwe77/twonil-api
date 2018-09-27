using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoNil.Logic.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic
{
   [TestClass]
   public class MatchExtensionsTests
   {
      [TestMethod]
      public void EndedInDraw_ReturnsFalse_WhenHomeScoreBiggerThanAwayScore()
      {
         var match = Helper.GetValidMatch();
         match.HomeScore = 1;
         match.AwayScore = 0;

         bool endedInDraw = match.EndedInDraw();

         Assert.IsFalse(endedInDraw);
      }

      [TestMethod]
      public void EndedInDraw_ReturnsFalse_WhenAwayScoreBiggerThanHomeScore()
      {
         var match = Helper.GetValidMatch();
         match.HomeScore = 0;
         match.AwayScore = 1;

         bool endedInDraw = match.EndedInDraw();

         Assert.IsFalse(endedInDraw);
      }

      [TestMethod]
      public void EndedInDraw_ReturnsFalse_WhenHomeScoreIsEqualToAwayScore()
      {
         var match = Helper.GetValidMatch();
         match.HomeScore = 0;
         match.AwayScore = 0;

         bool endedInDraw = match.EndedInDraw();

         Assert.IsTrue(endedInDraw);
      }

      [TestMethod]
      public void EndedInDraw_ReturnsFalse_WhenHomeScoreIsEqualToAwayScoreAndPenaltiesWereTaken()
      {
         var match = Helper.GetValidMatch();
         match.HomeScore = 0;
         match.AwayScore = 0;
         match.HomePenaltyScore = 1;
         match.AwayPenaltyScore = 0;
         match.PenaltiesTaken = true;

         bool endedInDraw = match.EndedInDraw();

         Assert.IsTrue(endedInDraw);
      }

      [TestMethod]
      public void GetWinner_ReturnsNoWinner_WhenMatchHasNotEnded()
      {
         var match = Helper.GetValidMatch();
         match.MatchStatus = MatchStatus.NotStarted;

         var winner = match.GetWinner();
         Assert.IsNull(winner);
      }

      [TestMethod]
      public void GetWinner_ReturnsNoWinner_WhenBothTeamsScoreEqualNumberOfGoals()
      {
         var match = Helper.GetValidMatch();
         match.MatchStatus = MatchStatus.Ended;

         match.HomeScore = 0;
         match.AwayScore = 0;

         var winner = match.GetWinner();
         Assert.IsNull(winner);
      }

      [TestMethod]
      public void GetWinner_ReturnsHomeTeam_WhenHomeTeamScoresMoreGoals()
      {
         var match = Helper.GetValidMatch();
         match.MatchStatus = MatchStatus.Ended;

         match.HomeScore = 1;
         match.AwayScore = 0;

         var winner = match.GetWinner();
         Assert.AreEqual(Helper.HomeTeam, winner);
      }

      [TestMethod]
      public void GetWinner_ReturnsAwayTeam_WhenAwayTeamScoresMoreGoals()
      {
         var match = Helper.GetValidMatch();
         match.MatchStatus = MatchStatus.Ended;

         match.HomeScore = 0;
         match.AwayScore = 1;

         var winner = match.GetWinner();
         Assert.AreEqual(Helper.AwayTeam, winner);
      }

      [TestMethod]
      public void GetWinner_ReturnsHomeTeam_WhenHomeTeamWinsThePenaltyShootout()
      {
         var match = Helper.GetValidMatch();
         match.MatchStatus = MatchStatus.Ended;

         match.HomeScore = 0;
         match.AwayScore = 0;
         match.HomePenaltyScore = 1;
         match.AwayPenaltyScore = 0;
         match.PenaltiesTaken = true;

         var winner = match.GetWinner();
         Assert.AreEqual(Helper.HomeTeam, winner);
      }

      [TestMethod]
      public void GetWinner_ReturnsAwayTeam_WhenAwayTeamWinsThePenaltyShootout()
      {
         var match = Helper.GetValidMatch();
         match.MatchStatus = MatchStatus.Ended;

         match.HomeScore = 0;
         match.AwayScore = 0;
         match.HomePenaltyScore = 0;
         match.AwayPenaltyScore = 1;
         match.PenaltiesTaken = true;

         var winner = match.GetWinner();
         Assert.AreEqual(Helper.AwayTeam, winner);
      }
   }
}
