using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Functionality.Calendar;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
    public class GameDateTimeService : ServiceWithGameBase
    {
        public GameDateTimeService(GameInfo gameInfo)
            : base(gameInfo)
        {
        }

        public GameDateTime GetNow()
        {
            var manager = new GameDateTimeReadManager(RepositoryFactory);
            return manager.GetNow();
        }

        public void EndNow()
        {
            var transactionManager = RepositoryFactory.CreateTransactionManager();

            var manager = new GameDateTimeMutationManager(transactionManager, RepositoryFactory);
            manager.GoToNext();

            transactionManager.Save();
        }
    }
}
