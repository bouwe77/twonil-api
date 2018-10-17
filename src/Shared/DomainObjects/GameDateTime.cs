using System;

namespace TwoNil.Shared.DomainObjects
{
    public class GameDateTime : DomainObjectBase
    {
        public DateTime DateTime { get; set; }

        public GameDateTimeStatus Status { get; set; }

        public GameDateTimeEventStatus Matches { get; set; }

        public bool ManagerPlaysMatch { get; set; }

        public GameDateTimeEventStatus EndOfSeason { get; set; }

        public string Date { get; set; }
    }
}
