using SQLite;

namespace TwoNil.Shared.DomainObjects
{
    [Table("Seasons")]
    public class Season : DomainObjectBase
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public int StartYear { get; set; }
        public SeasonStatus SeasonStatus { get; set; }
    }
}
