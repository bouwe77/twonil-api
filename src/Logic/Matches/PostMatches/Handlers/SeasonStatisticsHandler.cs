using System.Linq;

namespace TwoNil.Logic.Matches.PostMatches.Handlers
{
    public class SeasonStatisticsHandler : IPostMatchesHandler
    {
        public void Handle(PostMatchData postMatchData)
        {
            if (postMatchData.League1Finished)
                UpdateNationalChampion(postMatchData);

            if (postMatchData.NationalCupFinalHasBeenPlayed)
                UpdateNationalCupWinner(postMatchData);

            if (postMatchData.MatchesNationalSuperCup.Any())
                UpdateNationalSuperCupWinner(postMatchData);
        }

        private void UpdateNationalChampion(PostMatchData postMatchData)
        {
            postMatchData.SeasonStatistics.NationalChampion = postMatchData.LeagueTableLeague1.LeagueTablePositions[0].Team;
            postMatchData.SeasonStatistics.NationalChampionRunnerUp = postMatchData.LeagueTableLeague1.LeagueTablePositions[1].Team;
        }

        private void UpdateNationalCupWinner(PostMatchData postMatchData)
        {
            var cupFinal = postMatchData.MatchesNationalCup.Single();

            postMatchData.SeasonStatistics.CupWinner = cupFinal.GetWinner();
            postMatchData.SeasonStatistics.CupRunnerUp = cupFinal.GetLoser();
        }

        private void UpdateNationalSuperCupWinner(PostMatchData postMatchData)
        {
            var match = postMatchData.MatchesNationalSuperCup.Single();
            var winner = match.GetWinner();

            postMatchData.SeasonStatistics.NationalSuperCupWinner = winner;
        }
    }
}
