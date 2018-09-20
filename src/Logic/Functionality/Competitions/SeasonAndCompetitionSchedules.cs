using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions
{
    internal class SeasonAndCompetitionSchedules
    {
        public Season Season { get; set; }
        public CompetitionSchedule LeaguesSchedule { get; set; }
        public CompetitionSchedule PreSeasonFriendliesSchedule { get; set; }
        public CompetitionSchedule DuringSeasonFriendliesSchedule { get; set; }
        public CompetitionSchedule NationalCupSchedule { get; set; }
        public CompetitionSchedule NationalSuperCupSchedule { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public IEnumerable<DateTime> MatchDates { get; set; }
        public DateTime EndOfSeasonDate { get; set; }
    }
}
