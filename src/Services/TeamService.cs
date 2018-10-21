using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
    public class TeamService : ServiceWithGameBase
    {
        internal TeamService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo)
           : base(uowFactory, gameInfo)
        {
        }

        /// <summary>
        /// Gets all teams of the game.
        /// </summary>
        public IEnumerable<Team> GetAll()
        {
            using (var uow = UowFactory.Create())
            {
                var teams = uow.Teams.GetTeams();
                return teams;
            }
        }

        public IEnumerable<Team> GetGroupedByLeague()
        {
            var teams = GetAll().OrderBy(team => team.CurrentLeagueCompetition.Name).ThenBy(team => team.Name);
            return teams;
        }

        public IEnumerable<Team> GetBySeasonCompetition(string seasonCompetitionId)
        {
            using (var uow = UowFactory.Create())
            {
                var teams = uow.Teams.GetTeamsBySeasonCompetition(seasonCompetitionId);
                return teams;
            }
        }

        public Team GetTeam(string teamId)
        {
            using (var uow = UowFactory.Create())
            {
                var team = uow.Teams.GetTeam(teamId);
                return team;
            }
        }
    }
}
