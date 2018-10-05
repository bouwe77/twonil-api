using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Competitions;

namespace TwoNil.Logic.Matches.PostMatches
{
    public class DrawNextCupRoundHandler : IPostMatchesHandler
    {
        private readonly NationalCupManager _nationalCupManager;

        public DrawNextCupRoundHandler(IRepositoryFactory repositoryFactory)
        {
            _nationalCupManager = new NationalCupManager(repositoryFactory);
        }

        public void Handle(PostMatchData postMatchData)
        {
            if (!postMatchData.MatchesNationalCup.Any())
                return;

            postMatchData.CupMatchesNextRound = _nationalCupManager.DrawNextRound(postMatchData.RoundNationalCup, postMatchData.MatchesNationalCup, postMatchData.Season);
        }
    }
}
