using Randomization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Competitions.Friendlies
{
    public abstract class FriendlyManagerBase
    {
        protected Competition _competition;
        protected IRandomizer _randomizer;
        protected INumberRandomizer _numberRandomizer;

        public FriendlyManagerBase(IRandomizer randomizer, INumberRandomizer numberRandomizer)
        {
            _randomizer = randomizer;
            _numberRandomizer = numberRandomizer;

            using (var competitionRepository = new RepositoryFactory().CreateCompetitionRepository())
            {
                _competition = competitionRepository.GetFriendly();
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
                matches = new SingleRoundTournamentManager().GetMatches(teams);
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
