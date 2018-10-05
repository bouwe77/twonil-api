using System;
using System.Collections.Generic;
using System.Text;
using TwoNil.Data;
using TwoNil.Logic.Teams;

namespace TwoNil.Logic.Matches.PostMatches
{
    public class SeasonTeamStatisticsHandler : IPostMatchesHandler
    {
        public void Handle(PostMatchData postMatchData)
        {
            //var seasonTeamStatisticsManager = new SeasonTeamStatisticsManager(postMatchData.SeasonTeamStatistics, postMatchData.Season.Id);
            //seasonTeamStatisticsManager.Update(postMatchData.Matches.Values);
        }
    }
}
