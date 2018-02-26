using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   public interface IGameService
   {
      IEnumerable<GameInfo> GetGames(string userId);
      Game CreateGameForUser(string userId);
      GameInfo GetGame(string gameId, string userId);
      void AddChosenTeam(string gameId, string userId, Team chosenTeam);
      void DeleteGame(string gameId);
   }
}
