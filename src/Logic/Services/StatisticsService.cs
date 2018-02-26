using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
   public class StatisticsService : ServiceWithGameBase
   {
      public StatisticsService(GameInfo gameInfo)
         : base(gameInfo)
      {
      }

      public SeasonTeamStatistics GetSeasonTeamStatistics(string seasonId, string teamId)
      {
         using (var repository = RepositoryFactory.CreateSeasonTeamStatisticsRepository())
         {
            var seasonTeamStatistics = repository.GetBySeasonAndTeam(seasonId, teamId);
            return seasonTeamStatistics;
         }
      }

      public SeasonStatistics GetSeasonStatistics(string seasonId)
      {
         using (var repository = RepositoryFactory.CreateSeasonStatisticsRepository())
         {
            var seasonStatistics = repository.GetBySeason(seasonId);
            return seasonStatistics;
         }
      }
   }
}
