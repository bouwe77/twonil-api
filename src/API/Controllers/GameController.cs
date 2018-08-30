﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dolores.Http;
using Dolores.Responses;
using Microsoft.Extensions.Logging;
using Shally.Hal;
using Shally.Forms;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;
using TwoNil.Logic.Functionality;
using TwoNil.Shared.DomainObjects;

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

            var form = new Form("delete-game");
            form.Action = UriHelper.GetGameUri(game.Id);
            form.Method = "delete";
            form.Title = "Delete";
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

         var teamResource = new TeamMapper(UriHelper).Map(gameInfo.CurrentTeam, TeamMapper.TeamName, TeamMapper.Rating, TeamMapper.RatingGoalkeeper, TeamMapper.RatingDefence, TeamMapper.RatingMidfield, TeamMapper.RatingAttack, TeamMapper.RatingPercentage);
         halDocument.AddResource("rel:my-team", teamResource);

         var seasonService = ServiceFactory.CreateSeasonService(gameInfo);
         var currentSeason = seasonService.GetCurrentSeason();
         var seasonResource = new SeasonMapper(UriHelper).Map(currentSeason, SeasonMapper.SeasonShortName, SeasonMapper.SeasonLongName);

         bool endOfSeason = seasonService.DetermineSeasonEnded(currentSeason.Id);
         if (endOfSeason)
         {
            var form = new Form("end-season");
            form.Action = UriHelper.GetSeasonUri(gameId, currentSeason.Id);
            form.Method = "post";
            form.Title = "END SEASON!";
            seasonResource.AddForm(form);
         }

         halDocument.AddResource("rel:current-season", seasonResource);

         var statisticsService = ServiceFactory.CreateStatisticsService(gameInfo);
         var seasonTeamStatistics = statisticsService.GetSeasonTeamStatistics(currentSeason.Id, gameInfo.CurrentTeamId);
         var seasonTeamStatisticsResource = new SeasonTeamStatisticsMapper(UriHelper).Map(
            seasonTeamStatistics,
            SeasonTeamStatisticsMapper.SeasonLongName,
            SeasonTeamStatisticsMapper.SeasonShortName,
            SeasonTeamStatisticsMapper.LeagueName,
            SeasonTeamStatisticsMapper.CurrentLeagueTablePosition,
            SeasonTeamStatisticsMapper.LeagueTablePositions,
            SeasonTeamStatisticsMapper.MatchResults);
         halDocument.AddResource("rel:season-team-statistics", seasonTeamStatisticsResource);

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

               matchResource.AddResource("your-opponent", gameInfo.CurrentTeam.Equals(matchForCurrentTeam.HomeTeam) ? awayTeamResource : homeTeamResource);

               matchDayResource.AddResource("next-match", matchResource);
            }

            var playNextMatchDayForm = matchDayResourceFactory.GetForm();
            matchDayResource.AddForm(playNextMatchDayForm);

            halDocument.AddResource("rel:next-match-day", matchDayResource);
         }

         var leagueTableService = ServiceFactory.CreateLeagueTableService(gameInfo);
         var leagueTable = leagueTableService.GetBySeasonAndCompetition(currentSeason.Id, gameInfo.CurrentTeam.CurrentLeagueCompetitionId);
         var leagueTableResource = new LeagueTableMapper(UriHelper).Map(leagueTable);
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