using SQLite;

namespace TwoNil.Shared.DomainObjects
{
   [Table("SeasonCompetitionTeam")]
   public class SeasonCompetitionTeam : DomainObjectBase
   {
      private SeasonCompetition _seasonCompetition;
      private Team _team;

      public string SeasonCompetitionId { get; private set; }
      public string TeamId { get; private set; }

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
            SeasonCompetitionId = value != null ? value.Id : null;
         }
      }

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
            TeamId = value != null ? value.Id : null;
         }
      }
   }
}
