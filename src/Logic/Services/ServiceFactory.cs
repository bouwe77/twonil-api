using System;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
   /// <summary>
   /// Factory class for creating services.
   /// </summary>
   public class ServiceFactory
   {
      public GameService CreateGameService()
      {
         return new GameService();
      }

      public CompetitionService CreateCompetitionService()
      {
         return new CompetitionService();
      }

      public TeamService CreateTeamService(GameInfo gameInfo)
      {
         Assert(gameInfo);
         return new TeamService(gameInfo);
      }

      public PlayerService CreatePlayerService(GameInfo gameInfo)
      {
         Assert(gameInfo);
         return new PlayerService(gameInfo);
      }

      public UserService CreateUserService()
      {
         return new UserService();
      }

      public MatchService CreateMatchService(GameInfo gameInfo)
      {
         Assert(gameInfo);
         return new MatchService(gameInfo);
      }

      public SeasonService CreateSeasonService(GameInfo gameInfo)
      {
         Assert(gameInfo);
         return new SeasonService(gameInfo);
      }

      public RoundService CreateRoundService(GameInfo gameInfo)
      {
         Assert(gameInfo);
         return new RoundService(gameInfo);
      }

      private void Assert(GameInfo gameInfo)
      {
         if (gameInfo == null || string.IsNullOrWhiteSpace(gameInfo.GameId)) throw new ArgumentException("gameInfo");
      }

      public LeagueTableService CreateLeagueTableService(GameInfo gameInfo)
      {
         Assert(gameInfo);
         return new LeagueTableService(gameInfo);
      }

      public StatisticsService CreateStatisticsService(GameInfo gameInfo)
      {
         return new StatisticsService(gameInfo);
      }
   }
}
