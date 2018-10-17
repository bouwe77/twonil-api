using System;
using System.Collections.Generic;
using System.Linq;
using Randomization;
using TwoNil.Data;
using TwoNil.Logic.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Competitions.Friendlies
{
    public interface IPreSeasonFriendlyManager
    {
        CompetitionSchedule CreatePreSeasonSchedule(List<Team> teams, Season season, MatchDateManager matchDateManager);
    }

    public class PreSeasonFriendlyManager : FriendlyManagerBase, IPreSeasonFriendlyManager
    {
        public PreSeasonFriendlyManager(IUnitOfWorkFactory uowFactory, IRandomizer randomizer, INumberRandomizer numberRandomizer)
            : base(uowFactory, randomizer, numberRandomizer)
        {
        }

        public CompetitionSchedule CreatePreSeasonSchedule(List<Team> teams, Season season, MatchDateManager matchDateManager)
        {
            var competitionSchedule = new CompetitionSchedule();

            // Create a friendly season competition for all friendlies in the season.
            SeasonCompetition friendlySeasonCompetition = new SeasonCompetition
            {
                Competition = _competition,
                Season = season
            };

            competitionSchedule.SeasonCompetitions.Add(friendlySeasonCompetition);

            var roundsAndMatches = CreatePreSeasonRoundsAndMatches(teams);

            foreach (var round in roundsAndMatches)
            {
                var matchDate = matchDateManager.GetNextMatchDate(CompetitionType.Friendly, round.Key);

                var friendlyRound = RoundFactory.CreateRound(null, friendlySeasonCompetition, matchDate, round.Key, _competition);
                competitionSchedule.Rounds.Add(friendlyRound);

                foreach (var match in round.Value)
                {
                    match.Season = season;
                    match.Round = friendlyRound;
                    match.Date = matchDate;
                    match.CompetitionId = _competition.Id;
                    competitionSchedule.Matches.Add(match);
                }
            }

            // Add the teams to the pre-season friendly competition of this season.
            foreach (var team in teams)
            {
                var seasonCompetitionTeam = new SeasonCompetitionTeam
                {
                    SeasonCompetition = friendlySeasonCompetition,
                    Team = team
                };
                competitionSchedule.SeasonCompetitionTeams.Add(seasonCompetitionTeam);
            }

            return competitionSchedule;
        }

        /// <summary>
        /// Creates rounds and matches for the pre-season schedule.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        /// <param name="teams">The teams.</param>
        private Dictionary<int, List<Match>> CreatePreSeasonRoundsAndMatches(List<Team> teams)
        {
            if (teams.GroupBy(t => t.Id).Where(g => g.Count() > 1).Any())
                throw new ArgumentException("No duplicate teams allowed");

            teams.Shuffle();

            var roundRobin = new RoundRobinTournamentManager();
            var schedule = roundRobin.GetSchedule(teams, Constants.HowManyPreSeasonFriendlies, false);

            return schedule;
        }
    }
}
