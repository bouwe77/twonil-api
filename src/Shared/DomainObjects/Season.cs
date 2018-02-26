using SQLite;

namespace TwoNil.Shared.DomainObjects
{
   [Table("Seasons")]
   public class Season : DomainObjectBase
   {
      public string Name { get; set; }
      public int GameOrder { get; set; }
      public SeasonStatus SeasonStatus { get; set; }
   }
}
