using System.Linq;
using Shally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
    public class TeamStatisticsMapper : IResourceMapper<TeamStatistics>
    {
        private readonly UriHelper _uriHelper;
        public static string LeagueTablePositions = "leaguetable-positions";

        public TeamStatisticsMapper(UriHelper uriHelper)
        {
            _uriHelper = uriHelper;
        }

        public Resource Map(TeamStatistics teamStatistics, params string[] properties)
        {
            bool applyAllProperties = !properties.Any();

            var resource = new Resource(new Link(_uriHelper.GetTeamStatsUri(teamStatistics.GameId, teamStatistics.TeamId)));

            if (applyAllProperties || properties.Contains(LeagueTablePositions))
            {
                resource.AddProperty(LeagueTablePositions, teamStatistics.LeagueTablePositions);
            }

            return resource;
        }
    }
}