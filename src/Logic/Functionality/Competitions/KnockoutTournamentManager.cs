using System;
using System.Collections.Generic;
using System.Linq;
using Randomization;
using TwoNil.Logic.Functionality.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions
{
   internal class KnockoutTournamentManager
   {
      /// <summary>
      /// Creates a schedule for the first round.
      /// </summary>
      /// <param name="teams"></param>
      /// <returns></returns>
      public Dictionary<int, List<Match>> GetSchedule(List<Team> teams)
      {
         var possible = new[] { 2, 4, 8, 16, 32, 64, 128, 256 };
         bool argumentsValid = possible.Contains(teams.Count);
         if (!argumentsValid)
         {
            throw new Exception("Unexpected number of teams");
         }

         teams.Shuffle();

         // Draw the first round, the other rounds will be drawn later.
         var schedule = new Dictionary<int, List<Match>>();
         schedule[0] = new List<Match>();
         int numberOfMatches = teams.Count / 2;

         int teamIndex = 0;
         for (int i = 0; i < numberOfMatches; i++)
         {
            var homeTeam = teams[teamIndex];
            var awayTeam = teams[teamIndex + 1];
            var match = MatchFactory.CreateMatch(homeTeam, awayTeam);
            schedule[0].Add(match);
            teamIndex += 2;
         }

         return schedule;
      }

      public List<Match> DrawNextRound(List<Team> winnersPreviousRound)
      {
         var matchesNextRound = new List<Match>();
         
         winnersPreviousRound.Shuffle();

         int numberOfMatches = winnersPreviousRound.Count / 2;
         int teamIndex = 0;
         for (int i = 0; i < numberOfMatches; i++)
         {
            var homeTeam = winnersPreviousRound[teamIndex];
            var awayTeam = winnersPreviousRound[teamIndex + 1];
            var match = MatchFactory.CreateMatch(homeTeam, awayTeam);
            matchesNextRound.Add(match);
            teamIndex += 2;
         }

         return matchesNextRound;
      }
   }
}
