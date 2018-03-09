using System.Linq;
using Shally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class SeasonMapper : IResourceMapper<Season>
   {
      private readonly UriHelper _uriHelper;
      public static string SeasonName = "name";

      public SeasonMapper(UriHelper uriHelper)
      {
         _uriHelper = uriHelper;
      }

      public Resource Map(Season season, params string[] properties)
      {
         var resource = new Resource(new Link(_uriHelper.GetSeasonUri(season.GameId, season.Id)));

         if (properties.Contains(SeasonName))
         {
            resource.AddProperty(SeasonName, season.Name);
         }

         return resource;
      }
   }
}