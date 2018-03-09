using Shally.Hal;

namespace TwoNil.API.Resources
{
   /// <summary>
   /// This is a factory class instead of a IResourceMapper implementation because this resource does not correspond with a DomainObject.
   /// </summary>
   public class MatchDayResourceFactory
   {
      private readonly UriHelper _uriHelper;

      public MatchDayResourceFactory(UriHelper uriHelper)
      {
         _uriHelper = uriHelper;
      }

      public Resource Create(string gameId, string matchDayId)
      {
         var resource = new Resource(new Link(_uriHelper.GetMatchDayUri(gameId, matchDayId)));

         return resource;
      }
   }
}