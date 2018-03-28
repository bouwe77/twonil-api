using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
   public class TeamService : ServiceWithGameBase
   {
      internal TeamService(GameInfo gameInfo)
         : base(gameInfo)
      {
      }

      /// <summary>
      /// Gets all teams of the game.
      /// </summary>
      public IEnumerable<Team> GetAll()
      {
         using (var teamRepository = RepositoryFactory.CreateTeamRepository())
         {
            var teams = teamRepository.GetTeams();
            return teams;
         }
      }

      public IEnumerable<Team> GetGroupedByLeague()
      {
         var teams = GetAll().OrderBy(team => team.CurrentLeagueCompetition.Name).ThenBy(team => team.Name);
         return teams;
      }

      public IEnumerable<Team> GetBySeasonCompetition(SeasonCompetition seasonCompetition)
      {
         using (var teamRepository = RepositoryFactory.CreateTeamRepository())
         {
            var teams = teamRepository.GetTeamsBySeasonCompetition(seasonCompetition);
            return teams;
         }
      }

      public Team GetTeam(string teamId)
      {
         using (var teamRepository = RepositoryFactory.CreateTeamRepository())
         {
            var team = teamRepository.GetTeam(teamId);
            return team;
         }
      }
   }
}
