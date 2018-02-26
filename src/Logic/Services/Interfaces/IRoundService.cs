using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   public interface IRoundService
   {
      IEnumerable<Round> GetBySeasonCompetition(SeasonCompetition seasonCompetition);
   }
}
