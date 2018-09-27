using TwoNil.Data;
using TwoNil.Data.Repositories;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Competitions
{
   public class SeasonStatisticsManager
   {
      private readonly TransactionManager _transactionManager;
      private IRepositoryFactory _repositoryFactory;

      public SeasonStatisticsManager(TransactionManager transactionManager, IRepositoryFactory repositoryFactory)
      {
         _transactionManager = transactionManager;
         _repositoryFactory = repositoryFactory;
      }

      public void UpdateNationalSuperCupWinner(string seasonId, Team winner)
      {
         SeasonStatistics seasonStatistics;
         using (var seasonStatisticsRepository = _repositoryFactory.CreateSeasonStatisticsRepository())
         {
            seasonStatistics = seasonStatisticsRepository.GetBySeason(seasonId);
            seasonStatistics.NationalSuperCupWinner = winner;
            _transactionManager.RegisterUpdate(seasonStatistics);
         }
      }

      public void UpdateNationalCupWinner(string seasonId, Team winner, Team runnerUp)
      {
         SeasonStatistics seasonStatistics;
         using (var seasonStatisticsRepository = _repositoryFactory.CreateSeasonStatisticsRepository())
         {
            seasonStatistics = seasonStatisticsRepository.GetBySeason(seasonId);
            seasonStatistics.CupWinner = winner;
            seasonStatistics.CupRunnerUp = runnerUp;
            _transactionManager.RegisterUpdate(seasonStatistics);
         }
      }

      public void UpdateNationalChampion(string seasonId, Team nationalChampion, Team nationalChampionRunnerUp)
      {
         using (var seasonStatisticsRepository = _repositoryFactory.CreateSeasonStatisticsRepository())
         {
            var seasonStatistics = seasonStatisticsRepository.GetBySeason(seasonId);
            seasonStatistics.NationalChampion = nationalChampion;
            seasonStatistics.NationalChampionRunnerUp = nationalChampionRunnerUp;
            _transactionManager.RegisterUpdate(seasonStatistics);
         }
      }
   }
}
