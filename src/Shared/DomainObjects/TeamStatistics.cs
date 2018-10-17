namespace TwoNil.Shared.DomainObjects
{
    public class TeamStatistics : DomainObjectBase
    {
        public TeamStatistics()
        {
        }

        public TeamStatistics(Team team)
        {
            Team = team;
        }

        public string LeagueTablePositions { get; set; }

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
