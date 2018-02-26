﻿using System;
using System.Collections.Generic;
using System.Linq;
using Randomization;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions
{
   internal class MatchDateManager
   {
      private readonly INumberRandomizer _numberRandomizer;
      private readonly int _seasonNr;
      private List<DateTime> _leagueDates;
      private List<DateTime> _cupDates;
      private List<DateTime> _friendlyDates;
      private DateTime _superCupDate;
      private bool _initialized = false;

      public MatchDateManager(int seasonNr)
      {
         _numberRandomizer = new NumberRandomizer();
         _seasonNr = seasonNr;
      }

      public void Initialize()
      {
         // Determine the start date of the season.
         var startMatchDate = DetermineSeasonStartMatchDate();

         _friendlyDates = new List<DateTime>();
         _leagueDates = new List<DateTime>();
         _cupDates = new List<DateTime>();

         // Start the season with pre-season friendlies.
         DateTime friendlyDate = startMatchDate;
         for (int i = 0; i < Constants.HowManyPreSeasonFriendlies; i++)
         {
            _friendlyDates.Add(friendlyDate);
            friendlyDate = friendlyDate.AddDays(7);
         }

         // Super Cup.
         var superCupDate = _friendlyDates.Max().AddDays(7);
         _superCupDate = superCupDate;

         // 2 league rounds.
         DateTime leagueDate = superCupDate.AddDays(7);
         for (int i = 0; i < 2; i++)
         {
            _leagueDates.Add(leagueDate);
            leagueDate = leagueDate.AddDays(7);
         }

         // First cup round.
         var cupDate = _leagueDates.Max().AddDays(7);
         _cupDates.Add(cupDate);

         // 1 league round.
         leagueDate = cupDate.AddDays(7);
         _leagueDates.Add(leagueDate);

         // Second cup round.        
         cupDate = _leagueDates.Max().AddDays(7);
         _cupDates.Add(cupDate);

         // 1 league round.
         leagueDate = cupDate.AddDays(7);
         _leagueDates.Add(leagueDate);

         // Third cup round (semi final).
         cupDate = _leagueDates.Max().AddDays(7);
         _cupDates.Add(cupDate);

         // 2 league rounds.
         leagueDate = cupDate.AddDays(7);
         for (int i = 0; i < 2; i++)
         {
            _leagueDates.Add(leagueDate);
            leagueDate = leagueDate.AddDays(7);
         }

         // Cup final.
         cupDate = _leagueDates.Max().AddDays(7);
         _cupDates.Add(cupDate);

         _initialized = true;
      }

      private DateTime DetermineSeasonStartMatchDate()
      {
         // The season starts on a random date somewhere in August.
         // The year is the same as the season number (+1, because the year 0 is not allowed).
         // However, the year should not be displayed in the game.
         int day = _numberRandomizer.GetNumber(7, 14);
         var localDate = new DateTime(year: _seasonNr + 1, month: 8, day: day, hour: 20, minute: 0, second: 0);
         var utcDate = DateTime.SpecifyKind(localDate, DateTimeKind.Utc);

         return utcDate;
      }

      public DateTime GetNextMatchDate(CompetitionType competitionType, int roundIndex)
      {
         if (!_initialized)
         {
            throw new Exception("MatchDateManager is not initialized");
         }

         DateTime nextDateTime;

         switch (competitionType)
         {
            case CompetitionType.Friendly:
               nextDateTime = _friendlyDates[roundIndex];
               break;
            case CompetitionType.League:
               nextDateTime = _leagueDates[roundIndex];
               break;
            case CompetitionType.NationalCup:
               nextDateTime = _cupDates[roundIndex];
               break;
            case CompetitionType.NationalSuperCup:
               nextDateTime = _superCupDate;
               break;
            default:
               throw new ArgumentException("Unknown CompetitionType");
         }

         return nextDateTime;
      }
   }
}
