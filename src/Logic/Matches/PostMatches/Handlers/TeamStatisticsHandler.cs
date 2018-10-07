using TwoNil.Logic.Teams;

namespace TwoNil.Logic.Matches.PostMatches.Handlers
{
    public class TeamStatisticsHandler : IPostMatchesHandler
    {
        public void Handle(PostMatchData postMatchData)
        {
            var teamStatisticsManager = new TeamStatisticsManager(postMatchData.TeamStatistics);

            // Update the season team statistics for this season.
            teamStatisticsManager.Update(postMatchData.LeagueTables.Values, postMatchData.Leagues);
        }
    }
}
