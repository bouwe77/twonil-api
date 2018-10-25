using System; //======= KLAAR =======
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoNil.Data;
using TwoNil.Logic;
using TwoNil.Logic.Exceptions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
    public class GameService : ServiceBase
    {
        private readonly GameCreationManager _gameCreationManager;

        public GameService(IUnitOfWorkFactory uowFactory, GameCreationManager gameCreationManager)
            : base(uowFactory)
        {
            _gameCreationManager = gameCreationManager;
        }

        public async Task<Game> CreateGame()
        {
            var game = await _gameCreationManager.CreateGame();
            return game;
        }

        public IEnumerable<GameInfo> GetGames(string userId)
        {
            var gameInfos = new List<GameInfo>();

            using (var uow = UowFactory.Create())
            {
                var games = uow.Games.GetByUserId(userId);

                foreach (var game in games)
                {
                    var gameInfo = uow.GameInfos.GetGameInfo();
                    gameInfos.Add(gameInfo);
                }
            }

            return gameInfos;
        }

        public GameInfo GetGame(string gameId, string userId)
        {
            GameInfo gameInfo = null;

            using (var uow = UowFactory.Create())
            {
                // First check the game belongs to the user.
                var game = uow.Games.GetByUserId(userId).FirstOrDefault(x => x.Id == gameId);

                // If so, retrieve the game info.
                if (game != null)
                {
                    gameInfo = uow.GameInfos.GetGameInfo();
                }
            }

            return gameInfo;
        }

        public void AddChosenTeam(string gameId, string userId, Team chosenTeam)
        {
            throw new NotImplementedException();
        }

        public void DeleteGame(string gameId)
        {
            // Delete the game from the database.
            using (var uow = UowFactory.Create())
            {
                var game = uow.Games.GetOne(gameId);
                DeleteGame(game);
            }
        }

        public void DeleteAllGames()
        {
            // Delete all games from the database.
            using (var uow = UowFactory.Create())
            {
                var games = uow.Games.GetAll();
                foreach (var game in games)
                {
                    DeleteGame(game);
                }
            }
        }

        private void DeleteGame(Game game)
        {
            // Delete the game from the database.
            using (var uow = UowFactory.Create())
            {
                if (game == null)
                    throw new NotFoundException("Game not found");

                uow.Games.Remove(game);
            }
        }
    }
}
