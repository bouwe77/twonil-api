using System.Linq;
using Shally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class UserMapper : IResourceMapper<User>
   {
      public static string Firstname = "firstname";
      public static string Lastname = "lastname";
      public static string Username = "username";
      public static string Email = "email";
      private UriHelper _uriHelper;

      public UserMapper(UriHelper uriHelper)
      {
         _uriHelper = uriHelper;
      }

      public Resource Map(User user, params string[] properties)
      {
         var resource = new Resource(new Link(_uriHelper.GetUserUri(user.Id)));

         if (properties.Contains(Firstname))
         {
            resource.AddProperty(Firstname, user.Firstname);
         }

         if (properties.Contains(Lastname))
         {
            resource.AddProperty(Lastname, user.Lastname);
         }

         if (properties.Contains(Username))
         {
            resource.AddProperty(Username, user.Username);
         }

         if (properties.Contains(Email))
         {
            resource.AddProperty(Email, user.Email);
         }

         return resource;
      }
   }
}