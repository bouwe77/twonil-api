using Randomization;
using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Competitions.Friendlies
{
    public class DuringSeasonFriendlyManager : FriendlyManagerBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public DuringSeasonFriendlyManager(IRepositoryFactory repositoryFactory)
            : this(repositoryFactory, new Randomizer(), new NumberRandomizer())
        {
        }

        public DuringSeasonFriendlyManager(IRepositoryFactory repositoryFactory, IRandomizer randomizer, INumberRandomizer numberRandomizer)
            : base(randomizer, numberRandomizer)
        {
            _repositoryFactory = repositoryFactory;
        }

        public IEnumerable<Match> CreateDuringSeasonFriendlies(Round currentCupRound, IEnumerable<Team> teamsInNextCupRound)
        {
            var noMatches = new List<Match>();

            using (var roundRepository = _repositoryFactory.CreateRoundRepository())
            using (var teamRepository = _repositoryFactory.CreateTeamRepository())
            {
                // Try to find the next cup round.
                var nextCupRound = roundRepository.GetNextRound(currentCupRound);
                if (nextCupRound == null)
                    return noMatches;

                // Check for other rounds on the same day.
                var otherRoundsOnSameDay = roundRepository.GetRoundsByMatchDay(nextCupRound.SeasonId, nextCupRound.MatchDate).Except(new[] { nextCupRound });
                if (!otherRoundsOnSameDay.Any())
                    return noMatches;

                // Determine there is a friendly round on that day.
                var friendlyRound = otherRoundsOnSameDay.SingleOrDefault(r => r.CompetitionType == CompetitionType.Friendly);
                if (friendlyRound == null)
                    return noMatches;

                // For now, if there are any other rounds besides the already determined cup and friendly rounds, skip creating friendlies.
                // Later, theses rounds should be checked as well, to prevent the same team plays more than one match in a round.
                if (otherRoundsOnSameDay.Count() > 1)
                    return noMatches;

                // Determine which teams will not play in the next cup round and therefore are available for the next friendly round.
                var teamsAvailableForFriendly = teamRepository.GetAll().Except(teamsInNextCupRound).ToList();
                if (teamsAvailableForFriendly.Count < 2)
                    return noMatches;

                teamsAvailableForFriendly.Shuffle();

                int howManyTeams = _numberRandomizer.GetEvenNumber(2, teamsAvailableForFriendly.Count());

                var friendlyMatches = ArrangeFriendlies(teamsAvailableForFriendly.Take(howManyTeams).ToList(), friendlyRound);

                return friendlyMatches;
            }
        }
    }
}
