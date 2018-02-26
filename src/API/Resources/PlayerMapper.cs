using System.Linq;
using Sally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class PlayerMapper : IResourceMapper<Player>
   {
      public static string Name = "name";
      public static string Age = "age";
      public static string PreferredPosition = "preferred-position";
      public static string CurrentPosition = "current-position";
      public static string Rating = "rating";
      public static string Skills = "skills";

      public Resource Map(Player player, params string[] properties)
      {
         var resource = new Resource(new Link(UriFactory.GetPlayerUri(player.GameId, player.Id)));

         if (properties.Contains(Name))
         {
            resource.AddProperty(Name, player.Name);
         }

         if (properties.Contains(Age))
         {
            resource.AddProperty(Age, player.Age);
         }

         if (properties.Contains(PreferredPosition))
         {
            var position = PositionFactory.Create(player.PreferredPosition);
            resource.AddProperty(PreferredPosition, position);
         }

         if (properties.Contains(CurrentPosition) && player.CurrentPosition != null)
         {
            var position = PositionFactory.Create(player.CurrentPosition);
            resource.AddProperty(CurrentPosition, position);
         }

         if (properties.Contains(Rating))
         {
            var rating = RatingFactory.Create(player.Rating);
            resource.AddProperty(Rating, rating);
         }

         if (properties.Contains(Skills))
         {
            // Football skills
            var football = new
            {
               Goalkeeping = player.SkillGoalkeeping,
               Defending = player.SkillDefending,
               Passing = player.SkillPassing,
               Speed = player.SkillSpeed,
               Technique = player.SkillTechnique,
               Shooting = player.SkillShooting,
               Heading = player.SkillHeading,
               Tactics = player.SkillTactics,
            };

            // Physical skills
            var physical = new
            {
               Fitness = player.SkillFitness,
               Pace = player.SkillPace,
               Stamina = player.SkillStamina,
               Strength = player.SkillStrength
            };

            // Mental skills
            var mental = new
            {
               Form = player.SkillForm,
               Confidence = player.SkillConfidence,
               Mentality = player.SkillMentality,
               Intelligence = player.SkillIntelligence
            };

            var skills = new
            {
               football,
               physical,
               mental
            };
            resource.AddProperty("skills", skills);
         }

         return resource;
      }
   }
}