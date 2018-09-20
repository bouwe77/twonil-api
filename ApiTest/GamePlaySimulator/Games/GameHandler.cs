using ApiTest.GamePlaySimulator.Controllers;
using Dolores.Responses;

namespace ApiTest.GamePlaySimulator.Games
{
    public class GameHandler
    {
        private readonly string _gameId;

        public GameHandler(string gameId)
        {
            _gameId = gameId;
        }

        public Response GetGame()
        {
            var requestHandler = new ControllerHandler(_gameId);
            var gameResponse = requestHandler.GetGame();
            return gameResponse;
        }
    }
}
