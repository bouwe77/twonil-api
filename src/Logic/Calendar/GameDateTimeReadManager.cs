using System;
using System.Linq;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Calendar
{
    public interface IGameDateTimeReadManager
    {
        GameDateTime GetByDateTime(DateTime dateTime);
        GameDateTime GetNow(bool allowNonExisting = false);
        GameDateTime GetNext();
    }

    public class GameDateTimeReadManager : IGameDateTimeReadManager
    {
        private readonly IUnitOfWorkFactory _uowFactory;

        public GameDateTimeReadManager(IUnitOfWorkFactory uowFactory)
        {
            _uowFactory = uowFactory;
        }

        public GameDateTime GetNow(bool allowNonExisting = false)
        {
            using (var uow = _uowFactory.Create())
            {
                if (allowNonExisting)
                    return uow.GameDateTimes.GetByStatus(GameDateTimeStatus.Now).FirstOrDefault();
                else
                    return uow.GameDateTimes.GetByStatus(GameDateTimeStatus.Now).First();
            }
        }

        public GameDateTime GetByDateTime(DateTime dateTime)
        {
            using (var uow = _uowFactory.Create())
            {
                return uow.GameDateTimes.Find(g => g.DateTime == dateTime).First();
            }
        }

        public GameDateTime GetNext()
        {
            using (var uow = _uowFactory.Create())
            {
                return uow.GameDateTimes.GetByStatus(GameDateTimeStatus.Future).First();
            }
        }
    }
}
