using TwoNil.Data;
using TwoNil.Data.Database;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions
{
   internal class SeasonStatisticsManager
   {
      private readonly TransactionManager _repository;
      private DatabaseRepositoryFactory _repositoryFactory;

      public SeasonStatisticsManager(TransactionManager repository, DatabaseRepositoryFactory repositoryFactory)
      {
         _repository = repository;
         _repositoryFactory = repositoryFactory;
      }

      public void UpdateNationalSuperCupWinner(string seasonId, Team winner)
      {
         SeasonStatistics seasonStatistics;
         using (var seasonStatisticsRepository = _repositoryFactory.CreateSeasonStatisticsRepository())
         {
            seasonStatistics = seasonStatisticsRepository.GetBySeason(seasonId);
            seasonStatistics.NationalSuperCupWinner = winner;
            _repository.RegisterUpdate(seasonStatistics);
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
            _repository.RegisterUpdate(seasonStatistics);
         }
      }

      public void UpdateNationalChampion(string seasonId, Team nationalChampion, Team nationalChampionRunnerUp)
      {
         using (var seasonStatisticsRepository = _repositoryFactory.CreateSeasonStatisticsRepository())
         {
            var seasonStatistics = seasonStatisticsRepository.GetBySeason(seasonId);
            seasonStatistics.NationalChampion = nationalChampion;
            seasonStatistics.NationalChampionRunnerUp = nationalChampionRunnerUp;
            _repository.RegisterUpdate(seasonStatistics);
         }
      }
   }
}
