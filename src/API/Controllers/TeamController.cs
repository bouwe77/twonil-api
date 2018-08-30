using Shally.Hal;
using System.Collections.Generic;
using Dolores.Responses;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;

namespace TwoNil.API.Controllers
{
   public class TeamController : ControllerBase
   {
      public Response GetTeamItem(string gameId, string teamId)
      {
         var gameInfo = GetGameInfo(gameId);

         RequestHelper.ValidateId(teamId);

         var teamService = ServiceFactory.CreateTeamService(gameInfo);
         var team = teamService.GetTeam(teamId);
         if (team == null)
         {
            throw ResponseHelper.Get404NotFound($"Team ID '{teamId}' not found");
         }

         var halDocument = CreateHalDocument(UriHelper.GetTeamUri(gameId, teamId), gameInfo);

         var teamMapper = new TeamMapper(UriHelper);

         var teamListResourceFactory = new TeamListResourceFactory(gameInfo, UriHelper, UriHelper.GetTeamUri(gameId, "###teamid###"));
         halDocument.AddResource("rel:teams", teamListResourceFactory.Create());

         var teamResource = teamMapper.Map(team, TeamMapper.TeamName, TeamMapper.Rating, TeamMapper.RatingPercentage);
         halDocument.AddResource("rel:team", teamResource);

         var seasonService = ServiceFactory.CreateSeasonService(gameInfo);
         var currentSeason = seasonService.GetCurrentSeason();

         var statisticsService = ServiceFactory.CreateStatisticsService(gameInfo);
         var seasonTeamStatistics = statisticsService.GetSeasonTeamStatistics(currentSeason.Id, teamId);
         var seasonTeamStatisticsResource = new SeasonTeamStatisticsMapper(UriHelper).Map(
            seasonTeamStatistics,
            SeasonTeamStatisticsMapper.SeasonShortName,
            SeasonTeamStatisticsMapper.SeasonLongName,
            SeasonTeamStatisticsMapper.LeagueName,
            SeasonTeamStatisticsMapper.CurrentLeagueTablePosition,
            SeasonTeamStatisticsMapper.LeagueTablePositions,
            SeasonTeamStatisticsMapper.MatchResults);
         halDocument.AddResource("rel:season-team-statistics", seasonTeamStatisticsResource);

         var response = GetResponse(halDocument);
         return response;
      }

      //[HttpPost]
      // Example request:
      // HTTP POST to
      //                   https://localhost:44365/games/{gameId}/users/{userId}/team
      // With request body:
      //                   { "ChosenTeam": "https://localhost:44365/teams/LYEim" }
      //
      //public Response PostTeamToManage(string gameId, string userId, HttpRequestMessage request)
      //{
      //   using (var log = Logger.GetLogger(this, gameId, nameof(request)))
      //   {
      //      RequestHelper.ValidateId(gameId);
      //      RequestHelper.ValidateId(userId);

      //      Game game;
      //      bool gameBelongsToUser = TryFindGameForCurrentUser(gameId, out game);
      //      if (!gameBelongsToUser)
      //      {
      //         throw ResponseHelper.Get404NotFound($"Game ID '{gameId}' not found");
      //      }

      //      bool userIdIsLoggedInUser = User.Identity.Id.Equals(userId);
      //      if (!userIdIsLoggedInUser)
      //      {
      //         throw ResponseHelper.Get404NotFound($"User ID '{userId}' not found");
      //      }

      //      const string invalidRequestBodyError = "Invalid request body";
      //      if (request?.Content == null)
      //      {
      //         throw ResponseHelper.Get400BadRequest(invalidRequestBodyError);
      //      }

      //      ChosenTeamResource chosenTeamResource;
      //      try
      //      {
      //         var jsonString = request.Content.ReadAsStringAsync();
      //         chosenTeamResource = JsonConvert.DeserializeObject<ChosenTeamResource>(jsonString.Result);
      //      }
      //      catch (Exception exception)
      //      {
      //         log.Error($"Exception when parsing ChosenTeam request: {exception}");
      //         throw ResponseHelper.Get400BadRequest(invalidRequestBodyError);
      //      }

      //      // Extract the teamId from the posted team URI.
      //      string chosenTeamId = LinkHelper.GetIdFromUri(chosenTeamResource.ChosenTeam, @"\/teams\/(.*)");
      //      RequestHelper.ValidateId(chosenTeamId);

      //      // Get the team from the database.
      //      var serviceFactory = new ServiceFactory();
      //      var teamService = serviceFactory.CreateTeamService(game);
      //      var chosenTeam = teamService.GetTeam(chosenTeamId);

      //      // Determine team is found and belongs to current game.
      //      if (chosenTeam == null || game.Id != chosenTeam.GameId)
      //      {
      //         throw ResponseHelper.Get404NotFound($"Team with ID '{chosenTeamResource.ChosenTeam}' not found");
      //      }

      //      // Everything is OK, save the team as the current team for the game.
      //      var gameService = serviceFactory.CreateGameService();
      //      gameService.AddChosenTeam(gameId, chosenTeam);

      //      // Return a response containing a location with a link to the game.
      //      // LET OP Deze locatie verwijst niet naar hetgeen daadwerkelijk gecreeerd is, wellicht dat dit een redirect moet worden of zo? maar ik vind het nu wel even best
      //      var gameLink = UriHelper.GetGameUri(gameId);
      //      var location = new Uri(gameLink);

      //      return new HttpResponseMessage(HttpStatusCode.Created)
      //      {
      //         Headers = { Location = location }
      //      };
      //   }
      //}

      //public Response PostSubstitutePlayers(string gameId, string teamId, HttpRequestMessage request)
      //{
      //   using (var log = Logger.GetLogger(this, gameId, teamId, nameof(request)))
      //   {
      //      //return new HttpResponseMessage(HttpStatusCode.NotImplemented);

      //      RequestHelper.ValidateId(gameId);
      //      RequestHelper.ValidateId(teamId);

      //      // Check game exists and belongs to user.
      //      Game game;
      //      bool gameExists = TryFindGameForCurrentUser(gameId, out game);
      //      if (!gameExists)
      //      {
      //         throw ResponseHelper.Get404NotFound($"Game with ID '{gameId}' not found");
      //      }

      //      // Check the team exists, belongs to the game and the user is manager of the team.
      //      var teamService = ServiceFactory.CreateTeamService(game);
      //      var team = teamService.GetTeam(teamId);
      //      bool teamFound = team != null && game.Id == team.GameId;
      //      bool userIsManagerOfTeam = game.CurrentTeamId == teamId;

      //      if (!teamFound || !userIsManagerOfTeam)
      //      {
      //         throw ResponseHelper.Get404NotFound($"Game with ID '{game.Id}' and/or Team with ID '{teamId}' not found");
      //      }

      //      // Parse the request to determine whether it contains two playerIds.
      //      const string requestBodyInvalidError = "Request body is invalid";
      //      if (request?.Content == null)
      //      {
      //         throw ResponseHelper.Get400BadRequest(requestBodyInvalidError);
      //      }

      //      SubstitutePlayersResource substitutePlayersResource;
      //      try
      //      {
      //         var jsonString = request.Content.ReadAsStringAsync();
      //         substitutePlayersResource = JsonConvert.DeserializeObject<SubstitutePlayersResource>(jsonString.Result);
      //      }
      //      catch (Exception exception)
      //      {
      //         log.Error($"Error when parsing substitution request: {exception}");
      //         throw ResponseHelper.Get400BadRequest(requestBodyInvalidError);
      //      }

      //      string player2Id = null;
      //      string player1Id = null;

      //      if (!string.IsNullOrWhiteSpace(substitutePlayersResource?.Player1)
      //          && !string.IsNullOrWhiteSpace(substitutePlayersResource.Player2))
      //      {
      //         // Extract the playerId from the posted team URI.
      //         player1Id = LinkHelper.GetIdFromUri(substitutePlayersResource.Player1, @"\/players\/(.*)");
      //         RequestHelper.ValidateId(player1Id);

      //         // Extract the playerId from the posted team URI.
      //         player2Id = LinkHelper.GetIdFromUri(substitutePlayersResource.Player2, @"\/players\/(.*)");
      //         RequestHelper.ValidateId(player2Id);
      //      }

      //      // Check if the players exist and belong to the user.
      //      var playerService = new ServiceFactory().CreatePlayerService(game);
      //      var player1 = playerService.GetPlayer(player1Id, teamId);
      //      if (player1 == null || player1.GameId != gameId)
      //      {
      //         throw ResponseHelper.Get404NotFound($"Player with ID '{player1Id}' does not exist in team '{teamId}'");
      //      }

      //      var player2 = playerService.GetPlayer(player2Id, teamId);
      //      if (player2 == null || player2.GameId != gameId)
      //      {
      //         throw ResponseHelper.Get404NotFound($"Player with ID '{player2Id}' does not exist in team '{teamId}'");
      //      }

      //      playerService.SubstitutePlayers(player1, player2);

      //      var response = new HttpResponseMessage(HttpStatusCode.Created);

      //      log.ReturnValue = response;
      //      return response;
      //   }
      //}
   }
}
