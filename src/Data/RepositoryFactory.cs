using TwoNil.Data.Repositories;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private string _gameDatabaseFilePath;
        private string _masterDatabaseFilePath;

        private string _gameId;

        public RepositoryFactory()
        {
            _masterDatabaseFilePath = GetMasterDatabaseLocation();
        }

        public RepositoryFactory(string gameId)
        {
            _gameId = gameId;
            _gameDatabaseFilePath = GetGameDatabaseLocation(gameId);
            _masterDatabaseFilePath = GetMasterDatabaseLocation();
        }

        public TransactionManager CreateTransactionManager()
        {
            return new TransactionManager(_gameDatabaseFilePath);
        }

        internal GameDatabaseManager CreateGameDatabaseManager()
        {
            return new GameDatabaseManager(_gameDatabaseFilePath);
        }

        public SeasonTeamStatisticsRepository CreateSeasonTeamStatisticsRepository()
        {
            return new SeasonTeamStatisticsRepository(_gameDatabaseFilePath, _gameId);
        }

        public SeasonStatisticsRepository CreateSeasonStatisticsRepository()
        {
            return new SeasonStatisticsRepository(_gameDatabaseFilePath, _gameId);
        }

        public TeamStatisticsRepository CreateTeamStatisticsRepository()
        {
            return new TeamStatisticsRepository(_gameDatabaseFilePath, _gameId);
        }

        public PlayerRepository CreatePlayerRepository()
        {
            return new PlayerRepository(_gameDatabaseFilePath, _gameId);
        }

        public TeamRepository CreateTeamRepository()
        {
            return new TeamRepository(_gameDatabaseFilePath, _gameId);
        }

        public IMatchRepository CreateMatchRepository()
        {
            return new MatchRepository(_gameDatabaseFilePath, _gameId);
        }

        public RoundRepository CreateRoundRepository()
        {
            return new RoundRepository(_gameDatabaseFilePath, _gameId);
        }

        public LeagueTableRepository CreateLeagueTableRepository()
        {
            return new LeagueTableRepository(_gameDatabaseFilePath, _gameId);
        }

        public SeasonRepository CreateSeasonRepository()
        {
            return new SeasonRepository(_gameDatabaseFilePath, _gameId);
        }

        public SeasonCompetitionRepository CreateSeasonCompetitionRepository()
        {
            return new SeasonCompetitionRepository(_gameDatabaseFilePath, _gameId);
        }

        public IRepository<T> CreateRepository<T>() where T : DomainObjectBase, new()
        {
            return new ReadRepository<T>(_gameDatabaseFilePath, _gameId);
        }

        public GameInfoRepository CreateGameInfoRepository()
        {
            return new GameInfoRepository(_gameDatabaseFilePath, _gameId);
        }

        public GameRepository CreateGameRepository()
        {
            return new GameRepository(_masterDatabaseFilePath);
        }

        public IRepository<User> CreateUserRepository()
        {
            return new UserRepository(_masterDatabaseFilePath);
        }

        public PlayerSkillRepository CreatePlayerSkillRepository()
        {
            return new PlayerSkillRepository();
        }

        public LineRepository CreateLineRepository()
        {
            return new LineRepository();
        }

        public FormationRepository CreateFormationRepository()
        {
            return new FormationRepository();
        }

        public CompetitionRepository CreateCompetitionRepository()
        {
            return new CompetitionRepository();
        }

        public PlayerProfileRepository CreatePlayerProfileRepository()
        {
            return new PlayerProfileRepository();
        }

        public IPositionRepository CreatePositionRepository()
        {
            return new PositionRepository();
        }

        public NameRepository CreateNameRepository()
        {
            return new NameRepository();
        }

        private static string GetGameDatabaseLocation(string gameId)
        {
            return $@"D:\Mijn Databases\TwoNil\{gameId}.db";
        }

        private static string GetMasterDatabaseLocation()
        {
            return "D:\\Mijn Databases\\TwoNil\\twonil.db";
        }
    }
}
