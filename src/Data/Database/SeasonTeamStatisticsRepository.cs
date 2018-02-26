using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Database
{
   public class SeasonTeamStatisticsRepository : ReadRepository<SeasonTeamStatistics>
   {
      internal SeasonTeamStatisticsRepository(string databaseFilePath, string gameId)
         : base(databaseFilePath, gameId)
      {
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
         var databaseRepositoryFactory = new DatabaseRepositoryFactory(seasonTeamStatistics.GameId);
         using (var seasonRepository = databaseRepositoryFactory.CreateRepository<Season>())
         {
            var season = seasonRepository.GetOne(seasonTeamStatistics.SeasonId);
            seasonTeamStatistics.Season = season;
         }
      }
   }
}
