using SQLite;
using System;

namespace TwoNil.Shared.DomainObjects
{
    [Table("Seasons")]
    public class Season : DomainObjectBase
    {
        public int StartYear { get; set; }

        public SeasonStatus SeasonStatus { get; set; }

        public DateTime EndDateTime { get; set; }

        [Ignore]
        public string LongName { get { return $"{StartYear}/{EndYear}"; } }

        [Ignore]
        public string ShortName { get { return $"{StartYear.ToString().Substring(2)}/{EndYear.ToString().Substring(2)}"; } }

        [Ignore]
        public int EndYear { get { return StartYear + 1; } }
    }
}
