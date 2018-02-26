using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Database
{
   public class SeasonStatisticsRepository : ReadRepository<SeasonStatistics>
   {
      internal SeasonStatisticsRepository(string databaseFilePath, string gameId)
         : base(databaseFilePath, gameId)
      {
      }

      public SeasonStatistics GetBySeason(string seasonId)
      {
         var seasonStatistics = Find(x => x.SeasonId == seasonId).FirstOrDefault();

         if (seasonStatistics != null)
         {
            GetReferencedData(seasonStatistics);
         }

         return seasonStatistics;
      }

      private void GetReferencedData(SeasonStatistics seasonStatistics)
      {
         var databaseRepositoryFactory = new DatabaseRepositoryFactory(seasonStatistics.GameId);

         using (var seasonRepository = databaseRepositoryFactory.CreateRepository<Season>())
         {
            var season = seasonRepository.GetOne(seasonStatistics.SeasonId);
            seasonStatistics.Season = season;
         }

         using (var teamRepository = databaseRepositoryFactory.CreateRepository<Team>())
         {
            if (!string.IsNullOrWhiteSpace(seasonStatistics.NationalChampionTeamId))
            {
               var team = teamRepository.GetOne(seasonStatistics.NationalChampionTeamId);
               seasonStatistics.NationalChampion = team;
            }

            if (!string.IsNullOrWhiteSpace(seasonStatistics.NationalChampionRunnerUpTeamId))
            {
               var team = teamRepository.GetOne(seasonStatistics.NationalChampionRunnerUpTeamId);
               seasonStatistics.NationalChampionRunnerUp = team;
            }

            if (!string.IsNullOrWhiteSpace(seasonStatistics.CupWinnerTeamId))
            {
               var team = teamRepository.GetOne(seasonStatistics.CupWinnerTeamId);
               seasonStatistics.CupWinner = team;
            }

            if (!string.IsNullOrWhiteSpace(seasonStatistics.CupRunnerUpTeamId))
            {
               var team = teamRepository.GetOne(seasonStatistics.CupRunnerUpTeamId);
               seasonStatistics.CupRunnerUp = team;
            }

            if (!string.IsNullOrWhiteSpace(seasonStatistics.NationalSuperCupWinnerTeamId))
            {
               var team = teamRepository.GetOne(seasonStatistics.NationalSuperCupWinnerTeamId);
               seasonStatistics.NationalSuperCupWinner = team;
            }
         }
      }
   }
}
