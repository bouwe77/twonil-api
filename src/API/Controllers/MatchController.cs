using System;
using System.Globalization;
using System.Linq;
using Dolores.Http;
using Dolores.Responses;
using Shally.Hal;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;
using TwoNil.Logic.Exceptions;

namespace TwoNil.API.Controllers
{
   public class MatchController : ControllerBase
   {
      /// <summary>
      /// Plays all matches on the given match day.
      /// </summary>
      public Response PostPlayDayMatches(string gameId, string dayId)
      {
         var game = GetGameInfo(gameId);

         DateTime matchDay = ValidateAndParseMatchDay(dayId);

         try
         {
            var matchService = ServiceFactory.CreateMatchService(game);
            matchService.PlayMatchDay(matchDay);
         }
         catch (BusinessLogicException businessLogicException)
         {
            throw Handle(businessLogicException);
         }

         var matchDayUri = UriHelper.GetMatchDayUri(gameId, dayId);

         var response = new Response(HttpStatusCode.Created);
         response.Headers.Add("Location", matchDayUri);

         return response;
      }

      public Response GetDayMatches(string gameId, string dayId)
      {
         var game = GetGameInfo(gameId);

         DateTime matchDay = ValidateAndParseMatchDay(dayId);

         var matchService = ServiceFactory.CreateMatchService(game);

         var matches = matchService.GetByMatchDay(matchDay).ToList();

         if (!matches.Any())
         {
            throw ResponseHelper.Get404NotFound($"No matches found for match day '{dayId}'");
         }

         var halDocument = CreateHalDocument(UriHelper.GetMatchDayUri(gameId, dayId), game);

         var resourceFactory = new MatchesGroupedByCompetitionResourceFactory(UriHelper);
         var matchResources = resourceFactory.Create(matches, gameId, game.CurrentTeamId);

         halDocument.AddResource("rel:matches-per-competition", matchResources);

         return GetResponse(halDocument);
      }

      public Response GetItem(string gameId, string matchId)
      {
         return new Response(HttpStatusCode.NotImplemented);
      }

      public Response GetTeamMatches(string gameId, string seasonId, string teamId)
      {
         var game = GetGameInfo(gameId);

         RequestHelper.ValidateId(seasonId);
         RequestHelper.ValidateId(teamId);

         // Check team exists.
         var teamService = ServiceFactory.CreateTeamService(game);
         var team = teamService.GetTeam(teamId);
         if (team == null)
         {
            throw ResponseHelper.Get404NotFound($"Team with ID '{teamId}' not found");
         }

         var matchService = ServiceFactory.CreateMatchService(game);

         var matches = matchService.GetTeamRoundMatches(teamId, seasonId, team.CurrentLeagueCompetitionId).ToList();
         if (!matches.Any())
         {
            throw ResponseHelper.Get404NotFound($"No matches found for seasonId '{seasonId}' and teamId '{teamId}'");
         }

         var halDocument = CreateHalDocument(UriHelper.GetSeasonTeamMatchesUri(gameId, seasonId, teamId), game);

         halDocument.AddLink("rel:matches-of-team", new Link(UriHelper.GetTeamUri(gameId, teamId)));

         var resourceFactory = new TeamMatchResourceFactory(UriHelper);
         var resources = resourceFactory.Create(matches, gameId, seasonId, teamId);
         halDocument.AddResource("rel:matches", resources);

         return GetResponse(halDocument);
      }

      private static DateTime ValidateAndParseMatchDay(string dayId)
      {
         bool parseSuccessful = DateTime.TryParseExact(dayId, "yyyyMMddHH", new CultureInfo("en-US"), DateTimeStyles.None, out var matchDay);
         if (!parseSuccessful)
         {
            throw ResponseHelper.Get400BadRequest($"Invalid 'days' querystring argument: {dayId}");
         }

         return matchDay;
      }
   }
}