using System.Linq;
using TwoNil.Logic.Competitions;

namespace TwoNil.Logic.Matches.PostMatches.Handlers
{
    public class DrawNextCupRoundHandler : IPostMatchesHandler
    {
        private readonly INationalCupManager _nationalCupManager;

        public DrawNextCupRoundHandler(INationalCupManager nationalCupManager)
        {
            _nationalCupManager = nationalCupManager;
        }

        public void Handle(PostMatchData postMatchData)
        {
            if (!postMatchData.MatchesNationalCup.Any())
                return;

            postMatchData.CupMatchesNextRound = _nationalCupManager.DrawNextRound(postMatchData.RoundNationalCup, postMatchData.MatchesNationalCup, postMatchData.Season);
        }
    }
}
