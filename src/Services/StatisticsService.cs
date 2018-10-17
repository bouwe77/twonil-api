using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
    public class StatisticsService : ServiceWithGameBase
    {
        public StatisticsService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo)
           : base(uowFactory, gameInfo)
        {
        }

        public SeasonTeamStatistics GetSeasonTeamStatistics(string seasonId, string teamId)
        {
            using (var uow = UowFactory.Create())
            {
                var seasonTeamStatistics = uow.SeasonTeamStatistics.GetBySeasonAndTeam(seasonId, teamId);
                return seasonTeamStatistics;
            }
        }

        public SeasonStatistics GetSeasonStatistics(string seasonId)
        {
            using (var uow = UowFactory.Create())
            {
                var seasonStatistics = uow.SeasonStatics.GetBySeason(seasonId);
                return seasonStatistics;
            }
        }

        public TeamStatistics GetTeamStatistics(string teamId)
        {
            using (var uow = UowFactory.Create())
            {
                var teamStatistics = uow.TeamStatistics.GetByTeam(teamId);
                return teamStatistics;
            }
        }
    }
}
