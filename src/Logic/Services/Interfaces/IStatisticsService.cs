using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   public interface IStatisticsService
   {
      SeasonTeamStatistics GetSeasonTeamStatistics(string seasonId, string teamId);
      SeasonStatistics GetSeasonStatistics(string seasonId);
   }
}
