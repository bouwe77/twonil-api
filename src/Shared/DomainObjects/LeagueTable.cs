using System.Collections.Generic;

namespace TwoNil.Shared.DomainObjects
{
    public class LeagueTable : DomainObjectBase
    {
        private SeasonCompetition _seasonCompetition;

        public List<LeagueTablePosition> LeagueTablePositions { get; set; }

        public string SeasonCompetitionId { get; set; }
        public string CompetitionName { get; set; }
        public string SeasonId { get; set; }

        public SeasonCompetition SeasonCompetition
        {
            get
            {
                return _seasonCompetition;
            }
            set
            {
                _seasonCompetition = value;
                SeasonCompetitionId = value?.Id;
            }
        }
    }
}
