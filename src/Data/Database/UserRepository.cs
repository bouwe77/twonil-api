using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Database
{
   public class UserRepository : ReadRepository<User>
   {
      internal UserRepository(string databaseFilePath)
         : base(databaseFilePath, null)
      {
      }
   }
}
