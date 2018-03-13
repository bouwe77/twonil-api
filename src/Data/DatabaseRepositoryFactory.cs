using TwoNil.Data.Database;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data
{
   public class DatabaseRepositoryFactory : RepositoryFactoryBase, IDatabaseRepositoryFactory
   {
      private string _gameId;

      public DatabaseRepositoryFactory(string gameId)
         : base(GameDatabaseLocator.GetLocation(gameId))
      {
         _gameId = gameId;
      }

      public GameDatabaseManager CreateGameDatabaseManager()
      {
         return new GameDatabaseManager(DatabaseFilePath);
      }

      public SeasonTeamStatisticsRepository CreateSeasonTeamStatisticsRepository()
      {
         return new SeasonTeamStatisticsRepository(DatabaseFilePath, _gameId);
      }

      public SeasonStatisticsRepository CreateSeasonStatisticsRepository()
      {
         return new SeasonStatisticsRepository(DatabaseFilePath, _gameId);
      }

      public PlayerRepository CreatePlayerRepository()
      {
         return new PlayerRepository(DatabaseFilePath, _gameId);
      }

      public TeamRepository CreateTeamRepository()
      {
         return new TeamRepository(DatabaseFilePath, _gameId);
      }

      public IMatchRepository CreateMatchRepository()
      {
         return new MatchRepository(DatabaseFilePath, _gameId);
      }

      public RoundRepository CreateRoundRepository()
      {
         return new RoundRepository(DatabaseFilePath, _gameId);
      }

      public LeagueTableRepository CreateLeagueTableRepository()
      {
         return new LeagueTableRepository(DatabaseFilePath, _gameId);
      }

      public SeasonRepository CreateSeasonRepository()
      {
         return new SeasonRepository(DatabaseFilePath, _gameId);
      }

      public SeasonCompetitionRepository CreateSeasonCompetitionRepository()
      {
         return new SeasonCompetitionRepository(DatabaseFilePath, _gameId);
      }

      public IRepository<T> CreateRepository<T>() where T : DomainObjectBase, new()
      {
         return new ReadRepository<T>(DatabaseFilePath, _gameId);
      }

      public GameInfoRepository CreateGameInfoRepository()
      {
         return new GameInfoRepository(DatabaseFilePath, _gameId);
      }
   }
}
