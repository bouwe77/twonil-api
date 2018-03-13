using TwoNil.Data.Database;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data
{
   public interface IDatabaseRepositoryFactory
   {
      GameDatabaseManager CreateGameDatabaseManager();
      GameInfoRepository CreateGameInfoRepository();
      LeagueTableRepository CreateLeagueTableRepository();
      IMatchRepository CreateMatchRepository();
      PlayerRepository CreatePlayerRepository();
      IRepository<T> CreateRepository<T>() where T : DomainObjectBase, new();
      RoundRepository CreateRoundRepository();
      SeasonCompetitionRepository CreateSeasonCompetitionRepository();
      SeasonRepository CreateSeasonRepository();
      SeasonStatisticsRepository CreateSeasonStatisticsRepository();
      SeasonTeamStatisticsRepository CreateSeasonTeamStatisticsRepository();
      TeamRepository CreateTeamRepository();
   }
}