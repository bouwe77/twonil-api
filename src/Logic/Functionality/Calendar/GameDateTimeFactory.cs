using System;
using System.Collections.Generic;
using System.Text;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Calendar
{
    internal class GameDateTimeFactory
    {
        public static GameDateTime CreateForMatches(DateTime dateTime)
        {
            var gameDateTime = CreateWithoutEvents(dateTime);

            gameDateTime.Matches = GameDateTimeEventStatus.ToDo;

            return gameDateTime;
        }

        public static GameDateTime CreateForEndOfSeason(DateTime dateTime)
        {
            var gameDateTime = CreateWithoutEvents(dateTime);

            gameDateTime.EndOfSeason = GameDateTimeEventStatus.ToDo;

            return gameDateTime;
        }

        public static GameDateTime CreateWithoutEvents(DateTime dateTime)
        {
            return new GameDateTime
            {
                DateTime = dateTime,
                Date = dateTime.ToString("yyyy-MM-dd"),
                Status = GameDateTimeStatus.Future,
                Matches = GameDateTimeEventStatus.NotApplicable,
                EndOfSeason = GameDateTimeEventStatus.NotApplicable
            };
        }
    }
}
