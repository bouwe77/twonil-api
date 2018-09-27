using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Data.Repositories;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Teams
{
    public class TeamStatisticsManager
    {
        private readonly TransactionManager _transactionManager;
        private readonly IRepositoryFactory _repositoryFactory;

        public TeamStatisticsManager(TransactionManager transactionManager, IRepositoryFactory repositoryFactory)
        {
            _transactionManager = transactionManager;
            _repositoryFactory = repositoryFactory;
        }

        public void Update(IEnumerable<LeagueTable> leagueTables)
        {
            var teamStatistics = new Dictionary<string, TeamStatistics>();
            using (var teamStatisticsRepository = _repositoryFactory.CreateRepository<TeamStatistics>())
            {
                teamStatistics = teamStatisticsRepository.GetAll().ToDictionary(k => k.TeamId, v => v);
            }

            var leagues = new Dictionary<string, League>();
            using (var competitionRepository = _repositoryFactory.CreateCompetitionRepository())
            {
                leagues = competitionRepository.GetLeagues().ToDictionary(k => k.Id, v => v);
            }

            // Loop through league tables and with each position update the team in de dictionary
            foreach (var leagueTable in leagueTables)
            {
                var leagueStatisticsScore = leagues[leagueTable.SeasonCompetition.CompetitionId].StatisticsScore;

                foreach (var position in leagueTable.LeagueTablePositions)
                {
                    var positionScore = leagueStatisticsScore - position.Position;
                    var stat = teamStatistics[position.TeamId];
                    stat.LeagueTablePositions += positionScore + ",";
                }
            }

            // Add all TeamStatistics to the transaction manager because they all have changed.
            foreach (var teamStat in teamStatistics)
            {
                _transactionManager.RegisterUpdate(teamStat.Value);
            }
        }
    }
}
