using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Database
{
   public class LeagueTableRepository : ReadRepository<LeagueTable>
   {
      internal LeagueTableRepository(string databaseFilePath, string gameId)
         : base(databaseFilePath, gameId)
      {
      }

      public IEnumerable<LeagueTable> GetBySeasonCompetition(IEnumerable<string> seasonCompetitionIds)
      {
         var leagueTables = Find(lt => seasonCompetitionIds.Contains(lt.SeasonCompetitionId));

         foreach (var leagueTable in leagueTables)
         {
            GetReferencedData(leagueTable);
         }

         return leagueTables;
      }

      private void GetReferencedData(LeagueTable leagueTable)
      {
         var repositoryFactory = new DatabaseRepositoryFactory(leagueTable.GameId);

         using (var leagueTablePositionRepository = repositoryFactory.CreateRepository<LeagueTablePosition>())
         {
            leagueTable.LeagueTablePositions = leagueTablePositionRepository.Find(x => x.LeagueTableId == leagueTable.Id).OrderBy(x => x.Position).ToList();
         }

         using (var seasonCompetitionRepository = repositoryFactory.CreateRepository<SeasonCompetition>())
         {
            var seasonCompetition = seasonCompetitionRepository.GetOne(leagueTable.SeasonCompetitionId);
            leagueTable.SeasonCompetition = seasonCompetition;
         }

         using (var teamRepository = repositoryFactory.CreateTeamRepository())
         {
            foreach (var leagueTablePosition in leagueTable.LeagueTablePositions)
            {
               var team = teamRepository.GetTeam(leagueTablePosition.TeamId);
               leagueTablePosition.Team = team;
            }
         }
      }
   }
}
