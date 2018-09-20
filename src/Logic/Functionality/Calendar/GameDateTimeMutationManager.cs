﻿using System;
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

        internal void UpdateEndOfSeasonStatus(DateTime endOfSeasonDateTime)
        {
            var now = _readManager.GetNow();

            if (now.DateTime != endOfSeasonDateTime)
                throw new ConflictException($"Now is {now.DateTime}, so can not update end of season status {endOfSeasonDateTime}");

            now.EndOfSeason = GameDateTimeEventStatus.Done;

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

        public void CreateNewForSeason(IEnumerable<DateTime> matchDates, DateTime endOfSeasonDate)
        {
            var gameDateTimes = new List<GameDateTime>();

            // Add dates for all matches.
            foreach (var matchDate in matchDates)
            {
                gameDateTimes.Add(GameDateTimeFactory.CreateForMatches(matchDate));
            }

            // Add the date when the season can be ended.
            gameDateTimes.Add(GameDateTimeFactory.CreateForEndOfSeason(endOfSeasonDate));

            SetFirstDateToNowIfNecessary(gameDateTimes);

            _transactionManager.RegisterInsert(gameDateTimes);

        }

        private void SetFirstDateToNowIfNecessary(List<GameDateTime> gameDateTimes)
        {
            // Determine there is already a "Now" date/time. This is not the case when datetimes are created for a new game.
            var now = _readManager.GetNow(true);

            if (now == null)
                gameDateTimes.OrderBy(g => g.DateTime).First().Status = GameDateTimeStatus.Now;
        }
    }
}
