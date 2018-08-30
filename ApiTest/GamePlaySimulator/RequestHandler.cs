using Dolores.Http;
using Dolores.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoNil.API.Controllers;

namespace ApiTest
{
    internal class RequestHandler
    {
        private string _gameId;

        public RequestHandler(string gameId)
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

        public Response PlayMatchDay(string dayId)
        {
            var controller = new MatchController();

            var response = controller.PostPlayDayMatches(_gameId, dayId);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            return response;
        }
    }
}
