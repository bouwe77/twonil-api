using SQLite;

namespace TwoNil.Shared.DomainObjects
{
   [Table("LeagueTablePositions")]
   public class LeagueTablePosition : DomainObjectBase
   {
      private Team _team;
      public string TeamId { get; private set; }

      [Ignore]
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

      private LeagueTable _leagueTable;
      public string LeagueTableId { get; private set; }

      [Ignore]
      public LeagueTable LeagueTable
      {
         get
         {
            return _leagueTable;
         }
         set
         {
            _leagueTable = value;
            LeagueTableId = value?.Id;
         }
      }

      public int Position { get; set; }
      public int Matches { get; set; }
      public int Points { get; set; }
      public int Wins { get; set; }
      public int Losses { get; set; }
      public int Draws { get; set; }
      public int GoalsScored { get; set; }
      public int GoalsConceded { get; set; }
      public int GoalDifference { get; set; }
   }
}
