using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   public interface ILeagueTableService
   {
      LeagueTable GetBySeasonAndCompetition(string seasonId, string competitionId);
      IEnumerable<LeagueTable> GetBySeason(string seasonId);
   }
}
