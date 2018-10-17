using System;
using System.Threading.Tasks;
using TwoNil.Data.Repositories;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        IGenericRepository<Team> Teams { get; }
        IGenericRepository<Player> Players { get; }
        IGenericRepository<Season> Seasons { get; }
        IGenericRepository<Round> Rounds { get; }
        IGenericRepository<Match> Matches { get; }
        IGenericRepository<LeagueTable> LeagueTables { get; }
        IGenericRepository<LeagueTablePosition> LeagueTablePositions { get; }
        IGenericRepository<SeasonCompetition> SeasonCompetitions { get; }
        IGenericRepository<SeasonCompetitionTeam> SeasonCompetitionTeams { get; }
        IGenericRepository<SeasonStatistics> SeasonStatics { get; }
        IGenericRepository<SeasonTeamStatistics> SeasonTeamStatistics { get; }
        IGenericRepository<TeamStatistics> TeamStatistics { get; }
        IGenericRepository<Game> Games { get; }
        IGenericRepository<GameDateTime> GameDateTimes { get; }
        IGenericRepository<GameInfo> GameInfos { get; }

        ICompetitionRepository Competitions { get; }
        IFormationRepository Formations { get; }
        ILineRepository Lines { get; }
        INameRepository Names { get; }
        IPlayerProfileRepository PlayerProfiles { get; }
        IPlayerSkillRepository PlayerSkills { get; }
        IPositionRepository Positions { get; }
        ITeamNameRepository TeamNames { get; }

        Task CommitAsync();
        void Commit();
        ITransaction BeginTransaction();
    }
}
