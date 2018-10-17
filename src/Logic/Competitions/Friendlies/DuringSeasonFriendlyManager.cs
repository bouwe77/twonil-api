﻿using Randomization;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Competitions.Friendlies
{
    public interface IDuringSeasonFriendlyManager
    {
        IEnumerable<Match> CreateDuringSeasonFriendlies(Round currentCupRound, IEnumerable<Team> teamsInNextCupRound);
    }

    public class DuringSeasonFriendlyManager : FriendlyManagerBase, IDuringSeasonFriendlyManager
    {
        private readonly IUnitOfWorkFactory _uowFactory;

        public DuringSeasonFriendlyManager(IUnitOfWorkFactory uowFactory)
            : this(uowFactory, new Randomizer(), new NumberRandomizer())
        {
        }

        public DuringSeasonFriendlyManager(IUnitOfWorkFactory uowFactory, IRandomizer randomizer, INumberRandomizer numberRandomizer)
            : base(uowFactory, randomizer, numberRandomizer)
        {
            _uowFactory = uowFactory;
        }

        public IEnumerable<Match> CreateDuringSeasonFriendlies(Round currentCupRound, IEnumerable<Team> teamsInNextCupRound)
        {
            var noMatches = new List<Match>();

            using (var uow = _uowFactory.Create())
            {
                // Try to find the next cup round.
                var nextCupRound = uow.Rounds.GetNextRound(currentCupRound);
                if (nextCupRound == null)
                    return noMatches;

                // Check for other rounds on the same day.
                var otherRoundsOnSameDay = uow.Rounds.GetRoundsByMatchDay(nextCupRound.SeasonId, nextCupRound.MatchDate).Except(new[] { nextCupRound });
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
                var teamsAvailableForFriendly = uow.Teams.GetAll().Except(teamsInNextCupRound).ToList();
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
