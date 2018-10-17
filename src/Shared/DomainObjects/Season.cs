using System;

namespace TwoNil.Shared.DomainObjects
{
    public class Season : DomainObjectBase
    {
        public int StartYear { get; set; }

        public SeasonStatus SeasonStatus { get; set; }

        public DateTime EndDateTime { get; set; }

        public string LongName { get { return $"{StartYear}/{EndYear}"; } }

        public string ShortName { get { return $"{StartYear.ToString().Substring(2)}/{EndYear.ToString().Substring(2)}"; } }

        public int EndYear { get { return StartYear + 1; } }
    }
}
