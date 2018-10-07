using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoNil.Data;
using TwoNil.Logic.Competitions.Friendlies;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Matches.PostMatches.Handlers
{
    public class GenerateDuringSeasonFriendliesHandler : IPostMatchesHandler
    {
        private readonly DuringSeasonFriendlyManager _duringSeasonFriendlyManager;

        public GenerateDuringSeasonFriendliesHandler(IRepositoryFactory repositoryFactory)
        {
            _duringSeasonFriendlyManager = new DuringSeasonFriendlyManager(repositoryFactory);
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
