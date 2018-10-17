using System.Linq;
using TwoNil.Logic.Competitions.Friendlies;

namespace TwoNil.Logic.Matches.PostMatches.Handlers
{
    public class GenerateDuringSeasonFriendliesHandler : IPostMatchesHandler
    {
        private readonly IDuringSeasonFriendlyManager _duringSeasonFriendlyManager;

        public GenerateDuringSeasonFriendliesHandler(IDuringSeasonFriendlyManager duringSeasonFriendlyManager)
        {
            _duringSeasonFriendlyManager = duringSeasonFriendlyManager;
        }

        public void Handle(PostMatchData postMatchData)
        {
            if (postMatchData.NationalCupFinalHasBeenPlayed || !postMatchData.CupMatchesNextRound.Any())
                return;

            // During the next cup round there also might be a friendly round. If so, generate friendly matches.
            var teamsInNextCupRound = postMatchData.CupMatchesNextRound.Select(m => m.HomeTeam).ToList();
            teamsInNextCupRound.AddRange(postMatchData.CupMatchesNextRound.Select(m => m.AwayTeam));

            postMatchData.DuringSeasonFriendlies = _duringSeasonFriendlyManager.CreateDuringSeasonFriendlies(postMatchData.RoundNationalCup, teamsInNextCupRound);
        }
    }
}
