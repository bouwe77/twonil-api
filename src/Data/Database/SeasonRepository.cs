using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Database
{
   public class SeasonRepository : ReadRepository<Season>
   {
      internal SeasonRepository(string databaseFilePath, string gameId)
         : base(databaseFilePath, gameId)
      {
      }

      public Season GetCurrentSeason()
      {
         var seasons = GetAll().OrderByDescending(season => season.GameOrder);
         return seasons.FirstOrDefault();
      }
   }
}
