using System.Linq;
using TwoNil.Logic.Teams;

namespace TwoNil.Logic.Matches.PostMatches.Handlers
{
    public class SeasonTeamStatisticsHandler : IPostMatchesHandler
    {
        public void Handle(PostMatchData postMatchData)
        {
            var seasonTeamStatisticsManager = new SeasonTeamStatisticsManager(postMatchData.SeasonTeamStatistics, postMatchData.Season.Id);
            seasonTeamStatisticsManager.Update(postMatchData.Season.Id, postMatchData.Matches.Values.SelectMany(x => x), postMatchData.LeagueTables.Values);
        }
    }
}
