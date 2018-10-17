using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Exceptions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Calendar
{
    public interface IGameDateTimeMutationManager
    {
        void CreateNewForSeason(IUnitOfWork uow, IEnumerable<DateTime> matchDatesManagersTeam, IEnumerable<DateTime> matchDatesOtherTeams, DateTime endOfSeasonDate);
        void GoToNext(IUnitOfWork uow);
        void UpdateManagerPlaysMatch(IUnitOfWork uow, DateTime dateTime);
        void UpdateMatchStatus(IUnitOfWork uow, DateTime matchDateTime);
        void UpdateEndOfSeasonStatus(IUnitOfWork uow, DateTime endOfSeasonDateTime);
    }

    public class GameDateTimeMutationManager : IGameDateTimeMutationManager
    {
        private readonly IGameDateTimeReadManager _readManager;

        public GameDateTimeMutationManager(IGameDateTimeReadManager gameDateTimeReadManager)
        {
            _readManager = gameDateTimeReadManager;
        }

        public void UpdateMatchStatus(IUnitOfWork uow, DateTime matchDateTime)
        {
            var now = _readManager.GetNow();

            if (now.DateTime != matchDateTime)
                throw new ConflictException($"Now is {now.DateTime}, so can not update match status {matchDateTime}");

            now.Matches = GameDateTimeEventStatus.Done;

            uow.GameDateTimes.Update(now);
        }

        public void UpdateEndOfSeasonStatus(IUnitOfWork uow, DateTime endOfSeasonDateTime)
        {
            var now = _readManager.GetNow();

            if (now.DateTime != endOfSeasonDateTime)
                throw new ConflictException($"Now is {now.DateTime}, so can not update end of season status {endOfSeasonDateTime}");

            now.EndOfSeason = GameDateTimeEventStatus.Done;

            uow.GameDateTimes.Update(now);
        }

        public void GoToNext(IUnitOfWork uow)
        {
            var now = _readManager.GetNow();

            if (!now.CanNavigateToNext())
                throw new ConflictException($"Now {now.DateTime} is not finished yet");

            uow.GameDateTimes.Remove(now);

            var next = _readManager.GetNext();
            next.Status = GameDateTimeStatus.Now;
            uow.GameDateTimes.Update(next);
        }

        public void CreateNewForSeason(IUnitOfWork uow, IEnumerable<DateTime> matchDatesManagersTeam, IEnumerable<DateTime> matchDatesOtherTeams, DateTime endOfSeasonDate)
        {
            var gameDateTimes = new List<GameDateTime>();

            // Add dates for all matches.
            foreach (var matchDate in matchDatesManagersTeam)
            {
                gameDateTimes.Add(GameDateTimeFactory.CreateForManagersMatches(matchDate));
            }

            foreach (var matchDate in matchDatesOtherTeams)
            {
                gameDateTimes.Add(GameDateTimeFactory.CreateForOtherTeamsMatches(matchDate));
            }

            // Add the date when the season can be ended.
            gameDateTimes.Add(GameDateTimeFactory.CreateForEndOfSeason(endOfSeasonDate));

            SetFirstDateToNowIfNecessary(gameDateTimes);

            uow.GameDateTimes.Add(gameDateTimes);
        }

        public void UpdateManagerPlaysMatch(IUnitOfWork uow, DateTime dateTime)
        {
            var gameDateTime = _readManager.GetByDateTime(dateTime);

            gameDateTime.ManagerPlaysMatch = true;

           uow.GameDateTimes.Update(gameDateTime);
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
