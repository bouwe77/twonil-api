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

        public void NavigateToNext()
        {
            var readManager = new GameDateTimeReadManager(RepositoryFactory);

            var now = readManager.GetNow();
            if (now.ManagerPlaysMatch || now.Matches != GameDateTimeEventStatus.ToDo)
            {
                GoToNext();
            }
            else
            {
                var matchService = new ServiceFactory().CreateMatchService(GameInfo);

                // Keep on playing matches until a current GameDateTime indicates the manager plays a match. 
                while (!now.ManagerPlaysMatch && now.Matches == GameDateTimeEventStatus.ToDo)
                {
                    using (var transactionManager = RepositoryFactory.CreateTransactionManager())
                    {
                        matchService.PlayMatchDay(now.DateTime, transactionManager);
                        transactionManager.Save();
                    }

                    GoToNext();
                    now = readManager.GetNow();
                }
            }
        }

        private void GoToNext()
        {
            using (var transactionManager = RepositoryFactory.CreateTransactionManager())
            {
                var mutationManager = new GameDateTimeMutationManager(transactionManager, RepositoryFactory);
                mutationManager.GoToNext();
                transactionManager.Save();
            }
        }
    }
}
