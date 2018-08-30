using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
   public class SeasonCompetitionRepository : ReadRepository<SeasonCompetition>
   {
      internal SeasonCompetitionRepository(string databaseFilePath, string gameId)
         : base(databaseFilePath, gameId)
      {
      }

      public SeasonCompetition GetBySeasonAndCompetition(string seasonId, string competitionId)
      {
         return Find(x => x.SeasonId == seasonId && x.CompetitionId == competitionId).FirstOrDefault();
      }
   }
}
