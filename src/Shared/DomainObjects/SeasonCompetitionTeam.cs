namespace TwoNil.Shared.DomainObjects
{
   public class SeasonCompetitionTeam : DomainObjectBase
   {
      private SeasonCompetition _seasonCompetition;
      private Team _team;

      public string SeasonCompetitionId { get; set; }
      public string TeamId { get; set; }

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
