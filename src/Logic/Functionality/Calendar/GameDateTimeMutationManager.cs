using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoNil.Data;
using TwoNil.Logic.Exceptions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Calendar
{
    public class GameDateTimeMutationManager
    {
        private readonly TransactionManager _transactionManager;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly GameDateTimeReadManager _readManager;

        public GameDateTimeMutationManager(TransactionManager transactionManager, IRepositoryFactory repositoryFactory)
        {
            _transactionManager = transactionManager;
            _repositoryFactory = repositoryFactory;
            _readManager = new GameDateTimeReadManager(_repositoryFactory);
        }

        public void UpdateMatchStatus(DateTime matchDateTime)
        {
            var now = _readManager.GetNow();

            if (now.DateTime != matchDateTime)
                throw new ConflictException($"Now is {now.DateTime}, so can not update match status {matchDateTime}");

            now.Matches = GameDateTimeEventStatus.Done;

            _transactionManager.RegisterUpdate(now);
        }

        //TODO Rename this method
        public void GoToNext()
        {
            var now = _readManager.GetNow();

            if (!now.CanNavigateToNext)
                throw new ConflictException($"Now {now.DateTime} is not finished yet");

            now.Status = GameDateTimeStatus.Past;
            _transactionManager.RegisterUpdate(now);

            var next = _readManager.GetNext();
            next.Status = GameDateTimeStatus.Now;
            _transactionManager.RegisterUpdate(next);
        }
    }
}
