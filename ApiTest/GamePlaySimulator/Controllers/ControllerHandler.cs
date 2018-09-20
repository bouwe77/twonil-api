using Dolores.Http;
using Dolores.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TwoNil.API.Controllers;

namespace ApiTest.GamePlaySimulator.Controllers
{
    internal class ControllerHandler
    {
        private string _gameId;

        public ControllerHandler(string gameId)
        {
            _gameId = gameId;
        }

        public Response GetGame()
        {
            var controller = new GameController();

            var response = controller.GetItem(_gameId);

            Assert.AreEqual(HttpStatusCode.Ok, response.StatusCode);

            return response;
        }

        public Response GetLeagueTables(string seasonId)
        {
            var controller = new LeagueTableController();

            var response = controller.GetBySeason(_gameId, seasonId);

            Assert.AreEqual(HttpStatusCode.Ok, response.StatusCode);

            return response;
        }

        public Response PlayMatchDay(string dayId)
        {
            var controller = new MatchController();

            var response = controller.PostPlayDayMatches(_gameId, dayId);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            return response;
        }

        public string GetCurrentSeasonId()
        {
            //var controller = new SeasonController();
            //var response = controller.GetSeasonCollection(_gameId);
            //Assert.AreEqual(HttpStatusCode.Ok, response.StatusCode);
            throw new NotImplementedException();
        }
    }
}
