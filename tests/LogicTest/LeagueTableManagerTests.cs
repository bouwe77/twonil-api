using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoNil.Data;
using TwoNil.Data.Repositories;
using TwoNil.Logic.Competitions;
using TwoNil.Shared.DomainObjects;
using Match = TwoNil.Shared.DomainObjects.Match;

namespace TwoNil.Logic
{
   [TestClass]
   public class LeagueTableManagerTests
   {
      private Mock<IRepositoryFactory> _repositoryFactory;
      private Mock<IMatchRepository> _matchRepository;

      [TestInitialize]
      public void TestInitialize()
      {
         _matchRepository = new Mock<IMatchRepository>(MockBehavior.Strict);
         _repositoryFactory = new Mock<IRepositoryFactory>(MockBehavior.Strict);
      }

      [TestCleanup]
      public void TestCleanup()
      {
         _matchRepository.VerifyAll();
         _repositoryFactory.VerifyAll();
      }

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

         _repositoryFactory.Setup(x => x.CreateMatchRepository()).Returns(_matchRepository.Object);
         var leagueTableManager = new LeagueTableManager(_repositoryFactory.Object);
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

      [TestMethod]
      public void UpdateLeagueTable_SortsAlpabetically()
      {
         // We need 4 teams.
         var team1 = new Team { Id = "1", Name = "Team1" };
         var team2 = new Team { Id = "2", Name = "Team2" };
         var team3 = new Team { Id = "3", Name = "Team3" };
         var team4 = new Team { Id = "4", Name = "Team4" };

         // Every team has a league table position.
         var positions = new List<LeagueTablePosition>
         {
            new LeagueTablePosition { Team = team4, Position = 4 },
            new LeagueTablePosition { Team = team1, Position = 1 },
            new LeagueTablePosition { Team = team2, Position = 2 },
            new LeagueTablePosition { Team = team3, Position = 3 },
         };

         var leagueTable = new LeagueTable { LeagueTablePositions = positions };

         _repositoryFactory.Setup(x => x.CreateMatchRepository()).Returns(_matchRepository.Object);
         var leagueTableManager = new LeagueTableManager(_repositoryFactory.Object);
         leagueTableManager.UpdateLeagueTable(leagueTable, new List<Match>());

         // Because all positions have the exact same number of matches, points and goals, the positions should be sorted alphabetically.
         Assert.AreEqual("1", leagueTable.LeagueTablePositions[0].TeamId);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[0].Position);
         Assert.AreEqual("2", leagueTable.LeagueTablePositions[1].TeamId);
         Assert.AreEqual(2, leagueTable.LeagueTablePositions[1].Position);
         Assert.AreEqual("3", leagueTable.LeagueTablePositions[2].TeamId);
         Assert.AreEqual(3, leagueTable.LeagueTablePositions[2].Position);
         Assert.AreEqual("4", leagueTable.LeagueTablePositions[3].TeamId);
         Assert.AreEqual(4, leagueTable.LeagueTablePositions[3].Position);
      }

      [TestMethod]
      public void UpdateLeagueTable_ChecksMatchResults_IfTwoMatchesHaveBeenPlayed()
      {
         // We need 4 teams.
         var team1 = new Team { Id = "1", Name = "Team1" };
         var team2 = new Team { Id = "2", Name = "Team2" };
         var team3 = new Team { Id = "3", Name = "Team3" };
         var team4 = new Team { Id = "4", Name = "Team4" };

         // Team 1 and 2 both have 6 points and team 3 and 4 both have 3 points.
         var positions = new List<LeagueTablePosition>
         {
            new LeagueTablePosition { Team = team1, Matches = 2, Points = 6 },
            new LeagueTablePosition { Team = team2, Matches = 2, Points = 6 },
            new LeagueTablePosition { Team = team3, Matches = 2, Points = 3 },
            new LeagueTablePosition { Team = team4, Matches = 2, Points = 3 },
         };

         var leagueTable = new LeagueTable { LeagueTablePositions = positions };

         // Although not very realistic, but just to prove the method works, 
         // Team2 and Team4 have better match results against Team1 and Team3 respectively, so their position must be swapped.
         var matchesBetweenTeam1And2 = new List<Match>
         {
            new Match { HomeTeam = team1, AwayTeam = team2, HomeScore = 0, AwayScore = 1 }, //Team1-Team2 0-1
            new Match { HomeTeam = team2, AwayTeam = team1, HomeScore = 1, AwayScore = 0 }  //Team2-Team1 1-0, so Team2 wins 2-0 on aggregate
         };

         var matchesBetweenTeam3And4 = new List<Match>
         {
            new Match { HomeTeam = team4, AwayTeam = team3, HomeScore = 1, AwayScore = 0 }, // Team4-Team3 1-0
            new Match { HomeTeam = team3, AwayTeam = team4, HomeScore = 0, AwayScore = 0 }, // Team3-Team4 0-0, so Team4 wins 1-0 on aggregate
         };

         _repositoryFactory.Setup(x => x.CreateMatchRepository()).Returns(_matchRepository.Object);
         _matchRepository.SetupSequence(x => x.GetMatchesBetweenTeams(It.IsAny<SeasonCompetition>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(matchesBetweenTeam1And2)
            .Returns(matchesBetweenTeam3And4);

         var leagueTableManager = new LeagueTableManager(_repositoryFactory.Object);
         leagueTableManager.UpdateLeagueTable(leagueTable, new List<Match>());

         Assert.AreEqual("2", leagueTable.LeagueTablePositions[0].TeamId);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[0].Position);
         Assert.AreEqual("1", leagueTable.LeagueTablePositions[1].TeamId);
         Assert.AreEqual(2, leagueTable.LeagueTablePositions[1].Position);
         Assert.AreEqual("4", leagueTable.LeagueTablePositions[2].TeamId);
         Assert.AreEqual(3, leagueTable.LeagueTablePositions[2].Position);
         Assert.AreEqual("3", leagueTable.LeagueTablePositions[3].TeamId);
         Assert.AreEqual(4, leagueTable.LeagueTablePositions[3].Position);
      }

      [TestMethod]
      public void UpdateLeagueTable_ChecksMatchResults_IfTwoMatchesHaveBeenPlayedAndAllTeamsHaveTheSamePosition()
      {
         // We need 4 teams.
         var team1 = new Team { Id = "1", Name = "Team1" };
         var team2 = new Team { Id = "2", Name = "Team2" };
         var team3 = new Team { Id = "3", Name = "Team3" };
         var team4 = new Team { Id = "4", Name = "Team4" };

         // All teams have 6 points.
         var positions = new List<LeagueTablePosition>
         {
            new LeagueTablePosition { Team = team2, Matches = 2, Points = 6, Position = 1 },
            new LeagueTablePosition { Team = team4, Matches = 2, Points = 6, Position = 2 },
            new LeagueTablePosition { Team = team1, Matches = 2, Points = 6, Position = 3 },
            new LeagueTablePosition { Team = team3, Matches = 2, Points = 6, Position = 4 },
         };

         var leagueTable = new LeagueTable { LeagueTablePositions = positions };

         // Although not very realistic, but just to prove the method works, 
         // Team3 on position 4 has the best match results, followed by Team1 on position 3 etc., so their positions must be swapped.
         var matchesBetweenTeam4And2 = new List<Match>
         {
            new Match { HomeTeam = team4, AwayTeam = team2, HomeScore = 1, AwayScore = 0 }, //Team4-Team2 1-0
            new Match { HomeTeam = team2, AwayTeam = team4, HomeScore = 0, AwayScore = 1 }  //Team2-Team4 0-1, so Team4 wins 2-0 on aggregate
         };

         var matchesBetweenTeam1And4 = new List<Match>
         {
            new Match { HomeTeam = team1, AwayTeam = team4, HomeScore = 0, AwayScore = 1 }, // Team1-Team4 0-1
            new Match { HomeTeam = team4, AwayTeam = team1, HomeScore = 0, AwayScore = 0 }, // Team4-Team1 0-0, so Team4 wins 1-0 on aggregate
         };

         var matchesBetweenTeam3And1 = new List<Match>
         {
            new Match { HomeTeam = team3, AwayTeam = team1, HomeScore = 0, AwayScore = 1 }, // Team3-Team1 0-1
            new Match { HomeTeam = team1, AwayTeam = team3, HomeScore = 0, AwayScore = 0 }, // Team1-Team3 0-0, so Team1 wins 1-0 on aggregate
         };

         _repositoryFactory.Setup(x => x.CreateMatchRepository()).Returns(_matchRepository.Object);
         _matchRepository.SetupSequence(x => x.GetMatchesBetweenTeams(It.IsAny<SeasonCompetition>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(matchesBetweenTeam4And2)
            .Returns(matchesBetweenTeam1And4)
            .Returns(matchesBetweenTeam3And1);
         var leagueTableManager = new LeagueTableManager(_repositoryFactory.Object);
         leagueTableManager.UpdateLeagueTable(leagueTable, new List<Match>());

         Assert.AreEqual("4", leagueTable.LeagueTablePositions[0].TeamId);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[0].Position);
         Assert.AreEqual("2", leagueTable.LeagueTablePositions[1].TeamId);
         Assert.AreEqual(2, leagueTable.LeagueTablePositions[1].Position);
         Assert.AreEqual("1", leagueTable.LeagueTablePositions[2].TeamId);
         Assert.AreEqual(3, leagueTable.LeagueTablePositions[2].Position);
         Assert.AreEqual("3", leagueTable.LeagueTablePositions[3].TeamId);
         Assert.AreEqual(4, leagueTable.LeagueTablePositions[3].Position);
      }

      [TestMethod]
      public void UpdateLeagueTable_ChecksMatchResults_ChangesNothing_WhenPositionsAreCorrect()
      {
         // We need 4 teams.
         var team1 = new Team { Id = "1", Name = "Team1" };
         var team2 = new Team { Id = "2", Name = "Team2" };
         var team3 = new Team { Id = "3", Name = "Team3" };
         var team4 = new Team { Id = "4", Name = "Team4" };

         // Team 1 and 2 both have 6 points and team 3 and 4 both have 3 points.
         var positions = new List<LeagueTablePosition>
         {
            new LeagueTablePosition { Team = team1, Matches = 2, Points = 6 },
            new LeagueTablePosition { Team = team2, Matches = 2, Points = 6 },
            new LeagueTablePosition { Team = team3, Matches = 2, Points = 3 },
            new LeagueTablePosition { Team = team4, Matches = 2, Points = 3 },
         };

         var leagueTable = new LeagueTable { LeagueTablePositions = positions };

         // Not only Team1 has position 1, it also had better results against Team2.
         // The same for Team3 and Team4.
         var matchesBetweenTeam1And2 = new List<Match>
         {
            new Match { HomeTeam = team1, AwayTeam = team2, HomeScore = 1, AwayScore = 0 }, //Team1-Team2 1-0
            new Match { HomeTeam = team2, AwayTeam = team1, HomeScore = 0, AwayScore = 1 }  //Team2-Team1 0-1, so Team1 wins 2-0 on aggregate
         };

         var matchesBetweenTeam3And4 = new List<Match>
         {
            new Match { HomeTeam = team4, AwayTeam = team3, HomeScore = 0, AwayScore = 1 }, // Team4-Team3 0-1
            new Match { HomeTeam = team3, AwayTeam = team4, HomeScore = 0, AwayScore = 0 }, // Team3-Team4 0-0, so Team3 wins 1-0 on aggregate
         };

         _repositoryFactory.Setup(x => x.CreateMatchRepository()).Returns(_matchRepository.Object);
         _matchRepository.SetupSequence(x => x.GetMatchesBetweenTeams(It.IsAny<SeasonCompetition>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(matchesBetweenTeam1And2)
            .Returns(matchesBetweenTeam3And4);

         var leagueTableManager = new LeagueTableManager(_repositoryFactory.Object);
         leagueTableManager.UpdateLeagueTable(leagueTable, new List<Match>());

         Assert.AreEqual("1", leagueTable.LeagueTablePositions[0].TeamId);
         Assert.AreEqual(1, leagueTable.LeagueTablePositions[0].Position);
         Assert.AreEqual("2", leagueTable.LeagueTablePositions[1].TeamId);
         Assert.AreEqual(2, leagueTable.LeagueTablePositions[1].Position);
         Assert.AreEqual("3", leagueTable.LeagueTablePositions[2].TeamId);
         Assert.AreEqual(3, leagueTable.LeagueTablePositions[2].Position);
         Assert.AreEqual("4", leagueTable.LeagueTablePositions[3].TeamId);
         Assert.AreEqual(4, leagueTable.LeagueTablePositions[3].Position);
      }
   }
}
