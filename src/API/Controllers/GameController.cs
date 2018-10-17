using System;
using System.Collections.Generic;
using System.Linq;
using Dolores.Http;
using Dolores.Responses;
using Microsoft.Extensions.Logging;
using Shally.Hal;
using Shally.Forms;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;
using TwoNil.Logic;

namespace TwoNil.API.Controllers
{
    public class GameController : ControllerBase
    {
        private readonly GameInfoMapper _gameInfoMapper;

        public GameController()
        {
            _gameInfoMapper = new GameInfoMapper(UriHelper);
        }

        public Response GetCollection()
        {
            var gameService = ServiceFactory.CreateGameService();

            //TODO Let MODDERVOKKIN op
            var games = gameService.GetGames("17eqhq").ToList();

            var halDocument = CreateHalDocument(UriHelper.GetGamesUri());

            var gameResources = new List<Resource>();
            foreach (var game in games)
            {
                var gameResource = _gameInfoMapper.Map(game, GameInfoMapper.GameName, GameInfoMapper.TeamName);

                var form = new Form("delete-game")
                {
                    Action = UriHelper.GetGameUri(game.Id),
                    Method = "delete",
                    Title = "Delete"
                };
                gameResource.AddForm(form);

                gameResources.Add(gameResource);
            }

            halDocument.AddResource("rel:games", gameResources);

            var response = GetResponse(halDocument);

            return response;
        }

        public Response GetItem(string gameId)
        {
            RequestHelper.ValidateId(gameId);

            var gameInfo = GetGameInfo(gameId);

            if (gameInfo.CurrentTeam == null)
            {
                throw ResponseHelper.Get501NotImplemented("You must pick a team now, but this is not implemented yet...");
            }

            var halDocument = CreateHalDocument(UriHelper.GetGameUri(gameId), gameInfo);

            halDocument.AddLink("game-links", new Link(UriHelper.GetGameLinksUri(gameId)));

            // Add GameDateTime navigation.
            var gameDateTimeService = ServiceFactory.CreateGameDateTimeService(gameInfo);
            var now = gameDateTimeService.GetNow();
            var currentGameDateTimeResource = new GameDateTimeMapper(UriHelper).Map(now);
            halDocument.AddResource("rel:game-datetime-navigation", currentGameDateTimeResource);

            // Add my team.
            var teamResource = new TeamMapper(UriHelper).Map(gameInfo.CurrentTeam, TeamMapper.TeamName, TeamMapper.Rating, TeamMapper.RatingGoalkeeper, TeamMapper.RatingDefence, TeamMapper.RatingMidfield, TeamMapper.RatingAttack, TeamMapper.RatingPercentage);
            halDocument.AddResource("rel:my-team", teamResource);

            // Add season resource.
            var seasonService = ServiceFactory.CreateSeasonService(gameInfo);
            var currentSeason = seasonService.GetCurrentSeason();
            var seasonResource = new SeasonMapper(UriHelper).Map(currentSeason, SeasonMapper.SeasonShortName, SeasonMapper.SeasonLongName);

            bool endOfSeason = seasonService.DetermineSeasonEnded(currentSeason.Id);
            bool endSeasonNow = currentSeason.EndDateTime == now.DateTime;
            if (endOfSeason && endSeasonNow)
            {
                var form = new Form("end-season")
                {
                    Action = UriHelper.GetSeasonUri(gameId, currentSeason.Id),
                    Method = "post",
                    Title = "END SEASON!"
                };
                seasonResource.AddForm(form);
            }

            halDocument.AddResource("rel:current-season", seasonResource);

            // Add season team statistics.
            var statisticsService = ServiceFactory.CreateStatisticsService(gameInfo);
            var seasonTeamStatistics = statisticsService.GetSeasonTeamStatistics(currentSeason.Id, gameInfo.CurrentTeamId);
            var seasonTeamStatisticsResource = new SeasonTeamStatisticsMapper(UriHelper).Map(seasonTeamStatistics);
            halDocument.AddResource("rel:season-team-statistics", seasonTeamStatisticsResource);

            // Add next match day.
            var matchService = ServiceFactory.CreateMatchService(gameInfo);
            var nextMatchDate = matchService.GetNextMatchDate(currentSeason.Id);

            if (nextMatchDate.HasValue)
            {
                var matchDayResourceFactory = new MatchDayResourceFactory(UriHelper, gameId, nextMatchDate.Value);

                var matchDayResource = matchDayResourceFactory.Create();

                // Add a resource for the match of the current team.
                var matchForCurrentTeam = matchService.GetByMatchDayAndTeam(nextMatchDate.Value, gameInfo.CurrentTeamId);
                if (matchForCurrentTeam != null)
                {
                    var matchResource = new MatchMapper(UriHelper).Map(
                       matchForCurrentTeam,
                       MatchMapper.CompetitionName,
                       MatchMapper.CompetitionType,
                       MatchMapper.Date,
                       MatchMapper.Round);

                    var teamMapper = new TeamMapper(UriHelper);

                    var homeTeamResource = teamMapper.Map(matchForCurrentTeam.HomeTeam, TeamMapper.TeamName, TeamMapper.LeagueName, TeamMapper.CurrentLeaguePosition);
                    matchResource.AddResource("home-team", homeTeamResource);

                    var awayTeamResource = teamMapper.Map(matchForCurrentTeam.AwayTeam, TeamMapper.TeamName, TeamMapper.LeagueName, TeamMapper.CurrentLeaguePosition);
                    matchResource.AddResource("away-team", awayTeamResource);

                    matchResource.AddResource("your-opponent", gameInfo.CurrentTeamId == matchForCurrentTeam.HomeTeamId ? awayTeamResource : homeTeamResource);

                    matchDayResource.AddResource("next-match", matchResource);
                }

                // Only add the play next match day form when the matches are right now.
                if (nextMatchDate.Value == now.DateTime)
                {
                    var playNextMatchDayForm = matchDayResourceFactory.GetForm();
                    matchDayResource.AddForm(playNextMatchDayForm);
                }

                halDocument.AddResource("rel:next-match-day", matchDayResource);
            }

            // Add league table.
            var leagueTableService = ServiceFactory.CreateLeagueTableService(gameInfo);
            var leagueTable = leagueTableService.GetBySeasonAndCompetition(currentSeason.Id, gameInfo.CurrentTeam.CurrentLeagueCompetitionId);
            var leagueTableResource = new LeagueTableMapper(UriHelper).Map(leagueTable);
            leagueTableResource.AddLink("leaguetables", new Link(UriHelper.GetSeasonLeagueTablesUri(gameId, currentSeason.Id)) { Name = "all", Title = "All league tables" });

            halDocument.AddResource("rel:leaguetable", leagueTableResource);

            var response = GetResponse(halDocument);
            return response;
        }

        public Response DeleteItem(string gameId)
        {
            RequestHelper.ValidateId(gameId);
            GetGameInfo(gameId);

            var gameService = ServiceFactory.CreateGameService();
            gameService.DeleteGame(gameId);

            return new Response(HttpStatusCode.NoContent);
        }

        public Response DeleteCollection()
        {
#if !DEBUG
                return new Response(HttpStatusCode.MethodNotAllowed);
#endif

            var param = Request.GetQueryStringValue("param");

            string expectedTimestamp = DateTime.Now.ToString("yyyyMMddHHmm");
            if (string.IsNullOrWhiteSpace(param) || param != expectedTimestamp)
            {
                return new Response(HttpStatusCode.MethodNotAllowed);
            }

            var gameService = ServiceFactory.CreateGameService();
            gameService.DeleteAllGames();

            return new Response(HttpStatusCode.NoContent);
        }

        public Response Post()
        {
            try
            {
                var gameCreationManager = new GameCreationManager();
                var game = gameCreationManager.CreateGame();

                string locationUri = UriHelper.GetGameUri(game.Id);
                var response = new CreatedResponse(locationUri);

                return response;
            }
            catch (Exception exception)
            {
                Logger.LogWarning($"Creating the game failed: '{exception.Message}' {exception}");
                throw ResponseHelper.Get500InternalServerError("Creating the game failed");
            }
        }
    }
}