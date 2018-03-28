using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dolores.Http;
using Dolores.Responses;
using Shally.Hal;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;
using TwoNil.API.Resources.TwoNil.API.Resources;
using TwoNil.Logic.Exceptions;
using TwoNil.Logic.Services;
using TwoNil.Shared.DomainObjects;

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

         var matchService = ServiceFactory.CreateMatchService(game);

         try
         {
            matchService.PlayMatchDay(matchDay);
         }
         catch (BusinessLogicException businessLogicException)
         {
            throw Handle(businessLogicException);
         }

         // If the manager's team played a match the Location header URI is that match, otherwise it's the matches on that day.
         var locationUri = UriHelper.GetMatchDayUri(gameId, dayId);
         var matchForCurrentTeam = matchService.GetByMatchDayAndTeam(matchDay, game.CurrentTeamId);
         if (matchForCurrentTeam != null)
         {
            locationUri = UriHelper.GetMatchUri(gameId, matchForCurrentTeam.Id);
         }

         var response = new CreatedResponse(locationUri);
         return response;
      }

      public Response GetDayMatches(string gameId, string dayId)
      {
         var game = GetGameInfo(gameId);

         DateTime matchDay = ValidateAndParseMatchDay(dayId);

         var halDocument = CreateHalDocument(UriHelper.GetMatchDayUri(gameId, dayId), game);
         var matchResources = GetDayMatchesResources(game, matchDay, out int _).ToList();

         if (!matchResources.Any())
         {
            throw ResponseHelper.Get404NotFound($"No matches found for match day '{dayId}'");
         }

         halDocument.AddResource("rel:matches-per-competition", matchResources);

         return GetResponse(halDocument);
      }

      public Response GetItem(string gameId, string matchId)
      {
         var gameInfo = GetGameInfo(gameId);

         RequestHelper.ValidateId(matchId);

         var matchService = ServiceFactory.CreateMatchService(gameInfo);
         var match = matchService.GetMatch(matchId);
         if (match == null)
         {
            throw ResponseHelper.Get404NotFound($"Match ID '{matchId}' not found");
         }

         var halDocument = CreateHalDocument(UriHelper.GetMatchUri(gameId, matchId), gameInfo);

         var matchMapper = new MatchMapper(UriHelper);

         var matchResource = matchMapper.Map(
            match,
            MatchMapper.HomeScore,
            MatchMapper.AwayScore,
            MatchMapper.PenaltiesTaken,
            MatchMapper.HomePenaltyScore,
            MatchMapper.AwayPenaltyScore);

         var teamMapper = new TeamMapper(UriHelper);
         var homeTeamResource = teamMapper.Map(match.HomeTeam, TeamMapper.TeamName);
         var awayTeamResource = teamMapper.Map(match.AwayTeam, TeamMapper.TeamName);
         matchResource.AddResource("home-team", homeTeamResource);
         matchResource.AddResource("away-team", awayTeamResource);

         halDocument.AddResource("rel:match", matchResource);

         // Add the other matches that are played on this match day. Unless there is only one match, then there's no need to add these matches.
         var matchesPerCompetition = GetDayMatchesResources(gameInfo, match.Date, out int numberOfMatches);
         if (numberOfMatches > 1)
         {
            halDocument.AddResource("rel:matches-per-competition", matchesPerCompetition);
         }

         var response = GetResponse(halDocument);
         return response;
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

         var teamListResourceFactory = new TeamListResourceFactory(game, UriHelper, UriHelper.GetSeasonTeamMatchesUri(gameId, seasonId, "###teamid###"));
         halDocument.AddResource("rel:teams", teamListResourceFactory.Create());

         var seasonListResourceFactory = new SeasonListResourceFactory(game, UriHelper, UriHelper.GetSeasonTeamMatchesUri(gameId, "###seasonid###", teamId));
         halDocument.AddResource("rel:seasons", seasonListResourceFactory.Create());

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

      private IEnumerable<Resource> GetDayMatchesResources(GameInfo game, DateTime matchDay, out int numberOfMatches1)
      {
         var matchService = ServiceFactory.CreateMatchService(game);

         var matches = matchService.GetByMatchDay(matchDay).ToList();

         var resourceFactory = new MatchesGroupedByCompetitionResourceFactory(UriHelper);

         var stuff = resourceFactory.Create(matches, game.Id, game.CurrentTeamId, out int numberOfMatches2);
         numberOfMatches1 = numberOfMatches2;

         return stuff;
      }
   }
}