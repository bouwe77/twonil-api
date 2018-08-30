using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
   public class UserRepository : ReadRepository<User>
   {
      internal UserRepository(string databaseFilePath)
         : base(databaseFilePath, null)
      {
      }
   }
}
