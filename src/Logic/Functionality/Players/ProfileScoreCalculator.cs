using System.Collections.Generic;
using System.Linq;
using Randomization;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Players
{
   internal class ProfileScoreCalculator
   {
      private static readonly Randomizer Randomizer = new Randomizer();

      public List<PlayerSkillScore> Calculate(int startNumberR1, PlayerProfile profile, int age)
      {
         const ProfileSkillPriority profileSkillPriorityPrimary = ProfileSkillPriority.Primary;
         const ProfileSkillPriority profileSkillPrioritySecondary = ProfileSkillPriority.Secondary;
         const ProfileSkillPriority profileSkillPriorityTertiary = ProfileSkillPriority.Tertiary;
         const ProfileSkillPriority profileSkillPriorityQuatenary = ProfileSkillPriority.Quatenary;
         const ProfileSkillPriority profileSkillPriorityRandom = ProfileSkillPriority.Random;

         var playerSkillScores = new List<PlayerSkillScore>();

         // Geef alle primary skills een score tussen de -30 en +30% van R1.
         // Onthoud de hoogste primary skill score, want dat is uitgangspunt voor de secondary skills.
         decimal highestPrimaryScore = 0;
         foreach (var primarySkill in profile.PlayerProfileSkills.Where(x => x.SkillPriority == profileSkillPriorityPrimary))
         {
            decimal randomPercentage = Randomizer.GetRandomNumber(-30, 30);
            decimal score = startNumberR1 + ((startNumberR1 * randomPercentage) / 100);

            if (score > highestPrimaryScore) highestPrimaryScore = score;

            int roundedScore = (int)decimal.Round(score, 0);
            if (roundedScore > 20) roundedScore = 20;
            if (roundedScore < 1) roundedScore = 1;

            var playerSkillScore = new PlayerSkillScore
            {
               PlayerSkill = primarySkill.Skill,
               Score = roundedScore
            };
            playerSkillScores.Add(playerSkillScore);
         }

         // Bepaal uitgangspunt voor secondary skills score: tussen de 60% en 80% van de hoogste primary skill score (=R3)
         decimal secondaryPercentage = Randomizer.GetRandomNumber(60, 80);
         decimal r3 = (highestPrimaryScore * secondaryPercentage) / 100;

         // Bepaal random (true/false) of er onderscheid wordt gemaakt tussen tertiary en secondary skills.
         // Zo niet, dan zijn tertiary skills ook gewoon secondary.
         var booleans = new Dictionary<bool, float> { { true, 2 }, { false, 1 } };
         bool useTertiarySkills = booleans.RandomElementByWeight(x => x.Value).Key;

         // Pak respectievelijk alleen de secondary skills of zowel de secondary als tertiary skills.         
         IEnumerable<PlayerProfileSkill> secondarySkills = useTertiarySkills
            ? profile.PlayerProfileSkills.Where(x => x.SkillPriority == profileSkillPrioritySecondary)
            : profile.PlayerProfileSkills.Where(x => x.SkillPriority == profileSkillPrioritySecondary || x.SkillPriority == profileSkillPriorityTertiary);

         decimal highestSecondaryScore = 0;
         decimal lowestSecondaryScore = 20;
         foreach (var secondarySkill in secondarySkills)
         {
            // Neem een random percentage tussen de -30% en +30% van R3 en bepaal hiermee de score.
            decimal randomPercentage = Randomizer.GetRandomNumber(-30, 30);
            decimal score = r3 + ((r3 * randomPercentage) / 100);

            // Onthoud de hoogste en laagste secondary skill score, want dat is uitgangspunt voor de tertiary en quatenary skills.
            if (score > highestSecondaryScore) highestSecondaryScore = score;
            if (score < lowestSecondaryScore) lowestSecondaryScore = score;

            int roundedScore = (int)decimal.Round(score, 0);
            if (roundedScore > 20) roundedScore = 20;
            if (roundedScore < 1) roundedScore = 1;
            var playerSkillScore = new PlayerSkillScore
            {
               PlayerSkill = secondarySkill.Skill,
               Score = roundedScore
            };
            playerSkillScores.Add(playerSkillScore);
         }

         // Houtjetouwtje fix voor als een profiel geen secondary skills heeft...
         if (highestSecondaryScore == 0) highestSecondaryScore = r3;
         if (lowestSecondaryScore == 20) lowestSecondaryScore = r3;

         // Bepaal random (true/false) of er onderscheid wordt gemaakt tussen quatenary en tertiary skills.
         // Zo niet, dan zijn tertiary skills ook gewoon secondary.
         bool useQuatenarySkills = booleans.RandomElementByWeight(x => x.Value).Key;
         
         IEnumerable<PlayerProfileSkill> tertiarySkills = new List<PlayerProfileSkill>();
         IEnumerable<PlayerProfileSkill> quatenarySkills = new List<PlayerProfileSkill>();
         if (useTertiarySkills)
         {
            if (useQuatenarySkills)
            {
               tertiarySkills = profile.PlayerProfileSkills.Where(x => x.SkillPriority == profileSkillPriorityTertiary);
               quatenarySkills =
                  profile.PlayerProfileSkills.Where(x => x.SkillPriority == profileSkillPriorityQuatenary);
            }
            else
            {
               // De tertiary skills zijn zowel tertiary als quatenary.
               tertiarySkills = profile.PlayerProfileSkills.Where(
                  x => x.SkillPriority == profileSkillPriorityTertiary ||
                       x.SkillPriority == profileSkillPriorityQuatenary);
            }
         }
         else
         {
            if (useQuatenarySkills)
            {
               quatenarySkills = profile.PlayerProfileSkills.Where(x => x.SkillPriority == profileSkillPriorityQuatenary);
            }
            else
            {
               // De quatenary skills worden verplaatst naar de tertiary skills.
               tertiarySkills = profile.PlayerProfileSkills.Where(x => x.SkillPriority == profileSkillPriorityQuatenary);
               useTertiarySkills = true;
            }
         }

         if (useTertiarySkills)
         {
            // Bepaal uitgangspunt voor tertiary skills score: tussen de 60% en 80% van de hoogste secondary skill score (=R3)
            decimal tertiaryPercentage = Randomizer.GetRandomNumber(60, 80);
            decimal r4 = (highestSecondaryScore * tertiaryPercentage) / 100;

            foreach (var tertiarySkill in tertiarySkills)
            {
               // Neem een random percentage tussen de -30% en +30% van R4 en bepaal hiermee de score.
               decimal randomPercentage = Randomizer.GetRandomNumber(-30, 30);
               decimal score = r4 + ((r4 * randomPercentage) / 100);

               int roundedScore = (int)decimal.Round(score, 0);
               if (roundedScore > 20) roundedScore = 20;
               if (roundedScore < 1) roundedScore = 1;
               var playerSkillScore = new PlayerSkillScore
               {
                  PlayerSkill = tertiarySkill.Skill,
                  Score = roundedScore
               };
               playerSkillScores.Add(playerSkillScore);
            }
         }

         if (useQuatenarySkills)
         {
            foreach (var quatenarySkill in quatenarySkills)
            {
               // Pak een random nummer tussen 0 en de laagste secondary skill score.
               int score = Randomizer.GetRandomNumber(0, (int)decimal.Round(lowestSecondaryScore, 0));
               if (score > 20) score = 20;
               if (score < 1) score = 1;
               var playerSkillScore = new PlayerSkillScore
               {
                  PlayerSkill = quatenarySkill.Skill,
                  Score = score
               };
               playerSkillScores.Add(playerSkillScore);
            }
         }

         // De random skills krijgen een random waarde tussen 1 en de hoogste secondary skill.
         var randomSkills = profile.PlayerProfileSkills.Where(x => x.SkillPriority == profileSkillPriorityRandom);
         foreach (var randomSkill in randomSkills)
         {
            // Pak een random nummer tussen 1 en de hoogste secondary skill score.
            int score = Randomizer.GetRandomNumber(1, (int)decimal.Round(highestSecondaryScore, 0));
            if (score > 20) score = 20;
            var playerSkillScore = new PlayerSkillScore
            {
               PlayerSkill = randomSkill.Skill,
               Score = score
            };
            playerSkillScores.Add(playerSkillScore);
         }

         return playerSkillScores;
      }

      public List<PlayerSkillScore> Calculate(PlayerProfile playerProfile, int age)
      {
         // Pak random nummer tussen 1 en 20 (=R1).
         var r1 = Randomizer.GetRandomNumber(1, 20);

         return Calculate(r1, playerProfile, age);
      }
   }
}
