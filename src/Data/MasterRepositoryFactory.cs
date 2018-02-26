using TwoNil.Data.Database;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data
{
   public class MasterRepositoryFactory : RepositoryFactoryBase
   {
      public MasterRepositoryFactory()
         : base("D:\\Mijn Databases\\TwoNil\\twonil.db")
      {
      } 

      public GameRepository CreateGameRepository()
      {
         return new GameRepository(DatabaseFilePath);
      }

      public IRepository<User> CreateUserRepository()
      {
         return new UserRepository(DatabaseFilePath);
      }
   }
}
