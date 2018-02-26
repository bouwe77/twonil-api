using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Database
{
   public class RoundRepository : ReadRepository<Round>
   {
      internal RoundRepository(string databaseFilePath, string gameId)
         : base(databaseFilePath, gameId)
      {
      }

      public Round GetRound(string roundId)
      {
         var round = GetOne(roundId);
         if (round != null)
         {
            GetReferencedData(round);
         }

         return round;
      }

      private void GetReferencedData(Round round)
      {
         DatabaseRepositoryFactory databaseRepositoryFactory = new DatabaseRepositoryFactory(round.GameId);
         using (var seasonCompetitionRepository = databaseRepositoryFactory.CreateRepository<SeasonCompetition>())
         using (var seasonRepository = databaseRepositoryFactory.CreateRepository<Season>())
         using (var competitionRepository = new MemoryRepositoryFactory().CreateCompetitionRepository())
         {
            var seasonCompetition = seasonCompetitionRepository.GetOne(round.SeasonCompetitionId);
            round.SeasonCompetition = seasonCompetition;

            seasonCompetition.Season = seasonRepository.GetOne(seasonCompetition.SeasonId);
            seasonCompetition.Competition = competitionRepository.GetOne(seasonCompetition.CompetitionId);

            round.Season = seasonCompetition.Season;
         }
      }

      public IEnumerable<Round> GetBySeasonCompetition(string seasonCompetitionId)
      {
         var rounds = Find(round => round.SeasonCompetitionId.Equals(seasonCompetitionId));

         foreach (var round in rounds)
         {
            GetReferencedData(round);
         }

         return rounds;
      }

      public Round GetNextRound(Round previousRound)
      {
         var nextRound = Find(round => round.SeasonCompetitionId.Equals(previousRound.SeasonCompetitionId)
                                                       && round.Order == previousRound.Order + 1).FirstOrDefault();
         return nextRound;
      }

      public IEnumerable<Round> GetBySeason(string seasonId)
      {
         var rounds = Find(round => round.SeasonId.Equals(seasonId));

         foreach (var round in rounds)
         {
            GetReferencedData(round);
         }

         return rounds;
      }
   }
}
