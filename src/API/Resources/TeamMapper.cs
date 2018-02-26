using System.Linq;
using Sally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class TeamMapper : IResourceMapper<Team>
   {
      public static string TeamName = "name";
      public static string Rating = "rating";
      public static string RatingPercentage = "rating-percentage";
      public static string LeagueName = "league-name";

      public Resource Map(Team team, params string[] properties)
      {
         var resource = new Resource(new Link(UriFactory.GetTeamUri(team.GameId, team.Id)));

         if (properties.Contains(TeamName))
         {
            resource.AddProperty(TeamName, team.Name);
         }

         if (properties.Contains(Rating))
         {
            resource.AddProperty(Rating, team.Rating);
         }

         if (properties.Contains(RatingPercentage))
         {
            var ratingPercentage = (team.Rating / 20) * 100;
            resource.AddProperty(RatingPercentage, ratingPercentage);
         }

         if (properties.Contains(LeagueName))
         {
            resource.AddProperty(LeagueName, team.CurrentLeagueCompetition.Name);
         }

         return resource;
      }
   }
}