using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Players;
using TwoNil.Logic.Teams;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
    public class PlayerService : ServiceWithGameBase
    {
        private readonly IPlayerGenerator _playerGenerator;
        private readonly ITeamManager _teamManager;
        private readonly ITeamService _teamService;

        internal PlayerService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo, ITeamService teamService, IPlayerGenerator playerGenerator, ITeamManager teamManager)
           : base(uowFactory, gameInfo)
        {
            _playerGenerator = playerGenerator;
            _teamManager = teamManager;
            _teamService = teamService;
        }

        public IEnumerable<Player> GetAll()
        {
            using (var uow = UowFactory.Create())
            {
                var players = uow.Players.GetPlayers();
                return players;
            }
        }

        public IEnumerable<Player> GetByTeam(Team team)
        {
            using (var uow = UowFactory.Create())
            {
                var players = uow.Players.GetPlayersByTeam(team);
                return players;
            }
        }

        public Player GetPlayer(string playerId)
        {
            using (var uow = UowFactory.Create())
            {
                return uow.Players.GetOne(playerId);
            }
        }

        //public void Update(Player player)
        //{
        //   using (var uow = UowFactory.Create())
        //   {
        //      uow.Players.Update(player);
        //   }
        //}

        //public void Delete(Player player)
        //{
        //   using (var uow = UowFactory.Create())
        //   {
        //      uow.Players.Delete(player);
        //   }
        //}

        public void SubstitutePlayers(Player player1, Player player2)
        {
            // Update the team order of both players.
            int oldPlayer1TeamOrder = player1.TeamOrder;
            int oldPlayer2TeamOrder = player2.TeamOrder;
            player1.TeamOrder = oldPlayer2TeamOrder;
            player2.TeamOrder = oldPlayer1TeamOrder;

            using (var uow = UowFactory.Create())
            {
                //TODO Create transaction on UOW

                uow.Players.Update(player1);
                uow.Players.Update(player2);

                var team = player1.Team;

                _teamManager.UpdateRating(team);
                uow.Teams.Update(team);

                //TODO commit here
            }
        }

        public Player GetPlayer(string playerId, string teamId)
        {
            using (var uow = UowFactory.Create())
            {
                return uow.Players.Find(p => p.Id == playerId && p.TeamId == teamId).FirstOrDefault();
            }
        }
    }
}
