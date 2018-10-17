using System.Collections.Generic;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
   public class RoundService : ServiceWithGameBase
   {
      internal RoundService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo)
         : base(uowFactory, gameInfo)
      {
      }

      public IEnumerable<Round> GetBySeasonCompetition(SeasonCompetition seasonCompetition)
      {
         using (var roundRepository = RepositoryFactory.CreateRoundRepository())
         {
            return roundRepository.GetBySeasonCompetition(seasonCompetition.Id);
         }
      }
   }
}
