using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
    public class GameRepository : ReadRepository<Game>
    {
        internal GameRepository(string databaseFilePath)
           : base(databaseFilePath, null)
        {
        }

        public Game GetGame(string gameId)
        {
            var game = GetOne(gameId);
            return game;
        }

        public IEnumerable<Game> GetByUserId(string userId)
        {
            var userGames = Find(x => x.UserId == userId);
            return userGames;
        }

        public void CreateGame(string gameId)
        {
            // Create the game database.
            var gameDatabaseManager = new RepositoryFactory(gameId).CreateGameDatabaseManager();
            gameDatabaseManager.Create();
        }

        public void DeleteGame(string gameId)
        {
            // Delete the game from the master database.
            using (var transactionManager = new RepositoryFactory().CreateTransactionManager())
            {
                var game = GetGame(gameId);
                transactionManager.RegisterDelete(game);
                transactionManager.Save();
            }

            // Delete the game database.
            var gameDatabaseManager = new RepositoryFactory(gameId).CreateGameDatabaseManager();
            gameDatabaseManager.Delete();
        }
    }
}
