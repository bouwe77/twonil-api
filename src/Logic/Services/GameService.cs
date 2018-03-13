using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
   public class GameService : ServiceBase
   {
      public IEnumerable<GameInfo> GetGames(string userId)
      {
         var gameInfos = new List<GameInfo>();

         using (var gameRepository = new MasterRepositoryFactory().CreateGameRepository())
         {
            var games = gameRepository.GetByUserId(userId);

            foreach (var game in games)
            {
               using (var gameInfoRepository = new DatabaseRepositoryFactory(game.Id).CreateGameInfoRepository())
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

         using (var gameRepository = new MasterRepositoryFactory().CreateGameRepository())
         {
            // First check the game belongs to the user.
            var game = gameRepository.GetByUserId(userId).FirstOrDefault(x => x.Id == gameId);

            // If so, retrieve the game info.
            if (game != null)
            {
               using (var gameInfoRepository = new DatabaseRepositoryFactory(gameId).CreateGameInfoRepository())
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
         //using (var transactionManager = new MasterRepositoryFactory().CreateTransactionManager())
         //{
         //   transactionManager.RegisterUpdate(game);
         //   transactionManager.Save();
         //}
      }

      public void DeleteGame(string gameId)
      {
         // Delete the game record from the master database.
         using (var gameRepository = new MasterRepositoryFactory().CreateGameRepository())
         using (var transactionManager = new MasterRepositoryFactory().CreateTransactionManager())
         {
            var game = gameRepository.GetOne(gameId);
            transactionManager.RegisterDelete(game);
            transactionManager.Save();
         }

         // Delete the game database.
         var gameDatabaseManager = new DatabaseRepositoryFactory(gameId).CreateGameDatabaseManager();
         gameDatabaseManager.Delete();
      }
   }
}
