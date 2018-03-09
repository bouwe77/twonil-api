using System.Linq;
using Shally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class GameInfoMapper : IResourceMapper<GameInfo>
   {
      private readonly UriHelper _uriHelper;
      public static string GameName = "name";
      public static string TeamName = "my-team";

      public GameInfoMapper(UriHelper uriHelper)
      {
         _uriHelper = uriHelper;
      }

      public Resource Map(GameInfo gameInfo, params string[] properties)
      {
         var resource = new Resource(new Link(_uriHelper.GetGameUri(gameInfo.Id)));
         
         if (properties.Contains(GameName))
         {
            resource.AddProperty(GameName, gameInfo.Name);
         }

         if (gameInfo.CurrentTeam != null)
         {
            if (properties.Contains(TeamName))
            {
               resource.AddProperty(TeamName, gameInfo.CurrentTeam.Name);
            }
         }

         return resource;
      }
   }
}