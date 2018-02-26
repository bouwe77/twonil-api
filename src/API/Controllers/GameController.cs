﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dolores.Http;
using Dolores.Responses;
using Sally.Hal;
using Sally.Forms;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;

namespace TwoNil.API.Controllers
{
   public class GameController : ControllerBase
   {
      private readonly GameInfoMapper _gameInfoMapper;

      public GameController()
      {
         _gameInfoMapper = new GameInfoMapper();
      }

      public Response GetCollection()
      {
         var gameService = ServiceFactory.CreateGameService();

         //TODO Let MODDERVOKKIN op
         var games = gameService.GetGames("17eqhq").ToList();

         var halDocument = CreateHalDocument(UriFactory.GetGamesUri());

         var gameResources = new List<Resource>();
         foreach (var game in games)
         {
            var gameResource = _gameInfoMapper.Map(game, GameInfoMapper.GameName, GameInfoMapper.TeamName);

            var form = new Form("delete-game");
            form.Action = UriFactory.GetGameUri(game.Id);
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

         var halDocument = CreateHalDocument(UriFactory.GetGameUri(gameId), gameInfo);

         var teamResource = new TeamMapper().Map(gameInfo.CurrentTeam, TeamMapper.TeamName, TeamMapper.Rating, TeamMapper.RatingPercentage);
         halDocument.AddResource("rel:my-team", teamResource);

         var seasonService = ServiceFactory.CreateSeasonService(gameInfo);
         var currentSeason = seasonService.GetCurrentSeason();
         var seasonResource = new SeasonMapper().Map(currentSeason, SeasonMapper.SeasonName);

         bool endOfSeason = seasonService.DetermineSeasonEnded(currentSeason.Id);
         if (endOfSeason)
         {
            var form = new Form("end-season");
            form.Action = UriFactory.GetSeasonUri(gameId, currentSeason.Id);
            form.Method = "post";
            form.Title = "END SEASON!";
            seasonResource.AddForm(form);
         }

         halDocument.AddResource("rel:current-season", seasonResource);

         var statisticsService = ServiceFactory.CreateStatisticsService(gameInfo);
         var seasonTeamStatistics = statisticsService.GetSeasonTeamStatistics(currentSeason.Id, gameInfo.CurrentTeamId);
         var seasonTeamStatisticsResource = new SeasonTeamStatisticsMapper().Map(
            seasonTeamStatistics,
            SeasonTeamStatisticsMapper.SeasonName,
            SeasonTeamStatisticsMapper.LeagueName,
            SeasonTeamStatisticsMapper.CurrentLeagueTablePosition,
            SeasonTeamStatisticsMapper.LeagueTablePositions,
            SeasonTeamStatisticsMapper.MatchResults);
         halDocument.AddResource("rel:season-team-statistics", seasonTeamStatisticsResource);

         var matchService = ServiceFactory.CreateMatchService(gameInfo);
         var nextMatchDay = matchService.GetNextMatchDay(currentSeason.Id);

         if (nextMatchDay.HasValue)
         {
            string matchDayId = nextMatchDay.Value.ToString("yyyyMMddHH");
            var matchDayResource = MatchDayResourceFactory.Create(gameId, matchDayId);

            // Add a resource for the match of the current team.
            var matchForCurrentTeam = matchService.GetByMatchDayAndTeam(nextMatchDay.Value, gameInfo.CurrentTeamId);
            if (matchForCurrentTeam != null)
            {
               var matchResource = new MatchMapper().Map(
                  matchForCurrentTeam,
                  MatchMapper.CompetitionName,
                  MatchMapper.CompetitionType,
                  MatchMapper.Date,
                  MatchMapper.Round);

               if (matchForCurrentTeam.HomeTeam != null && matchForCurrentTeam.AwayTeam != null)
               {
                  var teamMapper = new TeamMapper();
                  var homeTeamResource = teamMapper.Map(matchForCurrentTeam.HomeTeam, TeamMapper.TeamName);
                  matchResource.AddResource("home-team", homeTeamResource);
                  var awayTeamResource = teamMapper.Map(matchForCurrentTeam.AwayTeam, TeamMapper.TeamName);
                  matchResource.AddResource("away-team", awayTeamResource);
               }

               matchDayResource.AddResource("next-match", matchResource);
            }

            var form = new Form("play-match-day");
            form.Action = UriFactory.GetMatchDayUri(gameId, matchDayId);
            form.Method = "post";
            form.Title = "Play matches";
            matchDayResource.AddForm(form);

            halDocument.AddResource("rel:next-match-day", matchDayResource);
         }

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
            var gameService = ServiceFactory.CreateGameService();

            //TODO Let MODDERVOKKIN op
            var game = gameService.CreateGameForUser("17eqhq");

            // No game is returned if there are no games available. Games need to be created with the GameCreator app.
            if (game == null)
            {
               throw ResponseHelper.GetHttpResponseException(HttpStatusCode.ServiceUnavailable, "At the moment new games can not be created. Sorry... :(");
            }

            var response = new Response(HttpStatusCode.Created);
            response.SetLocationHeader(UriFactory.GetGameUri(game.Id));
            return response;
         }
         catch (Exception)
         {
            throw ResponseHelper.Get500InternalServerError("Creating the game failed");
         }
      }
   }
}