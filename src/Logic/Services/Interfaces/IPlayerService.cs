using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
    public interface IPlayerService
    {
       IEnumerable<Player> GetAll();
       IEnumerable<Player> GetByTeam(Team team);
       Player GetPlayer(string playerId);
       Player GetPlayer(string playerId, string teamId);
       void SubstitutePlayers(Player player1, Player player2);
    }
}
