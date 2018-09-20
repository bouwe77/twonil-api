using System.Linq;
using Shally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
    public class SeasonTeamStatisticsMapper : IResourceMapper<SeasonTeamStatistics>
    {
        private readonly UriHelper _uriHelper;
        public static string SeasonShortName = "season-short-name";
        public static string SeasonLongName = "season-long-name";
        public static string CurrentLeagueTablePosition = "league-position";
        public static string LeagueName = "league-name";
        public static string LeagueTablePositions = "league-positions-history";
        public static string MatchResults = "league-results-history";

        public SeasonTeamStatisticsMapper(UriHelper uriHelper)
        {
            _uriHelper = uriHelper;
        }

        public Resource Map(SeasonTeamStatistics seasonTeamStatistics, params string[] properties)
        {
            bool applyAllProperties = !properties.Any();

            var resource = new Resource(new Link(_uriHelper.GetSeasonTeamStatsUri(seasonTeamStatistics.GameId, seasonTeamStatistics.SeasonId, seasonTeamStatistics.TeamId)));

            if (applyAllProperties || properties.Contains(SeasonShortName))
            {
                resource.AddProperty(SeasonShortName, seasonTeamStatistics.Season.ShortName);
            }

            if (applyAllProperties || properties.Contains(SeasonLongName))
            {
                resource.AddProperty(SeasonLongName, seasonTeamStatistics.Season.LongName);
            }

            if (applyAllProperties || properties.Contains(CurrentLeagueTablePosition))
            {
                resource.AddProperty(CurrentLeagueTablePosition, seasonTeamStatistics.CurrentLeagueTablePosition);
            }

            if (applyAllProperties || properties.Contains(LeagueTablePositions))
            {
                resource.AddProperty(LeagueTablePositions, seasonTeamStatistics.LeagueTablePositions);
            }

            if (applyAllProperties || properties.Contains(LeagueName))
            {
                resource.AddProperty(LeagueName, seasonTeamStatistics.LeagueName);
            }

            if (applyAllProperties || properties.Contains(MatchResults))
            {
                resource.AddProperty(MatchResults, seasonTeamStatistics.MatchResults);
            }

            return resource;
        }
    }
}