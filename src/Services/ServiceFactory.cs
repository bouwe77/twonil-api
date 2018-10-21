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
        GameDateTimeService CreateGameDateTimeService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo);
        GameService CreateGameService();
        LeagueTableService CreateLeagueTableService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo);
        MatchService CreateMatchService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo);
        PlayerService CreatePlayerService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo);
        RoundService CreateRoundService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo);
        SeasonService CreateSeasonService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo);
        StatisticsService CreateStatisticsService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo);
        TeamService CreateTeamService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo);
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

        public GameDateTimeService CreateGameDateTimeService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo)
        {
            Assert(gameInfo);
            var matchService = CreateMatchService(uowFactory, gameInfo);
            return new GameDateTimeService(_uowFactory, gameInfo, matchService, _gameDateTimeReadManager, _gameDateTimeMutationManager);
        }

        public TeamService CreateTeamService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo)
        {
            Assert(gameInfo);
            return new TeamService(_uowFactory, gameInfo);
        }

        public PlayerService CreatePlayerService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo)
        {
            Assert(gameInfo);
            var teamService = CreateTeamService(uowFactory, gameInfo);
            return new PlayerService(_uowFactory, gameInfo, teamService, _playerGenerator, _teamManager);
        }

        public UserService CreateUserService()
        {
            return new UserService(_uowFactory);
        }

        public MatchService CreateMatchService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo)
        {
            Assert(gameInfo);
            var seasonService = CreateSeasonService(uowFactory, gameInfo);
            return new MatchService(_uowFactory, gameInfo, seasonService, _postMatchOrchestrator);
        }

        public SeasonService CreateSeasonService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo)
        {
            Assert(gameInfo);
            return new SeasonService(_uowFactory, gameInfo, _seasonManager);
        }

        public RoundService CreateRoundService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo)
        {
            Assert(gameInfo);
            return new RoundService(_uowFactory, gameInfo);
        }

        public LeagueTableService CreateLeagueTableService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo)
        {
            Assert(gameInfo);
            return new LeagueTableService(_uowFactory, gameInfo);
        }

        public StatisticsService CreateStatisticsService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo)
        {
            return new StatisticsService(_uowFactory, gameInfo);
        }

        private void Assert(GameInfo gameInfo)
        {
            if (gameInfo == null || string.IsNullOrWhiteSpace(gameInfo.GameId)) throw new ArgumentException(nameof(gameInfo));
        }
    }
}
