using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions
{
   internal class CompetitionSchedule
   {
      public CompetitionSchedule()
      {
         Rounds = new List<Round>();
         SeasonCompetitions = new List<SeasonCompetition>();
         SeasonCompetitionTeams = new List<SeasonCompetitionTeam>();
         Matches = new List<Match>();
         LeagueTables = new List<LeagueTable>();
      }

      public List<Round> Rounds { get; set; }

      public List<SeasonCompetition> SeasonCompetitions { get; set; }

      public List<SeasonCompetitionTeam> SeasonCompetitionTeams { get; set; }

      public List<Match> Matches { get; set; }

      public List<LeagueTable> LeagueTables { get; set; }
   }
}
