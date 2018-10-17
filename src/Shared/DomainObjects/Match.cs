using System;

namespace TwoNil.Shared.DomainObjects
{
    public class Match : DomainObjectBase
    {
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        public DateTime Date { get; set; }

        public string HomeTeamId { get; set; }

        public string AwayTeamId { get; set; }

        public string RoundId { get; set; }

        public string SeasonId { get; set; }

        public string CompetitionId { get; set; }
        public bool DrawPermitted { get; set; }
        public bool PenaltiesTaken { get; set; }
        public int HomePenaltyScore { get; set; }
        public int AwayPenaltyScore { get; set; }
        public MatchStatus MatchStatus { get; set; }

        public bool Played
        {
            get
            {
                return MatchStatus == MatchStatus.Ended;
            }
        }

        private Team _homeTeam;
        public Team HomeTeam
        {
            get
            {
                return _homeTeam;
            }
            set
            {
                _homeTeam = value;
                HomeTeamId = value != null ? value.Id : null;
            }
        }

        private Team _awayTeam;
        public Team AwayTeam
        {
            get
            {
                return _awayTeam;
            }
            set
            {
                _awayTeam = value;
                AwayTeamId = value != null ? value.Id : null;
            }
        }

        private Round _round;
        public Round Round
        {
            get
            {
                return _round;
            }
            set
            {
                _round = value;
                RoundId = value != null ? value.Id : null;
            }
        }

        private Season _season;
        public Season Season
        {
            get
            {
                return _season;
            }
            set
            {
                _season = value;
                SeasonId = value != null ? value.Id : null;
            }
        }
    }
}
