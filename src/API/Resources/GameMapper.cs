using System.Linq;
using Sally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class GameInfoMapper : IResourceMapper<GameInfo>
   {
      public static string GameName = "name";
      public static string TeamName = "my-team";

      public Resource Map(GameInfo gameInfo, params string[] properties)
      {
         var resource = new Resource(new Link(UriFactory.GetGameUri(gameInfo.Id)));
         
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