using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions
{
    internal class NewSeasonInfo
    {
        public int StartYear { get; set; }
        public List<Team> TeamsLeague1 { get; set; }
        public List<Team> TeamsLeague2 { get; set; }
        public List<Team> TeamsLeague3 { get; set; }
        public List<Team> TeamsLeague4 { get; set; }
        public SeasonStatistics PreviousSeasonStatistics { get; set; }
        public IEnumerable<Team> AllTeams => TeamsLeague1.Concat(TeamsLeague2).Concat(TeamsLeague3).Concat(TeamsLeague4);
    }
}