using TwoNil.Data; //======= KLAAR =======
using TwoNil.Logic.Calendar;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
    public class GameDateTimeService : ServiceWithGameBase
    {
        private readonly IMatchService _matchService;
        private readonly IGameDateTimeReadManager _gameDateTimeReadManager;
        private readonly IGameDateTimeMutationManager _gameDateTimeMutationManager;

        public GameDateTimeService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo, IMatchService matchService, IGameDateTimeReadManager gameDateTimeReadManager, IGameDateTimeMutationManager gameDateTimeMutationManager)
            : base(uowFactory, gameInfo)
        {
            _gameDateTimeReadManager = gameDateTimeReadManager;
            _gameDateTimeMutationManager = gameDateTimeMutationManager;
            _matchService = matchService;
        }

        public GameDateTime GetNow()
        {
            return _gameDateTimeReadManager.GetNow();
        }

        public void NavigateToNext()
        {
            var now = _gameDateTimeReadManager.GetNow();
            if (now.ManagerPlaysMatch || now.Matches != GameDateTimeEventStatus.ToDo)
            {
                GoToNext();
            }
            else
            {
                // Keep on playing matches until a current GameDateTime indicates the manager plays a match. 
                while (!now.ManagerPlaysMatch && now.Matches == GameDateTimeEventStatus.ToDo)
                {
                    using (var uow = UowFactory.Create())
                    {
                        _matchService.PlayMatchDay(now.DateTime);
                        //TODO Hier moet een transactie omheen
                    }

                    GoToNext();
                    now = _gameDateTimeReadManager.GetNow();
                }
            }
        }

        private void GoToNext()
        {
            using (var uow = UowFactory.Create())
            {
                _gameDateTimeMutationManager.GoToNext(uow);
                //TODO Hier moet een transactie omheen
            }
        }
    }
}
