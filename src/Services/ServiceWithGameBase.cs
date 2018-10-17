using System;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
   public abstract class ServiceWithGameBase : ServiceBase
   {
      protected GameInfo GameInfo;

      public ServiceWithGameBase(IUnitOfWorkFactory uowFactory, GameInfo gameInfo)
            : base(uowFactory)
      {
         if (gameInfo == null || string.IsNullOrEmpty(gameInfo.Id))
         {
            throw new ArgumentException("GameInfo can not be null");
         }

         GameInfo = gameInfo;
      }
   }
}
