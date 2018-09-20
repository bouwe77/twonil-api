using System;
using System.Collections.Generic;
using System.Text;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Calendar
{
    internal class GameDateTimeFactory
    {
        public static GameDateTime Create(DateTime dateTime, GameDateTimeEventStatus eventStatus)
        {
            return new GameDateTime
            {
                DateTime = dateTime,
                Date = dateTime.ToString("yyyy-MM-dd"),
                Matches = eventStatus,
                Status = GameDateTimeStatus.Future
            };
        }
    }
}
