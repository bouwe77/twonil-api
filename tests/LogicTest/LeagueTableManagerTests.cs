using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoNil.Logic.Functionality.Competitions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic
{
   [TestClass]
   public class LeagueTableManagerTests
   {
      [TestMethod]
      public void UpdateLeagueTable_WorksAsExpected()
      {
         // We need 4 teams.
         var team1 = new Team { Id = "1", Name = "Team1" };
         var team2 = new Team { Id = "2", Name = "Team2" };
         var team3 = new Team { Id = "3", Name = "Team3" };
         var team4 = new Team { Id = "4", Name = "Team4" };

         // Every team has a league table position.
         var positions = new List<LeagueTablePosition>
         {
            new LeagueTablePosition { Team = team1 },
            new LeagueTablePosition { Team = team2 },
            new LeagueTablePosition { Team = team3 },
            new LeagueTablePosition { Team = team4 },
         };

         // All 4 teams are playing a match.
         var matches = new List<Match>
         {
            // Team1 vs Team2: 1-0
            new Match { HomeTeam = team1, AwayTeam = team2, HomeScore = 1, AwayScore = 0 },
            // Team3 vs Team4: 0-0
            new Match { HomeTeam = team3, AwayTeam = team4, HomeScore = 0, AwayScore = 0 }
         };

         var leagueTable = new LeagueTable { LeagueTablePositions = positions };

         var leagueTableManager = new LeagueTableManager();
         leagueTableManager.UpdateLeagueTable(leagueTable, matches);

         // Expected positions:[1] Team1, [2] Team3, [3] Team4, [4] Team2.
         // Note that Team3 is 2nd because their results are totally equal, so we order on team name alphabetically...

         // Team1 is 1st, has played 1 match, 3 points, 1 win, 0 draws, 0 losses, 1 goal scored, 0 conceded
         Assert.AreEqual(team1, leagueTable.LeagueTablePositions[0].Team);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[0].Position);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[0].Team.CurrentLeaguePosition);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[0].Matches);
         Assert.AreEqual(3, leagueTable.LeagueTablePositions[0].Points);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[0].Wins);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[0].Losses);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[0].Draws);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[0].GoalsScored);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[0].GoalsConceded);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[0].GoalDifference);

         // Team3 is 2nd, has played 1 match, 1 point, 0 wins, 1 draw, 0 losses, 0 goals scored, 0 conceded
         Assert.AreEqual(team3, leagueTable.LeagueTablePositions[1].Team);
         Assert.AreEqual(2, leagueTable.LeagueTablePositions[1].Position);
         Assert.AreEqual(2, leagueTable.LeagueTablePositions[1].Team.CurrentLeaguePosition);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[1].Matches);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[1].Points);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[1].Wins);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[1].Losses);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[1].Draws);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[1].GoalsScored);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[1].GoalsConceded);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[1].GoalDifference);

         // Team4 is 3rd, has played 1 match, 1 point, 0 wins, 1 draw, 0 losses, 0 goals scored, 0 conceded
         Assert.AreEqual(team4, leagueTable.LeagueTablePositions[2].Team);
         Assert.AreEqual(3, leagueTable.LeagueTablePositions[2].Position);
         Assert.AreEqual(3, leagueTable.LeagueTablePositions[2].Team.CurrentLeaguePosition);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[2].Matches);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[2].Points);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[2].Wins);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[2].Losses);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[2].Draws);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[2].GoalsScored);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[2].GoalsConceded);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[2].GoalDifference);

         // Team2 is 4th, has played 1 match, 0 points, 0 wins, 0 draws, 1 loss, 0 goals scored, 1 conceded
         Assert.AreEqual(team2, leagueTable.LeagueTablePositions[3].Team);
         Assert.AreEqual(4, leagueTable.LeagueTablePositions[3].Position);
         Assert.AreEqual(4, leagueTable.LeagueTablePositions[3].Team.CurrentLeaguePosition);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[3].Matches);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[3].Points);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[3].Wins);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[3].Losses);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[3].Draws);
         Assert.AreEqual(0, leagueTable.LeagueTablePositions[3].GoalsScored);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[3].GoalsConceded);
         Assert.AreEqual(-1, leagueTable.LeagueTablePositions[3].GoalDifference);
      }
   }
}
