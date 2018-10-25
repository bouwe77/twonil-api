using System;
using TwoNil.Data;
using TwoNil.Logic.Calendar;
using TwoNil.Logic.Competitions;
using TwoNil.Logic.Matches.PostMatches;
using TwoNil.Logic.Players;
using TwoNil.Logic.Teams;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
    public interface IServiceFactory
    {
        CompetitionService CreateCompetitionService();
        GameDateTimeService CreateGameDateTimeService(GameInfo gameInfo);
        GameService CreateGameService();
        LeagueTableService CreateLeagueTableService(GameInfo gameInfo);
        MatchService CreateMatchService(GameInfo gameInfo);
        PlayerService CreatePlayerService(GameInfo gameInfo);
        RoundService CreateRoundService(GameInfo gameInfo);
        SeasonService CreateSeasonService(GameInfo gameInfo);
        StatisticsService CreateStatisticsService(GameInfo gameInfo);
        TeamService CreateTeamService(GameInfo gameInfo);
        UserService CreateUserService();
    }

    /// <summary>
    /// Factory class for creating services.
    /// </summary>
    public class ServiceFactory : IServiceFactory
    {
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly GameDateTimeReadManager _gameDateTimeReadManager;
        private readonly GameDateTimeMutationManager _gameDateTimeMutationManager;
        private readonly PlayerGenerator _playerGenerator;
        private readonly TeamManager _teamManager;
        private readonly PostMatchOrchestrator _postMatchOrchestrator;
        private readonly SeasonManager _seasonManager;

        public ServiceFactory(
            IUnitOfWorkFactory uowFactory,
            GameDateTimeReadManager gameDateTimeReadManager,
            GameDateTimeMutationManager gameDateTimeMutationManager,
            PlayerGenerator playerGenerator,
            TeamManager teamManager,
            PostMatchOrchestrator postMatchOrchestrator,
            SeasonManager seasonManager)
        {
            _uowFactory = uowFactory;
            _gameDateTimeReadManager = gameDateTimeReadManager;
            _gameDateTimeMutationManager = gameDateTimeMutationManager;
            _playerGenerator = playerGenerator;
            _teamManager = teamManager;
            _postMatchOrchestrator = postMatchOrchestrator;
            _seasonManager = seasonManager;
        }

        public GameService CreateGameService()
        {
            return new GameService(_uowFactory);
        }

        public CompetitionService CreateCompetitionService()
        {
            return new CompetitionService(_uowFactory);
        }

        public GameDateTimeService CreateGameDateTimeService(GameInfo gameInfo)
        {
            Assert(gameInfo);
            var matchService = CreateMatchService(gameInfo);
            return new GameDateTimeService(_uowFactory, gameInfo, matchService, _gameDateTimeReadManager, _gameDateTimeMutationManager);
        }

        public TeamService CreateTeamService(GameInfo gameInfo)
        {
            Assert(gameInfo);
            return new TeamService(_uowFactory, gameInfo);
        }

        public PlayerService CreatePlayerService(GameInfo gameInfo)
        {
            Assert(gameInfo);
            var teamService = CreateTeamService(gameInfo);
            return new PlayerService(_uowFactory, gameInfo, teamService, _playerGenerator, _teamManager);
        }

        public UserService CreateUserService()
        {
            return new UserService(_uowFactory);
        }

        public MatchService CreateMatchService(GameInfo gameInfo)
        {
            Assert(gameInfo);
            var seasonService = CreateSeasonService(gameInfo);
            return new MatchService(_uowFactory, gameInfo, seasonService, _postMatchOrchestrator);
        }

        public SeasonService CreateSeasonService(GameInfo gameInfo)
        {
            Assert(gameInfo);
            return new SeasonService(_uowFactory, gameInfo, _seasonManager);
        }

        public RoundService CreateRoundService(GameInfo gameInfo)
        {
            Assert(gameInfo);
            return new RoundService(_uowFactory, gameInfo);
        }

        public LeagueTableService CreateLeagueTableService(GameInfo gameInfo)
        {
            Assert(gameInfo);
            return new LeagueTableService(_uowFactory, gameInfo);
        }

        public StatisticsService CreateStatisticsService(GameInfo gameInfo)
        {
            return new StatisticsService(_uowFactory, gameInfo);
        }

        private void Assert(GameInfo gameInfo)
        {
            if (gameInfo == null || string.IsNullOrWhiteSpace(gameInfo.GameId)) throw new ArgumentException(nameof(gameInfo));
        }
    }
}
