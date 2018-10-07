namespace TwoNil.Logic.Matches.PostMatches.Handlers
{
    public class TeamHandler : IPostMatchesHandler
    {
        public void Handle(PostMatchData postMatchData)
        {
            UpdateLeagueTablePositions(postMatchData);
        }

        private void UpdateLeagueTablePositions(PostMatchData postMatchData)
        {
            foreach (var leagueTable in postMatchData.LeagueTables.Values)
            {
                foreach (var position in leagueTable.LeagueTablePositions)
                {
                    postMatchData.Teams[position.TeamId].CurrentLeaguePosition = position.Position;
                }
            }
        }
    }
}
