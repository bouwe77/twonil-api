using Randomization;
using System;
using System.Collections.Generic;
using System.Text;
using TwoNil.Data;
using TwoNil.Logic.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Competitions.Friendlies
{
    public class DuringSeasonFriendlyRoundsManager : FriendlyManagerBase
    {
        public DuringSeasonFriendlyRoundsManager(IUnitOfWorkFactory unitOfWorkFactory, IRandomizer randomizer, INumberRandomizer numberRandomizer)
            : base(unitOfWorkFactory, randomizer, numberRandomizer)
        {
        }

        public CompetitionSchedule CreateDuringSeasonFriendlyRounds(SeasonCompetition seasonCompetition, List<DateTime> matchDates, int startIndex)
        {
            // The during season friendly schedule only consists of rounds. The matches in the rounds are determined during the season.
            var competitionSchedule = new CompetitionSchedule();

            foreach (var matchDate in matchDates)
            {
                var friendlyRound = RoundFactory.CreateRound(null, seasonCompetition, matchDate, startIndex, _competition);

                competitionSchedule.Rounds.Add(friendlyRound);
                startIndex++;
            }

            return competitionSchedule;
        }
    }
}
