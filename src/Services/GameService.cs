using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
    public class GameService : ServiceBase
    {
        public IEnumerable<GameInfo> GetGames(string userId)
        {
            var gameInfos = new List<GameInfo>();

            using (var gameRepository = new RepositoryFactory().CreateGameRepository())
            {
                var games = gameRepository.GetByUserId(userId);

                foreach (var game in games)
                {
                    using (var gameInfoRepository = new RepositoryFactory(game.Id).CreateGameInfoRepository())
                    {
                        var gameInfo = gameInfoRepository.GetGameInfo();
                        gameInfos.Add(gameInfo);
                    }
                }
            }

            return gameInfos;
        }

        public GameInfo GetGame(string gameId, string userId)
        {
            GameInfo gameInfo = null;

            using (var gameRepository = new RepositoryFactory().CreateGameRepository())
            {
                // First check the game belongs to the user.
                var game = gameRepository.GetByUserId(userId).FirstOrDefault(x => x.Id == gameId);

                // If so, retrieve the game info.
                if (game != null)
                {
                    using (var gameInfoRepository = new RepositoryFactory(gameId).CreateGameInfoRepository())
                    {
                        gameInfo = gameInfoRepository.GetGameInfo();
                    }
                }
            }

            return gameInfo;
        }

        public void AddChosenTeam(string gameId, string userId, Team chosenTeam)
        {
            throw new NotImplementedException();
            //var game = GetGame(gameId, userId);
            //if (game == null)
            //{
            //   throw new NotFoundException($"Game with ID '{gameId}' not found");
            //}

            //game.CurrentTeam = chosenTeam;
            //using (var transactionManager = new RepositoryFactory().CreateTransactionManager())
            //{
            //   transactionManager.RegisterUpdate(game);
            //   transactionManager.Save();
            //}
        }

        public void DeleteGame(string gameId)
        {
            // Delete the game from the database.
            using (var gameRepository = new RepositoryFactory().CreateGameRepository())
            {
                gameRepository.DeleteGame(gameId);
            }
        }

        public void DeleteAllGames()
        {
            // Delete all games from the database.
            using (var gameRepository = new RepositoryFactory().CreateGameRepository())
            {
                var games = gameRepository.GetAll();
                foreach (var game in games)
                {
                    gameRepository.DeleteGame(game.Id);
                }
            }
        }
    }
}
