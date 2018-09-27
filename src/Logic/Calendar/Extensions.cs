using System;
using System.Collections.Generic;
using System.Text;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Calendar
{
    public static class Extensions
    {
        public static bool CanNavigateToNext(this GameDateTime gameDateTime)
        {
            if (gameDateTime.Status != GameDateTimeStatus.Now)
                return false;

            bool nothingOnToDo = gameDateTime.Matches != GameDateTimeEventStatus.ToDo
                                 && gameDateTime.EndOfSeason != GameDateTimeEventStatus.ToDo;

            if (nothingOnToDo)
                return true;

            bool thereAreMatchesButNotForTheManager = gameDateTime.EndOfSeason == GameDateTimeEventStatus.NotApplicable
                                                      && gameDateTime.Matches == GameDateTimeEventStatus.ToDo
                                                      && !gameDateTime.ManagerPlaysMatch;

            if (thereAreMatchesButNotForTheManager)
                return true;

            return false;
        }
    }
}
