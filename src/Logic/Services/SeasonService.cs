using System.Linq;
using TwoNil.Logic.Exceptions;
using TwoNil.Logic.Functionality.Competitions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
   public class SeasonService : ServiceWithGameBase
   {
      internal SeasonService(GameInfo gameInfo)
         : base(gameInfo)
      {
      }

      public Season GetCurrentSeason()
      {
         using (var seasonRepository = RepositoryFactory.CreateSeasonRepository())
         {
            return seasonRepository.GetCurrentSeason();
         }
      }

      public Season Get(string seasonId)
      {
         using (var seasonRepository = RepositoryFactory.CreateSeasonRepository())
         {
            return seasonRepository.GetOne(seasonId);
         }
      }

      public bool DetermineSeasonEnded(string seasonId)
      {
         var season = Get(seasonId);
         if (season == null)
         {
            throw new NotFoundException($"Season '{seasonId}' does not exist");
         }

         bool seasonEnded;
         using (var matchRepository = RepositoryFactory.CreateMatchRepository())
         {
            seasonEnded = matchRepository.GetBySeason(seasonId).All(m => m.MatchStatus == MatchStatus.Ended);
         }

         return seasonEnded;
      }

      public void EndSeasonAndCreateNext(string seasonId)
      {
         var seasonManager = new SeasonManager(RepositoryFactory);

         Season season;
         using (var seasonRepository = RepositoryFactory.CreateSeasonRepository())
         {
            season = seasonRepository.GetOne(seasonId);
         }

         var transactionManager = RepositoryFactory.CreateTransactionManager();
         seasonManager.EndSeason(season, transactionManager);

         //wil ik dit??? transactionManager.Save();

         seasonManager.CreateNextSeason(season, transactionManager);
         transactionManager.Save();
      }
   }
}
