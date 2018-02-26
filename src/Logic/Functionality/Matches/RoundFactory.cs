using System;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Matches
{
   internal class RoundFactory
   {
      public static Round CreateRound(string name, SeasonCompetition seasonCompetition, DateTime matchDate, int order, Competition competition)
      {
         AssertNotNull(seasonCompetition, nameof(seasonCompetition));
         AssertNotNull(seasonCompetition.Season, nameof(seasonCompetition.Season));
         AssertNotNull(matchDate, nameof(matchDate));
         AssertNotNull(competition, nameof(competition));

         var round = new Round
         {
            Name = name,
            SeasonCompetition = seasonCompetition,
            Season = seasonCompetition.Season,
            Order = order,
            CompetitionId = competition.Id,
            CompetitionName = competition.Name,
            CompetitionType = competition.CompetitionType,
            MatchDate = matchDate
         };

         return round;
      }

      private static void AssertNotNull(object obj, string name)
      {
         if (obj == null) throw new ArgumentException($"{name} can not be null");
      }
   }
}
