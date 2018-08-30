using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
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
         var repositoryFactory = new RepositoryFactory();
         using (var positionRepository = repositoryFactory.CreatePositionRepository())
         {
            player.PreferredPosition = positionRepository.GetOne(player.PreferredPositionId);
            player.CurrentPosition = positionRepository.GetOne(player.CurrentPositionId);
         }

         using (var teamRepository = repositoryFactory.CreateRepository<Team>())
         {
            var team = teamRepository.GetOne(player.TeamId);
            player.Team = team;
         }
      }
   }
}