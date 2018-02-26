using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Data.Database;
using TwoNil.Logic.Functionality.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Teams
{
   internal class SeasonTeamStatisticsManager
   {
      private readonly DatabaseRepositoryFactory _repositoryFactory;
      private readonly TransactionManager _repository;
      private readonly IDictionary<string, SeasonTeamStatistics> _seasonTeamStatistics;

      public SeasonTeamStatisticsManager(TransactionManager repository, DatabaseRepositoryFactory repositoryFactory, string seasonId)
      {
         _repository = repository;
         _repositoryFactory = repositoryFactory;

         // Add all SeasonTeamStatistics of the season to a dictionary on TeamId.
         using (var seasonTeamStatsticsRepository = _repositoryFactory.CreateRepository<SeasonTeamStatistics>())
         {
            _seasonTeamStatistics = new Dictionary<string, SeasonTeamStatistics>();
            var seasonTeamStatistics = seasonTeamStatsticsRepository.Find(x => x.SeasonId == seasonId);
            foreach (var stt in seasonTeamStatistics)
            {
               _seasonTeamStatistics.Add(stt.TeamId, stt);
            }
         }
      }

      public void Update(string seasonId, IEnumerable<Match> matches, LeagueTable leagueTable)
      {
         foreach (var match in matches)
         {
            UpdateTeamStatistics(seasonId, match);
         }

         UpdateLeagueTablePositions(seasonId, leagueTable);

         // Register all SeasonTeamStatistics for update because they all have changed.
         foreach (var sts in _seasonTeamStatistics)
         {
            _repository.RegisterUpdate(sts.Value);
         }
      }

      private void UpdateTeamStatistics(string seasonId, Match match)
      {
         if (match.MatchStatus == MatchStatus.Ended)
         {
            char homeMatchResult = 'D';
            char awayMatchResult = 'D';

            if (!match.EndedInDraw())
            {
               if (match.GetWinner().Id == _seasonTeamStatistics[match.HomeTeamId].TeamId)
               {
                  homeMatchResult = 'W';
                  awayMatchResult = 'L';
               }
               else
               {
                  homeMatchResult = 'L';
                  awayMatchResult = 'W';
               }
            }

            _seasonTeamStatistics[match.HomeTeamId].MatchResults += homeMatchResult + ",";
            _seasonTeamStatistics[match.AwayTeamId].MatchResults += awayMatchResult + ",";
         }
      }

      private void UpdateLeagueTablePositions(string seasonId, LeagueTable leagueTable)
      {
         var teams = leagueTable.LeagueTablePositions.Select(x => x.Team);

         foreach (var team in teams)
         {
            var teamStatistics = _seasonTeamStatistics[team.Id];
            teamStatistics.CurrentLeagueTablePosition = team.CurrentLeaguePosition;
            teamStatistics.LeagueTablePositions += team.CurrentLeaguePosition + ",";
         }
      }
   }
}