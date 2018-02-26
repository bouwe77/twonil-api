using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Matches
{
   internal class PostMatchData
   {
      public PostMatchData()
      {
         LeagueTables = new List<LeagueTable>();
         Teams = new List<Team>();
         SeasonTeamStatistics = new List<SeasonTeamStatistics>();
      }

      public List<LeagueTable> LeagueTables { get; set; }
      public List<SeasonTeamStatistics> SeasonTeamStatistics { get; set; }
      public List<Team> Teams { get; set; }
      //etc.
   }
}
