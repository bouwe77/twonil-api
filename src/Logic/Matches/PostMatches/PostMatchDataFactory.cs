using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Matches.PostMatches
{
    public interface IPostMatchDataFactory
    {
        PostMatchData InitializePostMatchData(IEnumerable<Match> matches);
    }

    public class PostMatchDataFactory : IPostMatchDataFactory
    {
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly string _seasonId;

        public PostMatchDataFactory(IUnitOfWorkFactory uowFactory)
        {
            _uowFactory = uowFactory;
        }

        public PostMatchData InitializePostMatchData(IEnumerable<Match> matches)
        {
            var seasonId = matches.First().SeasonId;

            PostMatchData postMatchData;
            using (var uow = _uowFactory.Create())
            {
                postMatchData = new PostMatchData(
                    uow.Competitions.GetLeague1().Id,
                    uow.Competitions.GetLeague2().Id,
                    uow.Competitions.GetLeague3().Id,
                    uow.Competitions.GetLeague4().Id,
                    uow.Competitions.GetNationalCup().Id,
                    uow.Competitions.GetNationalSuperCup().Id,
                    uow.Competitions.GetFriendly().Id);

                postMatchData.MatchDateTime = matches.First().Date;

                postMatchData.Season = uow.Seasons.GetOne(matches.First().SeasonId);

                // Add all rounds as Dictionary of CompetitionId and Round.
                postMatchData.Rounds = matches.Select(m => m.Round).GroupBy(r => r.CompetitionId).ToDictionary(g => g.Key, g => g.First());

                // Add all matches as Dictionary of CompetitionId and List of Matches.
                postMatchData.Matches = matches.GroupBy(m => m.CompetitionId).ToDictionary(g => g.Key, g => g.AsEnumerable());

                // Add all teams as Dictionary of TeamId and Team.
                postMatchData.Teams = uow.Teams.GetTeams().ToDictionary(k => k.Id, t => t);

                // Add all league tables as Dictionary of CompetitionId and LeagueTable.
                postMatchData.LeagueTables = uow.LeagueTables.GetBySeason(_seasonId).ToDictionary(k => k.SeasonCompetition.CompetitionId, l => l);

                // Add all leagues as Dictionary of CompetitionId and League.
                postMatchData.Leagues = uow.Competitions.GetLeagues().ToDictionary(k => k.Id, l => l);

                // Add the managers team.
                postMatchData.ManagersTeam = uow.GameInfos.GetGameInfo().CurrentTeam;

                // Add the season statistics.
                postMatchData.SeasonStatistics = uow.Seasons.GetBySeason(_seasonId);

                // Add the season/team statistics.
                postMatchData.SeasonTeamStatistics = uow.SeasonTeamStatistics
                                                        .GetBySeason(_seasonId)
                                                        .ToDictionary(k => k.TeamId, v => v);

                // Add the team statistics.
                postMatchData.TeamStatistics = uow.TeamStatistics.GetAll().ToDictionary(k => k.TeamId, v => v);

                // Determine season has ended, all matches have been played in several competitions, etc.
                int numberOfLeagueMatches = (Constants.HowManyTeamsPerLeague - 1) * 2;
                postMatchData.League1Finished = postMatchData.LeagueTableLeague1.LeagueTablePositions.All(x => x.Matches == numberOfLeagueMatches);

                return postMatchData;
            }
        }
    }
}
