using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Calendar
{
    public class GameDateTimeReadManager
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public GameDateTimeReadManager(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public GameDateTime GetNow(bool allowNonExisting = false)
        {
            using (var repo = _repositoryFactory.CreateGameDateTimeRepository())
            {
                if (allowNonExisting)
                    return repo.GetByStatus(GameDateTimeStatus.Now).FirstOrDefault();
                else
                    return repo.GetByStatus(GameDateTimeStatus.Now).First();
            }
        }

        internal GameDateTime GetNext()
        {
            using (var repo = _repositoryFactory.CreateGameDateTimeRepository())
            {
                return repo.GetByStatus(GameDateTimeStatus.Future).First();
            }
        }
    }
}
