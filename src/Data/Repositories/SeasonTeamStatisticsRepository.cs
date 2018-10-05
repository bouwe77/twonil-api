using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
    public class SeasonTeamStatisticsRepository : ReadRepository<SeasonTeamStatistics>
    {
        internal SeasonTeamStatisticsRepository(string databaseFilePath, string gameId)
           : base(databaseFilePath, gameId)
        {
        }

        public IEnumerable<SeasonTeamStatistics> GetBySeason(string seasonId)
        {
            var seasonTeamStatistics = Find(x => x.SeasonId == seasonId);

            foreach (var stat in seasonTeamStatistics)
            {
                GetReferencedData(stat);
            }

            return seasonTeamStatistics;
        }

        public SeasonTeamStatistics GetBySeasonAndTeam(string seasonId, string teamId)
        {
            var seasonTeamStatistics = Find(x => x.SeasonId == seasonId && x.TeamId == teamId).FirstOrDefault();

            if (seasonTeamStatistics != null)
            {
                GetReferencedData(seasonTeamStatistics);
            }

            return seasonTeamStatistics;
        }

        private void GetReferencedData(SeasonTeamStatistics seasonTeamStatistics)
        {
            var repositoryFactory = new RepositoryFactory(seasonTeamStatistics.GameId);
            using (var seasonRepository = repositoryFactory.CreateRepository<Season>())
            {
                var season = seasonRepository.GetOne(seasonTeamStatistics.SeasonId);
                seasonTeamStatistics.Season = season;
            }
        }
    }
}
