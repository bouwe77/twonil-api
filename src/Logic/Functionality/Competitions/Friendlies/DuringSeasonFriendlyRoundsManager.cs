﻿using Randomization;
using System;
using System.Collections.Generic;
using System.Text;
using TwoNil.Logic.Functionality.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions.Friendlies
{
    internal class DuringSeasonFriendlyRoundsManager : FriendlyManagerBase
    {
        public DuringSeasonFriendlyRoundsManager()
            : base(new Randomizer(), new NumberRandomizer())
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
