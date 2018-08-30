using System.Collections.Generic;
using SQLite;

namespace TwoNil.Shared.DomainObjects
{
    [Table("LeagueTables")]
    public class LeagueTable : DomainObjectBase
    {
        private SeasonCompetition _seasonCompetition;

        [Ignore]
        public List<LeagueTablePosition> LeagueTablePositions { get; set; }

        public string SeasonCompetitionId { get; private set; }
        public string CompetitionName { get; set; }
        public string SeasonId { get; set; }

        [Ignore]
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
