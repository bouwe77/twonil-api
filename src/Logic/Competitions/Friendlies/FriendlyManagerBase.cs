using Randomization;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Competitions.Friendlies
{
    public abstract class FriendlyManagerBase
    {
        protected Competition _competition;
        protected IRandomizer _randomizer;
        protected INumberRandomizer _numberRandomizer;
        private readonly SingleRoundTournamentManager _singleRoundTournamentManager;

        public FriendlyManagerBase(IUnitOfWorkFactory uowFactory, IRandomizer randomizer, INumberRandomizer numberRandomizer, SingleRoundTournamentManager singleRoundTournamentManager)
        {
            _randomizer = randomizer;
            _numberRandomizer = numberRandomizer;
            _singleRoundTournamentManager = singleRoundTournamentManager;
            using (var uow = uowFactory.Create())
            {
                _competition = uow.Competitions.GetFriendly();
            }
        }

        public IEnumerable<Match> ArrangeFriendlies(List<Team> teams, Round round)
        {
            // If the number of teams is not even, remove a team.
            if (teams.Count % 2 != 0)
            {
                teams.RemoveAt(0);
            }

            var matches = new List<Match>();
            if (teams.Any())
            {
                // Create a so-called single round tournament between the teams.
                matches = _singleRoundTournamentManager.GetMatches(teams);
            }

            foreach (var match in matches)
            {
                match.Season = round.Season;
                match.Round = round;
                match.Date = round.MatchDate;
                match.CompetitionId = _competition.Id;
            }

            return matches;
        }
    }
}
