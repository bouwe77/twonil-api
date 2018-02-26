using System.Linq;
using Sally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class SeasonMapper : IResourceMapper<Season>
   {
      public static string SeasonName = "name";

      public Resource Map(Season season, params string[] properties)
      {
         var resource = new Resource(new Link(UriFactory.GetSeasonUri(season.GameId, season.Id)));

         if (properties.Contains(SeasonName))
         {
            resource.AddProperty(SeasonName, season.Name);
         }

         return resource;
      }
   }
}