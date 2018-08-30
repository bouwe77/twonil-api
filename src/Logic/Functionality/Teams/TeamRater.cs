using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Teams
{
   /// <summary>
   /// Calculates the team's rating per <see cref="Line"/> depending on which players are on which position in the field.
   /// </summary>
   internal class TeamRater
   {
      public static (decimal ratingGoalkeeper, decimal ratingDefence, decimal ratingMidfield, decimal ratingAttack, decimal ratingTeam) GetRating(List<Player> players)
      {
         using (var lineRepository = new RepositoryFactory().CreateLineRepository())
         {
            var goalkeeper = lineRepository.GetGoalkeeper();
            var defence = lineRepository.GetDefence();
            var midfield = lineRepository.GetMidfield();
            var attack = lineRepository.GetAttack();

            decimal ratingGoalkeeper = players.Single(p => p.CurrentPosition.Line.Equals(goalkeeper)).RatingGoalkeeping;
            decimal ratingDefence = players.Where(p => p.CurrentPosition.Line.Equals(defence)).Select(p => p.RatingDefence).Average();
            decimal ratingMidfield = players.Where(p => p.CurrentPosition.Line.Equals(midfield)).Select(p => p.RatingMidfield).Average();
            decimal ratingAttack = players.Where(p => p.CurrentPosition.Line.Equals(attack)).Select(p => p.RatingAttack).Average();

            decimal ratingTeam = (ratingGoalkeeper + ratingDefence + ratingMidfield + ratingAttack) / 4;

            return (ratingGoalkeeper, ratingDefence, ratingMidfield, ratingAttack, ratingTeam);
         }
      }
   }
}
