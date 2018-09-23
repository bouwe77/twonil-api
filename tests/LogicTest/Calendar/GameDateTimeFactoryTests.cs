using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TwoNil.Logic.Functionality.Calendar;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Calendar
{
    [TestClass]
    public class GameDateTimeFactoryTests
    {
        private DateTime _dateTime = new DateTime(2018, 9, 23);

        [TestMethod]
        public void CreateWithoutEvents()
        {
            var gameDateTime = GameDateTimeFactory.CreateWithoutEvents(_dateTime);

            AssertDefaultStuff(gameDateTime);

            Assert.IsFalse(gameDateTime.ManagerPlaysMatch);
            Assert.AreEqual(GameDateTimeEventStatus.NotApplicable, gameDateTime.Matches);
            Assert.AreEqual(GameDateTimeEventStatus.NotApplicable, gameDateTime.EndOfSeason);
        }

        [TestMethod]
        public void CreateForOtherTeamsMatches()
        {
            var gameDateTime = GameDateTimeFactory.CreateForOtherTeamsMatches(_dateTime);

            AssertDefaultStuff(gameDateTime);

            Assert.IsFalse(gameDateTime.ManagerPlaysMatch);
            Assert.AreEqual(GameDateTimeEventStatus.ToDo, gameDateTime.Matches);
            Assert.AreEqual(GameDateTimeEventStatus.NotApplicable, gameDateTime.EndOfSeason);
        }

        [TestMethod]
        public void CreateForManagersMatches()
        {
            var gameDateTime = GameDateTimeFactory.CreateForManagersMatches(_dateTime);

            AssertDefaultStuff(gameDateTime);

            Assert.IsTrue(gameDateTime.ManagerPlaysMatch);
            Assert.AreEqual(GameDateTimeEventStatus.ToDo, gameDateTime.Matches);
            Assert.AreEqual(GameDateTimeEventStatus.NotApplicable, gameDateTime.EndOfSeason);
        }

        [TestMethod]
        public void CreateForEndOfSeason()
        {
            var gameDateTime = GameDateTimeFactory.CreateForEndOfSeason(_dateTime);

            AssertDefaultStuff(gameDateTime);

            Assert.IsFalse(gameDateTime.ManagerPlaysMatch);
            Assert.AreEqual(GameDateTimeEventStatus.NotApplicable, gameDateTime.Matches);
            Assert.AreEqual(GameDateTimeEventStatus.ToDo, gameDateTime.EndOfSeason);
        }

        private void AssertDefaultStuff(GameDateTime gameDateTime)
        {
            Assert.AreEqual(_dateTime, gameDateTime.DateTime);
            Assert.AreEqual("2018-09-23", gameDateTime.Date);
            Assert.AreEqual(GameDateTimeStatus.Future, gameDateTime.Status);
        }
    }
}
