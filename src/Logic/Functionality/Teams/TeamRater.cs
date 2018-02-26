using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Teams
{
   /// <summary>
   /// Calculates the team's rating per <see cref="Line"/> depending on which players are on which position in the field.
   /// </summary>
   internal class TeamRater
   {
      /// <summary>
      /// Gets the average rating for the given players.
      /// These players are typically all players in a specific line.
      /// </summary>
      /// <param name="players"></param>
      /// <returns></returns>
      public static decimal GetRating(IEnumerable<Player> players)
      {
         // For now, team rating is the average rating of all players.
         var average = players.Average(player => player.Rating);
         return average;
      }
   }
}
