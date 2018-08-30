using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
    public class TeamStatisticsRepository : ReadRepository<TeamStatistics>
    {
        internal TeamStatisticsRepository(string databaseFilePath, string gameId)
           : base(databaseFilePath, gameId)
        {
        }

        public TeamStatistics GetByTeam(string teamId)
        {
            var teamStatistics = Find(x => x.TeamId == teamId).FirstOrDefault();

            if (teamStatistics != null)
            {
                GetReferencedData(teamStatistics);
            }

            return teamStatistics;
        }

        private void GetReferencedData(TeamStatistics teamStatistics)
        {
            var repositoryFactory = new RepositoryFactory(teamStatistics.GameId);
            using (var teamRepository = repositoryFactory.CreateRepository<Team>())
            {
                var team = teamRepository.GetOne(teamStatistics.TeamId);
                teamStatistics.Team = team;
            }
        }
    }
}
