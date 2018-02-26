using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   public interface IMatchOrchestrator
   {
      void MatchesWerePlayed(List<Match> playedMatches);
   }
}
