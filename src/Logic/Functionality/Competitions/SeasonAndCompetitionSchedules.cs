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

        public IEnumerable<Match> AllMatches
        {
            get
            {
                return LeaguesSchedule.Matches
                        .Concat(PreSeasonFriendliesSchedule.Matches)
                        .Concat(NationalCupSchedule.Matches)
                        .Concat(NationalSuperCupSchedule.Matches);
            }
        }

        public IEnumerable<DateTime> AllMatchDates
        {
            get
            {
                return LeaguesSchedule.Rounds.Select(r => r.MatchDate)
                            .Concat(PreSeasonFriendliesSchedule.Rounds.Select(r => r.MatchDate))
                            .Concat(DuringSeasonFriendliesSchedule.Rounds.Select(r => r.MatchDate))
                            .Concat(NationalCupSchedule.Rounds.Select(r => r.MatchDate))
                            .Concat(NationalSuperCupSchedule.Rounds.Select(r => r.MatchDate));
            }
        }
    }
}
