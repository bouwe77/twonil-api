using Sally.Hal;

namespace TwoNil.API.Resources
{
   /// <summary>
   /// This is a factory class instead of a IResourceMapper implementation because this resource does not correspond with a DomainObject.
   /// </summary>
   public class MatchDayResourceFactory
   {
      public static Resource Create(string gameId, string matchDayId)
      {
         var resource = new Resource(new Link(UriFactory.GetMatchDayUri(gameId, matchDayId)));

         return resource;
      }
   }
}