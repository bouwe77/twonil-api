using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Logic.Functionality.Competitions;
using TwoNil.Logic.Functionality.Competitions.Friendlies;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic
{
    [TestClass]
    public class PreSeasonFriendlyManagerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatePreSeasonSchedule_WhenDuplicateTeams_ThrowsException()
        {
            // Arrange
            var teams = GetTeams(1).ToList();
            teams.Add(teams[0]);

            // Act
            var friendlyManager = new PreSeasonFriendlyManager();
            friendlyManager.CreatePreSeasonSchedule(teams, new Season(), GetMatchDateManager());
        }

        [TestMethod]
        public void CreatePreSeasonSchedule_CreatesRoundsAndMatches()
        {
            // Arrange
            const int numberOfTeams = 16;
            const int expectedNumberOfRounds = 4;

            var friendlyManager = new PreSeasonFriendlyManager();

            var teams = GetTeams(numberOfTeams).ToList();
            var season = new Season();
            MatchDateManager matchDateManager = GetMatchDateManager();

            // Act
            var schedule = friendlyManager.CreatePreSeasonSchedule(teams, season, matchDateManager);

            // Assert

            // Check the expected number of rounds.
            Assert.AreEqual(expectedNumberOfRounds, schedule.Rounds.Count);

            // There are 8 matches per round.
            var numberOfMatchesPerRound = schedule.Matches.GroupBy(m => m.RoundId).Select(c => c.Count());
            Assert.IsTrue(numberOfMatchesPerRound.All(x => x == 8));

            // Per round, check all matches have different teams.
            foreach (var round in schedule.Rounds)
            {
                var matches = schedule.Matches.Where(m => m.RoundId == round.Id);
                var teamsInThisRound = matches.Select(m => m.HomeTeamId).ToList();
                teamsInThisRound.AddRange(matches.Select(m => m.AwayTeamId));
                Assert.IsTrue(teamsInThisRound.Distinct().Count() == teamsInThisRound.Count(), "Per round all matches must have different teams");
            }

            // Check every team has different opponents.
            foreach (var team in teams)
            {
                var opponents = schedule.Matches.Where(m => m.HomeTeam.Id == team.Id).Select(m => m.AwayTeam).ToList();
                opponents.AddRange(schedule.Matches.Where(m => m.AwayTeam.Id == team.Id).Select(m => m.HomeTeam));
                Assert.IsFalse(opponents.GroupBy(t => t.Id).Where(g => g.Count() > 1).Any(), "Every team must have different opponents");
            }
        }

        private static MatchDateManager GetMatchDateManager()
        {
            var matchDateManager = new MatchDateManager(2018);
            matchDateManager.Initialize();
            return matchDateManager;
        }

        private IEnumerable<Team> GetTeams(int howMany)
        {
            return Enumerable.Range(1, howMany).Select(x => new Team { Name = $"Team {x}" });
        }
    }
}
