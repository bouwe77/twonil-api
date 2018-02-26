using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Database   
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
   }
}
