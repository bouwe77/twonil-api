using TwoNil.Data.Repositories;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data
{
    public interface IRepositoryFactory
    {
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
        TeamStatisticsRepository CreateTeamStatisticsRepository();
        TeamRepository CreateTeamRepository();
        GameRepository CreateGameRepository();
        IRepository<User> CreateUserRepository();
        PlayerSkillRepository CreatePlayerSkillRepository();
        LineRepository CreateLineRepository();
        FormationRepository CreateFormationRepository();
        CompetitionRepository CreateCompetitionRepository();
        PlayerProfileRepository CreatePlayerProfileRepository();
        IPositionRepository CreatePositionRepository();
        NameRepository CreateNameRepository();
    }
}