using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Database
{
   public class TeamRepository : ReadRepository<Team>
   {
      internal TeamRepository(string databaseFilePath, string gameId)
         : base(databaseFilePath, gameId)
      {
      }

      public IEnumerable<Team> GetTeams()
      {
         var teams = GetAll();

         foreach (var team in teams)
         {
            GetReferencedData(team);
         }

         return teams;
      }

      public IEnumerable<Team> GetTeamsBySeasonCompetition(SeasonCompetition seasonCompetition)
      {
         var seasonCompetitionTeamRepository = new DatabaseRepositoryFactory(seasonCompetition.GameId).CreateRepository<SeasonCompetitionTeam>();
         var seasonCompetitionTeams = seasonCompetitionTeamRepository.Find(x => x.SeasonCompetition.Equals(seasonCompetition));

         var teams = seasonCompetitionTeams.Select(seasonCompetitionTeam => seasonCompetitionTeam.Team).ToList();
         return teams;
      }

      public Team GetTeam(string teamId)
      {
         var team = GetOne(teamId);
         if (team != null)
         {
            GetReferencedData(team);
         }

         return team;
      }

      private void GetReferencedData(Team team)
      {
         var repositoryFactory = new MemoryRepositoryFactory();
         using (var competitionRepository = repositoryFactory.CreateCompetitionRepository())
         {
            var currentLeagueCompetition = competitionRepository.GetOne(team.CurrentLeagueCompetitionId);
            team.CurrentLeagueCompetition = currentLeagueCompetition;
         }

         using (var formationRepository = repositoryFactory.CreateFormationRepository())
         {
            var formation = formationRepository.GetOne(team.FormationId);
            team.Formation = formation;
         }
      }
   }
}
