using System;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Calendar
{
    public class GameDateTimeFactory
    {
        public static GameDateTime CreateForOtherTeamsMatches(DateTime dateTime)
        {
            var gameDateTime = CreateWithoutEvents(dateTime);
            gameDateTime.Matches = GameDateTimeEventStatus.ToDo;

            return gameDateTime;
        }

        public static GameDateTime CreateForManagersMatches(DateTime dateTime)
        {
            var gameDateTime = CreateForOtherTeamsMatches(dateTime);
            gameDateTime.ManagerPlaysMatch = true;

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
                ManagerPlaysMatch = false,
                EndOfSeason = GameDateTimeEventStatus.NotApplicable
            };
        }
    }
}
