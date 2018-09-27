using System;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Matches
{
   public class MatchFactory
   {
      public static Match CreateMatch(Team homeTeam, Team awayTeam)
      {
         if (homeTeam == null || awayTeam == null)
         {
            throw new Exception("Home and away team can not be null");
         }

         if (homeTeam.Equals(awayTeam))
         {
            throw new Exception("Home and away team can not be the same");
         }

         var match = new Match
         {
            DrawPermitted = true,
            HomeTeam = homeTeam,
            AwayTeam = awayTeam,
            MatchStatus = MatchStatus.NotStarted
         };

         return match;
      }
   }
}
