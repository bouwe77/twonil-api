using Randomization;
using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Functionality.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions.Friendlies
{
    internal class DuringSeasonFriendlyManager : FriendlyManagerBase
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ITransactionManager _transactionManager;

        public DuringSeasonFriendlyManager(IRepositoryFactory repositoryFactory, ITransactionManager transactionManager)
            : this(repositoryFactory, transactionManager, new Randomizer(), new NumberRandomizer())
        {
        }

        public DuringSeasonFriendlyManager(IRepositoryFactory repositoryFactory, ITransactionManager transactionManager, IRandomizer randomizer, INumberRandomizer numberRandomizer)
            : base(randomizer, numberRandomizer)
        {
            _repositoryFactory = repositoryFactory;
            _transactionManager = transactionManager;
        }

        public void CreateDuringSeasonFriendlies(Round currentCupRound, IEnumerable<Team> teamsInNextCupRound)
        {
            //TODO Kan ik niet i.p.v. currentCupRound de NEXT cup round meegeven aan deze methode?
            //TODO Deze methode moet geunittest worden, dus verplaatsen...

            // Determine next cup round.
            using (var roundRepository = _repositoryFactory.CreateRoundRepository())
            using (var teamRepository = _repositoryFactory.CreateTeamRepository())
            {
                // Try to find the next cup round.
                var nextCupRound = roundRepository.GetNextRound(currentCupRound);
                if (nextCupRound == null)
                    return;

                // Check for other rounds on the same day.
                var otherRoundsOnSameDay = roundRepository.GetRoundsByMatchDay(nextCupRound.SeasonId, nextCupRound.MatchDate).Except(new[] { nextCupRound });
                if (!otherRoundsOnSameDay.Any())
                    return;

                // Determine there is a friendly round on that day.
                var friendlyRound = otherRoundsOnSameDay.SingleOrDefault(r => r.CompetitionType == CompetitionType.Friendly);
                if (friendlyRound == null)
                    return;

                // For now, if there are any other rounds besides the already determined cup and friendly rounds, skip creating friendlies.
                // Later, theses rounds should be checked as well, to prevent the same team plays more than one match in a round.
                if (otherRoundsOnSameDay.Count() > 1)
                    return;

                // Determine which teams will not play in the next cup round and therefore are available for the next friendly round.
                var teamsAvailableForFriendly = teamRepository.GetAll().Except(teamsInNextCupRound).ToList();
                if (teamsAvailableForFriendly.Count < 2)
                    return;

                teamsAvailableForFriendly.Shuffle();

                int howManyTeams = _numberRandomizer.GetEvenNumber(2, teamsAvailableForFriendly.Count());

                var friendlyMatches = ArrangeFriendlies(teamsAvailableForFriendly.Take(howManyTeams).ToList(), friendlyRound);

                _transactionManager.RegisterInsert(friendlyMatches);
            }
        }
    }
}
