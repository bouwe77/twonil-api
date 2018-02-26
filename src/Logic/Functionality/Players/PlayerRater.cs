using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Players
{
   /// <summary>
   /// Calculates the player's rating depending on his position and skills.
   /// </summary>
   internal class PlayerRater
   {
      public static decimal GetRating(Player player)
      {
         // Determine rating depending on whether the player is a goalkeeper or field player.
         var rating = player.PreferredPosition.Line.IsField ? DetermineFieldPlayerRating(player) : DetermineGoalkeeperRating(player);
         return rating;
      }

      private static decimal DetermineGoalkeeperRating(Player player)
      {
         return player.SkillGoalkeeping;
      }

      private static decimal DetermineFieldPlayerRating(Player player)
      {
         // Get the average of all football-related field skills
         var average = (player.SkillDefending + player.SkillHeading + player.SkillPassing + player.SkillShooting + player.SkillSpeed + player.SkillTechnique) / 6;
         return average;
      }
   }
}
