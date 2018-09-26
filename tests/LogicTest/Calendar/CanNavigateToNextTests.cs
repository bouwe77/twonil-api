using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoNil.Logic.Functionality.Calendar;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Calendar
{
    [TestClass]
    public class CanNavigateToNextTests
    {
        [TestMethod]
        public void CanNavigateToNext_WhenFuture_ReturnsFalse()
        {
            var gameDateTime = GetDefaultGameDateTime();

            gameDateTime.Status = GameDateTimeStatus.Future;

            bool result = gameDateTime.CanNavigateToNext();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanNavigateToNext_WhenNow_ReturnsTrue()
        {
            var gameDateTime = GetDefaultGameDateTime();

            gameDateTime.Status = GameDateTimeStatus.Now;

            bool result = gameDateTime.CanNavigateToNext();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanNavigateToNext_WhenMatchesHaveNotYetBeenPlayed_ReturnsFalse()
        {
            var gameDateTime = GetDefaultGameDateTime();
            gameDateTime.Status = GameDateTimeStatus.Now;

            gameDateTime.Matches = GameDateTimeEventStatus.ToDo;
            gameDateTime.ManagerPlaysMatch = true;

            bool result = gameDateTime.CanNavigateToNext();

            Assert.IsFalse(result);
        }


        [TestMethod]
        public void CanNavigateToNext_WhenMatchesHaveNotYetBeenPlayed_ButManagersTeamDoesNotPlay_ReturnsTrue()
        {
            var gameDateTime = GetDefaultGameDateTime();
            gameDateTime.Status = GameDateTimeStatus.Now;

            gameDateTime.Matches = GameDateTimeEventStatus.ToDo;
            gameDateTime.ManagerPlaysMatch = false;

            bool result = gameDateTime.CanNavigateToNext();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanNavigateToNext_WhenMatchesHaveBeenPlayed_ReturnsTrue()
        {
            var gameDateTime = GetDefaultGameDateTime();
            gameDateTime.Status = GameDateTimeStatus.Now;

            gameDateTime.Matches = GameDateTimeEventStatus.Done;

            bool result = gameDateTime.CanNavigateToNext();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanNavigateToNext_WhenSeasonHasToBeEnded_ReturnsFalse()
        {
            var gameDateTime = GetDefaultGameDateTime();
            gameDateTime.Status = GameDateTimeStatus.Now;

            gameDateTime.EndOfSeason = GameDateTimeEventStatus.ToDo;

            bool result = gameDateTime.CanNavigateToNext();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanNavigateToNext_WhenSeasonHasBeenEnded_ReturnsTrue()
        {
            var gameDateTime = GetDefaultGameDateTime();
            gameDateTime.Status = GameDateTimeStatus.Now;

            gameDateTime.EndOfSeason = GameDateTimeEventStatus.Done;

            bool result = gameDateTime.CanNavigateToNext();

            Assert.IsTrue(result);
        }

        private GameDateTime GetDefaultGameDateTime()
        {
            return new GameDateTime
            {

            };
        }
    }
}
