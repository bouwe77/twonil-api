using System;
using System.Collections.Generic;
using System.Linq;
using Randomization;
using TwoNil.Logic.Functionality.Teams;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
   public class TeamService : ServiceWithGameBase
   {
      private readonly TeamGenerator _teamGenerator;
      private readonly IListRandomizer _listRandomizer;

      internal TeamService(GameInfo gameInfo)
         : base(gameInfo)
      {
         _teamGenerator = new TeamGenerator();
         _listRandomizer = new ListRandomizer();
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

      public Team GetMyTeam()
      {
         throw new NotImplementedException();
      }
   }
}
