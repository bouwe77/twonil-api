﻿using System.Linq;
using TwoNil.Logic.Competitions;

namespace TwoNil.Logic.Matches.PostMatches.Handlers
{
    public class LeagueTableHandler : IPostMatchesHandler
    {
        private readonly ILeagueTableManager _leagueTableManager;

        public LeagueTableHandler(ILeagueTableManager leagueTableManager)
        {
            _leagueTableManager = leagueTableManager;
        }

        public void Handle(PostMatchData postMatchData)
        {
            if (postMatchData.MatchesLeague1.Any())
                _leagueTableManager.UpdateLeagueTable(postMatchData.LeagueTableLeague1, postMatchData.MatchesLeague1);

            if (postMatchData.MatchesLeague2.Any())
                _leagueTableManager.UpdateLeagueTable(postMatchData.LeagueTableLeague2, postMatchData.MatchesLeague2);

            if (postMatchData.MatchesLeague3.Any())
                _leagueTableManager.UpdateLeagueTable(postMatchData.LeagueTableLeague3, postMatchData.MatchesLeague3);

            if (postMatchData.MatchesLeague4.Any())
                _leagueTableManager.UpdateLeagueTable(postMatchData.LeagueTableLeague4, postMatchData.MatchesLeague4);
        }
    }
}
