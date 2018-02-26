using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Database
{
   public class PlayerRepository : ReadRepository<Player>
   {
      internal PlayerRepository(string databaseFilePath, string gameId)
         : base(databaseFilePath, gameId)
      {
      }

      public IEnumerable<Player> GetPlayersByTeam(Team team, bool getReferencedData = true)
      {
         var players = Find(player => player.TeamId != null && player.TeamId.Equals(team.Id)).ToList();

         if (getReferencedData)
         {
            foreach (var player in players)
            {
               GetReferencedData(player);
            }
         }

         return players;
      }

      public IEnumerable<Player> GetPlayers()
      {
         var players = GetAll();
         
         foreach (var player in players)
         {
            GetReferencedData(player);
         }

         return players;
      }

      private void GetReferencedData(Player player)
      {
         var memoryRepositoryFactory = new MemoryRepositoryFactory();
         using (var positionRepository = memoryRepositoryFactory.CreatePositionRepository())
         {
            player.PreferredPosition = positionRepository.GetOne(player.PreferredPositionId);
            player.CurrentPosition = positionRepository.GetOne(player.CurrentPositionId);
         }

         var databaseRepositoryFactory = new DatabaseRepositoryFactory(player.GameId);
         using (var teamRepository = databaseRepositoryFactory.CreateRepository<Team>())
         {
            var team = teamRepository.GetOne(player.TeamId);
            player.Team = team;
         }
      }
   }
}