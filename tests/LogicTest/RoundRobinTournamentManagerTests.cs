using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoNil.Logic.Functionality.Competitions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic
{
   [TestClass]
   public class RoundRobinTournamentManagerTests
   {
      [TestMethod]
      public void GetLeagueSchedule_ForOddNumberOfTeams()
      {
         var roundRobinTournamentManager = new RoundRobinTournamentManager();

         var teamA = new Team { Name = "A" };
         var teamB = new Team { Name = "B" };
         var teamC = new Team { Name = "C" };

         var teams = new List<Team> { teamA, teamB, teamC };

         var schedule = roundRobinTournamentManager.GetSchedule(teams);

         // Totaal 6 rondes.
         Assert.AreEqual(6, schedule.Count);

         // De eerste ronde is B-C.
         var matchesRound1 = schedule[0];
         Assert.AreEqual(1, matchesRound1.Count);
         Assert.AreEqual(teamB, matchesRound1[0].HomeTeam);
         Assert.AreEqual(teamC, matchesRound1[0].AwayTeam);

         // De tweede ronde is C-A.
         var matchesRound2 = schedule[1];
         Assert.AreEqual(1, matchesRound2.Count);
         Assert.AreEqual(teamC, matchesRound2[0].HomeTeam);
         Assert.AreEqual(teamA, matchesRound2[0].AwayTeam);

         // De derde ronde is A-B.
         var matchesRound3 = schedule[2];
         Assert.AreEqual(1, matchesRound3.Count);
         Assert.AreEqual(teamA, matchesRound3[0].HomeTeam);
         Assert.AreEqual(teamB, matchesRound3[0].AwayTeam);

         // De vierde ronde is C-B.
         var matchesRound4 = schedule[3];
         Assert.AreEqual(1, matchesRound4.Count);
         Assert.AreEqual(teamC, matchesRound4[0].HomeTeam);
         Assert.AreEqual(teamB, matchesRound4[0].AwayTeam);

         // De vijfde ronde is A-C.
         var matchesRound5 = schedule[4];
         Assert.AreEqual(1, matchesRound5.Count);
         Assert.AreEqual(teamA, matchesRound5[0].HomeTeam);
         Assert.AreEqual(teamC, matchesRound5[0].AwayTeam);

         // De zesde ronde is B-A.
         var matchesRound6 = schedule[5];
         Assert.AreEqual(1, matchesRound6.Count);
         Assert.AreEqual(teamB, matchesRound6[0].HomeTeam);
         Assert.AreEqual(teamA, matchesRound6[0].AwayTeam);
      }

      [TestMethod]
      public void GetLeagueSchedule_ForEvenNumberOfTeams()
      {
         var roundRobinTournamentManager = new RoundRobinTournamentManager();

         var teamA = new Team { Name = "A" };
         var teamB = new Team { Name = "B" };
         var teamC = new Team { Name = "C" };
         var teamD = new Team { Name = "D" };

         var teams = new List<Team> { teamA, teamB, teamC, teamD };

         var schedule = roundRobinTournamentManager.GetSchedule(teams);

         // Totaal 6 rondes.
         Assert.AreEqual(6, schedule.Count);

         // De eerste ronde is A-D en B-C.
         var matchesRound1 = schedule[0];
         Assert.AreEqual(2, matchesRound1.Count);
         Assert.AreEqual(teamA, matchesRound1[0].HomeTeam);
         Assert.AreEqual(teamD, matchesRound1[0].AwayTeam);
         Assert.AreEqual(teamB, matchesRound1[1].HomeTeam);
         Assert.AreEqual(teamC, matchesRound1[1].AwayTeam);

         // De tweede ronde is C-A en D-B.
         var matchesRound2 = schedule[1];
         Assert.AreEqual(2, matchesRound2.Count);
         Assert.AreEqual(teamC, matchesRound2[0].HomeTeam);
         Assert.AreEqual(teamA, matchesRound2[0].AwayTeam);
         Assert.AreEqual(teamD, matchesRound2[1].HomeTeam);
         Assert.AreEqual(teamB, matchesRound2[1].AwayTeam);

         // De derde ronde is A-B en C-D.
         var matchesRound3 = schedule[2];
         Assert.AreEqual(2, matchesRound3.Count);
         Assert.AreEqual(teamA, matchesRound3[0].HomeTeam);
         Assert.AreEqual(teamB, matchesRound3[0].AwayTeam);
         Assert.AreEqual(teamC, matchesRound3[1].HomeTeam);
         Assert.AreEqual(teamD, matchesRound3[1].AwayTeam);

         // De vierde ronde is D-A en C-B.
         var matchesRound4 = schedule[3];
         Assert.AreEqual(2, matchesRound4.Count);
         Assert.AreEqual(teamD, matchesRound4[0].HomeTeam);
         Assert.AreEqual(teamA, matchesRound4[0].AwayTeam);
         Assert.AreEqual(teamC, matchesRound4[1].HomeTeam);
         Assert.AreEqual(teamB, matchesRound4[1].AwayTeam);

         // De vijfde ronde is A-C en B-D.
         var matchesRound5 = schedule[4];
         Assert.AreEqual(2, matchesRound5.Count);
         Assert.AreEqual(teamA, matchesRound5[0].HomeTeam);
         Assert.AreEqual(teamC, matchesRound5[0].AwayTeam);
         Assert.AreEqual(teamB, matchesRound5[1].HomeTeam);
         Assert.AreEqual(teamD, matchesRound5[1].AwayTeam);

         // De zesde ronde is B-A en D-C.
         var matchesRound6 = schedule[5];
         Assert.AreEqual(2, matchesRound6.Count);
         Assert.AreEqual(teamB, matchesRound6[0].HomeTeam);
         Assert.AreEqual(teamA, matchesRound6[0].AwayTeam);
         Assert.AreEqual(teamD, matchesRound6[1].HomeTeam);
         Assert.AreEqual(teamC, matchesRound6[1].AwayTeam);
      }
   }
}
