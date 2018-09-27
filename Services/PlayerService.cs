using System.Collections.Generic;
using System.Linq;
using TwoNil.Logic.Players;
using TwoNil.Logic.Teams;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
   public class PlayerService : ServiceWithGameBase
   {
      private readonly PlayerGenerator _playerGenerator;
      private TeamService _teamService;

      internal PlayerService(GameInfo gameInfo)
         : base(gameInfo)
      {
         _playerGenerator = new PlayerGenerator();
         _teamService = new ServiceFactory().CreateTeamService(gameInfo);
      }

      public IEnumerable<Player> GetAll()
      {
         using (var playerRepository = RepositoryFactory.CreatePlayerRepository())
         {
            var players = playerRepository.GetPlayers();
            return players;
         }
      }

      public IEnumerable<Player> GetByTeam(Team team)
      {
         using (var playerRepository = RepositoryFactory.CreatePlayerRepository())
         {
            var players = playerRepository.GetPlayersByTeam(team);
            return players;
         }
      }

      public Player GetPlayer(string playerId)
      {
         using (var playerRepository = RepositoryFactory.CreatePlayerRepository())
         {
            return playerRepository.GetOne(playerId);
         }
      }

      //public void Update(Player player)
      //{
      //   using (var playerRepository = _repositoryFactory.GetPlayerRepository())
      //   {
      //      playerRepository.Update(player);
      //   }
      //}

      //public void Delete(Player player)
      //{
      //   using (var playerRepository = _repositoryFactory.GetPlayerRepository())
      //   {
      //      playerRepository.Delete(player);
      //   }
      //}

      public void SubstitutePlayers(Player player1, Player player2)
      {
         // Update the team order of both players.
         int oldPlayer1TeamOrder = player1.TeamOrder;
         int oldPlayer2TeamOrder = player2.TeamOrder;
         player1.TeamOrder = oldPlayer2TeamOrder;
         player2.TeamOrder = oldPlayer1TeamOrder;

         using (var transactionManager = RepositoryFactory.CreateTransactionManager())
         {
            transactionManager.RegisterUpdate(player1);
            transactionManager.RegisterUpdate(player2);

            var team = player1.Team;

            var teamManager = new TeamManager(RepositoryFactory);
            teamManager.UpdateRating(team);
            transactionManager.RegisterUpdate(team);

            transactionManager.Save();
         }
      }

      public Player GetPlayer(string playerId, string teamId)
      {
         using (var playerRepository = RepositoryFactory.CreatePlayerRepository())
         {
            return playerRepository.Find(p => p.Id == playerId && p.TeamId == teamId).FirstOrDefault();
         }
      }
   }
}
