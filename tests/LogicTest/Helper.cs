using TwoNil.Logic.Functionality.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic
{
   internal class Helper
   {
      public static Team HomeTeam = new Team { Id = "1", Name = "Team 1" };
      public static Team AwayTeam = new Team { Id = "2", Name = "Team 2" };

      public static Match GetValidMatch()
      {
         var match = MatchFactory.CreateMatch(HomeTeam, AwayTeam);
         return match;
      }
   }
}
