using System;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
   public abstract class ServiceWithGameBase : ServiceBase
   {
      protected DatabaseRepositoryFactory RepositoryFactory;
      protected GameInfo GameInfo;

      public ServiceWithGameBase(GameInfo gameInfo)
      {
         if (gameInfo == null || string.IsNullOrEmpty(gameInfo.Id))
         {
            throw new ArgumentException("GameInfo can not be null");
         }

         GameInfo = gameInfo;
         RepositoryFactory = new DatabaseRepositoryFactory(gameInfo.Id);
      }
   }
}
