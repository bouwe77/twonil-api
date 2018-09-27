using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
   public class RoundService : ServiceWithGameBase
   {
      internal RoundService(GameInfo gameInfo)
         : base(gameInfo)
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
