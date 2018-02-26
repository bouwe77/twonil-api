using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   public interface IUserService
   {
      User GetUser(string username, string password);
      User GetUser(string userId);
      User CreateUser(string firstname, string lastname, string username, string email, string password);
   }
}
