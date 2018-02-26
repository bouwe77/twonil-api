using System.Collections.Generic;
using System.Linq;
using TwoNil.Logic.Exceptions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
   public class LeagueTableService : ServiceWithGameBase
   {
      internal LeagueTableService(GameInfo gameInfo)
         : base(gameInfo)
      {
      }

      public LeagueTable GetBySeasonAndCompetition(string seasonId, string competitionId)
      {
         SeasonCompetition seasonCompetition;
         using (var seasonCompetitionRepository = RepositoryFactory.CreateSeasonCompetitionRepository())
         {
            seasonCompetition = seasonCompetitionRepository.Find(sc => sc.SeasonId == seasonId && sc.CompetitionId == competitionId).FirstOrDefault();
            if (seasonCompetition == null)
            {
               string message = $"Combination of season '{seasonId}' and competition '{competitionId}' does not exist";
               throw new NotFoundException(message);
            }
         }

         using (var leagueTableRepository = RepositoryFactory.CreateLeagueTableRepository())
         {
            var leagueTable = leagueTableRepository.GetBySeasonCompetition(new[] { seasonCompetition.Id }).FirstOrDefault();
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
         using (var seasonCompetitionRepository = RepositoryFactory.CreateSeasonCompetitionRepository())
         using (var leagueTableRepository = RepositoryFactory.CreateLeagueTableRepository())
         {
            var seasonCompetitions = seasonCompetitionRepository.Find(x => x.SeasonId == seasonId).Select(x => x.Id);
            var leagueTables = leagueTableRepository.GetBySeasonCompetition(seasonCompetitions);
            return leagueTables;
         }
      }
   }
}
