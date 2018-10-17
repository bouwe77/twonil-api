using System.Collections.Generic; //======= KLAAR =======
using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Exceptions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
   public class LeagueTableService : ServiceWithGameBase
   {
      internal LeagueTableService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo)
         : base(uowFactory, gameInfo)
      {
      }

      public LeagueTable GetBySeasonAndCompetition(string seasonId, string competitionId)
      {
         SeasonCompetition seasonCompetition;
         using (var uow = UowFactory.Create())
         {
            seasonCompetition = uow.SeasonCompetitions.Find(sc => sc.SeasonId == seasonId && sc.CompetitionId == competitionId).FirstOrDefault();
            if (seasonCompetition == null)
            {
               string message = $"Combination of season '{seasonId}' and competition '{competitionId}' does not exist";
               throw new NotFoundException(message);
            }
         }

         using (var uow = UowFactory.Create())
         {
            var leagueTable = uow.LeagueTables.GetBySeasonCompetition(new[] { seasonCompetition.Id }).FirstOrDefault();
            if (leagueTable == null)
            {
               string message = $"No league table exists for season '{seasonId}' and competition '{competitionId}'";
               throw new NotFoundException(message);
            }

            return leagueTable;
         }
      }

      public IEnumerable<LeagueTable> GetBySeason(string seasonId)
      {
         using (var uow = UowFactory.Create())
         {
            var seasonCompetitions = uow.SeasonCompetitions.Find(x => x.SeasonId == seasonId).Select(x => x.Id);
            var leagueTables = uow.LeagueTables.GetBySeasonCompetition(seasonCompetitions);
            return leagueTables;
         }
      }
   }
}
