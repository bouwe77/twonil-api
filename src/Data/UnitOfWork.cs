using System;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using TwoNil.Data.Repositories;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private TwoNilDbContext _twoNilContext;
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UnitOfWork(DbContextFactory dbContextFactory, IDbConnectionFactory dbConnectionFactory)
        {
            _twoNilContext = dbContextFactory.CreateContext();
            _dbConnectionFactory = dbConnectionFactory;
        }

        public IGenericRepository<User> Users => new GenericRepository<User>(_twoNilContext);

        public IGenericRepository<Team> Teams => new GenericRepository<Team>(_twoNilContext);

        public IGenericRepository<Player> Players => new GenericRepository<Player>(_twoNilContext);

        public IGenericRepository<Season> Seasons => new GenericRepository<Season>(_twoNilContext);

        public IGenericRepository<Round> Rounds => new GenericRepository<Round>(_twoNilContext);

        public IGenericRepository<Match> Matches => new GenericRepository<Match>(_twoNilContext);

        public IGenericRepository<LeagueTable> LeagueTables => new GenericRepository<LeagueTable>(_twoNilContext);

        public IGenericRepository<LeagueTablePosition> LeagueTablePositions => new GenericRepository<LeagueTablePosition>(_twoNilContext);

        public IGenericRepository<SeasonCompetition> SeasonCompetitions => new GenericRepository<SeasonCompetition>(_twoNilContext);

        public IGenericRepository<SeasonCompetitionTeam> SeasonCompetitionTeams => new GenericRepository<SeasonCompetitionTeam>(_twoNilContext);

        public IGenericRepository<SeasonStatistics> SeasonStatics => new GenericRepository<SeasonStatistics>(_twoNilContext);

        public IGenericRepository<SeasonTeamStatistics> SeasonTeamStatistics => new GenericRepository<SeasonTeamStatistics>(_twoNilContext);

        public IGenericRepository<TeamStatistics> TeamStatistics => new GenericRepository<TeamStatistics>(_twoNilContext);

        public IGenericRepository<Game> Games => new GenericRepository<Game>(_twoNilContext);

        public IGenericRepository<GameDateTime> GameDateTimes => new GenericRepository<GameDateTime>(_twoNilContext);

        public IGenericRepository<GameInfo> GameInfos => new GenericRepository<GameInfo>(_twoNilContext);

        public ICompetitionRepository Competitions => new CompetitionRepository();

        public IFormationRepository Formations => new FormationRepository();

        public ILineRepository Lines => new LineRepository();

        public INameRepository Names => new NameRepository();

        public IPlayerProfileRepository PlayerProfiles => new PlayerProfileRepository();

        public IPlayerSkillRepository PlayerSkills => new PlayerSkillRepository();

        public IPositionRepository Positions => new PositionRepository();

        public ITeamNameRepository TeamNames => new TeamNameRepository();

        public async Task CommitAsync()
        {
            await _twoNilContext.SaveChangesAsync();
        }

        public void Commit()
        {
            _twoNilContext.SaveChanges();
        }

        public ITransaction BeginTransaction()
        {
            var dbContextTransaction = _twoNilContext.Database.BeginTransaction();
            return new Transaction(dbContextTransaction);
        }

        public void Dispose()
        {
            if (_twoNilContext != null)
                _twoNilContext.Dispose();
        }
    }
}
