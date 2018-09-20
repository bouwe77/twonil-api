using SQLite;

namespace TwoNil.Shared.DomainObjects
{
   [Table("SeasonCompetition")]
   public class SeasonCompetition : DomainObjectBase
   {
      private Competition _competition;
      private Season _season;

      public string CompetitionId { get; set; }
      public string SeasonId { get; set; }

      [Ignore]
      public Competition Competition
      {
         get
         {
            return _competition;
         }
         set
         {
            _competition = value;
            CompetitionId = value != null ? value.Id : null;
         }
      }

      [Ignore]
      public Season Season
      {
         get
         {
            return _season;
         }
         set
         {
            _season = value;
            SeasonId = value != null ? value.Id : null;
         }
      }
   }
}
