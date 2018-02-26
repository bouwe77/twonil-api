using TwoNil.Data.Database;

namespace TwoNil.Data
{
   public abstract class RepositoryFactoryBase
   {
      protected string DatabaseFilePath;

      protected RepositoryFactoryBase(string databaseFilePath)
      {
         DatabaseFilePath = databaseFilePath;
      }

      public TransactionManager CreateTransactionManager()
      {
         return new TransactionManager(DatabaseFilePath);
      }
   }
}
