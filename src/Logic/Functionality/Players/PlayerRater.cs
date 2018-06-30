using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Players
{
   /// <summary>
   /// Calculates the player's rating depending on his position and skills.
   /// </summary>
   internal class PlayerRater
   {
      public static (decimal rating, decimal ratingGoalkeeping, decimal ratingDefence, decimal ratingMidfield, decimal ratingAttack) GetRating(Player player)
      {
         decimal ratingGoalkeeping = DetermineGoalkeeperRating(player);
         decimal ratingDefence = DetermineDefenceRating(player);
         decimal ratingMidfield = DetermineMidfieldRating(player);
         decimal ratingAttack = DetermineAttackRating(player);

         decimal rating = (ratingGoalkeeping + ratingDefence + ratingMidfield + ratingAttack) / 4;

         return (rating, ratingGoalkeeping, ratingDefence, ratingMidfield, ratingAttack);
      }

      private static decimal DetermineGoalkeeperRating(Player player)
      {
         return 
            (player.SkillGoalkeeping*5 +
             player.SkillDefending*3 +
             player.SkillPassing*2 +
             player.SkillSpeed*1 +
             player.SkillTechnique*1 +
             player.SkillShooting*2 +
             player.SkillHeading*2)
            /16;
      }

      private static decimal DetermineDefenceRating(Player player)
      {
         return
            (player.SkillGoalkeeping * 1 +
             player.SkillDefending * 4 +
             player.SkillPassing * 2 +
             player.SkillSpeed * 2 +
             player.SkillTechnique * 1 +
             player.SkillShooting * 1 +
             player.SkillHeading * 3)
            / 14;
      }

      private static decimal DetermineMidfieldRating(Player player)
      {
         return
            (player.SkillGoalkeeping * 1 +
             player.SkillDefending * 2 +
             player.SkillPassing * 4 +
             player.SkillSpeed * 2 +
             player.SkillTechnique * 2 +
             player.SkillShooting * 1 +
             player.SkillHeading * 1)
            / 13;
      }

      private static decimal DetermineAttackRating(Player player)
      {
         return
            (player.SkillGoalkeeping * 1 +
             player.SkillDefending * 1 +
             player.SkillPassing * 2 +
             player.SkillSpeed * 3 +
             player.SkillTechnique * 3 +
             player.SkillShooting * 4 +
             player.SkillHeading * 2)
            / 16;
      }
   }
}
