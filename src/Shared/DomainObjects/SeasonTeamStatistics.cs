﻿namespace TwoNil.Shared.DomainObjects
{
    public class SeasonTeamStatistics : DomainObjectBase
    {
        public SeasonTeamStatistics()
        {
        }

        public SeasonTeamStatistics(Season season, Team team, string leagueName)
        {
            Season = season;
            Team = team;
            CurrentLeagueTablePosition = team.CurrentLeaguePosition;
            LeagueName = leagueName;
        }

        public string MatchResults { get; set; }

        public int CurrentLeagueTablePosition { get; set; }

        public string LeagueTablePositions { get; set; }

        public string LeagueName { get; set; }

        private Season _season;
        public string SeasonId { get; set; }

        public Season Season
        {
            get
            {
                return _season;
            }
            set
            {
                _season = value;
                SeasonId = value?.Id;
            }
        }

        private Team _team;
        public string TeamId { get; set; }

        public Team Team
        {
            get
            {
                return _team;
            }
            set
            {
                _team = value;
                TeamId = value?.Id;
            }
        }
    }
}
