using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   public interface IServiceFactory
   {
      IUserService CreateUserService();
      IGameService CreateGameService();
      ICompetitionService CreateCompetitionService();
      ITeamService CreateTeamService(GameInfo gameInfo);
      IPlayerService CreatePlayerService(GameInfo gameInfo);
      IMatchService CreateMatchService(GameInfo gameInfo);
      ISeasonService CreateSeasonService(GameInfo gameInfo);
      IRoundService CreateRoundService(GameInfo gameInfo);
      ILeagueTableService CreateLeagueTableService(GameInfo gameInfo);
      IStatisticsService CreateStatisticsService(GameInfo gameInfo);
   }
}
