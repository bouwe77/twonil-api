using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Matches.PostMatches
{
    public class PostMatchDataFactory
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IEnumerable<Match> _matches;
        private readonly string _seasonId;

        public PostMatchDataFactory(IRepositoryFactory repositoryFactory, IEnumerable<Match> matches)
        {
            _repositoryFactory = repositoryFactory;
            _matches = matches;
            _seasonId = matches.First().SeasonId;
        }

        public PostMatchData InitializePostMatchData()
        {
            PostMatchData postMatchData;
            using (var competitionRepository = _repositoryFactory.CreateCompetitionRepository())
            {
                postMatchData = new PostMatchData(
                    competitionRepository.GetLeague1().Id,
                    competitionRepository.GetLeague2().Id,
                    competitionRepository.GetLeague3().Id,
                    competitionRepository.GetLeague4().Id,
                    competitionRepository.GetNationalCup().Id,
                    competitionRepository.GetNationalSuperCup().Id,
                    competitionRepository.GetFriendly().Id);
            }

            postMatchData.MatchDateTime = _matches.First().Date;

            using (var seasonRepository = _repositoryFactory.CreateSeasonRepository())
            {
                postMatchData.Season = seasonRepository.GetOne(_matches.First().SeasonId);
            }

            // Add all rounds as Dictionary of CompetitionId and Round.
            postMatchData.Rounds = _matches.Select(m => m.Round).GroupBy(r => r.CompetitionId).ToDictionary(g => g.Key, g => g.First());

            // Add all matches as Dictionary of CompetitionId and List of Matches.
            postMatchData.Matches = _matches.GroupBy(m => m.CompetitionId).ToDictionary(g => g.Key, g => g.AsEnumerable());

            // Add all teams as Dictionary of TeamId and Team.
            using (var teamRepository = _repositoryFactory.CreateTeamRepository())
            {
                postMatchData.Teams = teamRepository.GetTeams().ToDictionary(k => k.Id, t => t);
            }

            // Add all league tables as Dictionary of CompetitionId and LeagueTable.
            using (var leagueTableRepository = _repositoryFactory.CreateLeagueTableRepository())
            {
                postMatchData.LeagueTables = leagueTableRepository.GetBySeason(_seasonId).ToDictionary(k => k.SeasonCompetition.CompetitionId, l => l);
            }

            // Add all leagues as Dictionary of CompetitionId and League.
            using (var competitionRepository = _repositoryFactory.CreateCompetitionRepository())
            {
                postMatchData.Leagues = competitionRepository.GetLeagues().ToDictionary(k => k.Id, l => l);
            }

            // Add the managers team.
            using (var gameInfoRepository = _repositoryFactory.CreateGameInfoRepository())
            {
                postMatchData.ManagersTeam = gameInfoRepository.GetGameInfo().CurrentTeam;
            }

            // Add the season statistics.
            using (var seasonStatisticsRepository = _repositoryFactory.CreateSeasonStatisticsRepository())
            {
                postMatchData.SeasonStatistics = seasonStatisticsRepository.GetBySeason(_seasonId);
            }

            // Add the season/team statistics.
            using (var seasonTeamStatisticsRepository = _repositoryFactory.CreateSeasonTeamStatisticsRepository())
            {
                postMatchData.SeasonTeamStatistics = seasonTeamStatisticsRepository
                                                        .GetBySeason(_seasonId)
                                                        .ToDictionary(k => k.TeamId, v => v);
            }

            // Add the team statistics.
            using (var teamStatisticsRepository = _repositoryFactory.CreateTeamStatisticsRepository())
            {
                postMatchData.TeamStatistics = teamStatisticsRepository.GetAll().ToDictionary(k => k.TeamId, v => v);
            }

            // Determine season has ended, all matches have been played in several competitions, etc.
            int numberOfLeagueMatches = (Constants.HowManyTeamsPerLeague - 1) * 2;
            postMatchData.League1Finished = postMatchData.LeagueTableLeague1.LeagueTablePositions.All(x => x.Matches == numberOfLeagueMatches);

            return postMatchData;
        }
    }
}
