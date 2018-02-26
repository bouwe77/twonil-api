namespace TwoNil.API.Resources
{
   public class PositionFactory
   {
      public static Position Create(Shared.DomainObjects.Position domainObject, bool fullResource = true)
      {
         var resource = new Position
         {
            Name = domainObject.Name,
            Shortname = domainObject.ShortName,
            Line = domainObject.Line.Name
         };

         return resource;
      }
   }
}