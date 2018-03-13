using Dolores.Http;
using Dolores.Responses;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;
using TwoNil.Logic.Exceptions;

namespace TwoNil.API.Controllers
{
   public class SeasonController : ControllerBase
   {
      public Response PostEndSeasonItem(string gameId, string seasonId)
      {
         var game = GetGameInfo(gameId);

         RequestHelper.ValidateId(seasonId);

         var seasonService = ServiceFactory.CreateSeasonService(game);

         try
         {
            bool seasonEnded = seasonService.DetermineSeasonEnded(seasonId);
            if (!seasonEnded)
            {
               throw ResponseHelper.Get400BadRequest("The season is not finished yet");
            }
         }
         catch (BusinessLogicException businessLogicException)
         {
            throw Handle(businessLogicException);
         }

         seasonService.EndSeasonAndCreateNext(seasonId);

         var seasonUri = UriHelper.GetSeasonUri(gameId, seasonId);

         var response = new Response(HttpStatusCode.Ok);
         response.Headers.Add("Location", seasonUri);

         return response;
      }

      public Response GetSeasonItem(string gameId, string seasonId)
      {
         RequestHelper.ValidateId(gameId);

         var gameInfo = GetGameInfo(gameId);
         RequestHelper.ValidateId(seasonId);

         var seasonService = ServiceFactory.CreateStatisticsService(gameInfo);
         var seasonStatistics = seasonService.GetSeasonStatistics(seasonId);

         if (seasonStatistics == null)
         {
            throw ResponseHelper.Get404NotFound($"Season with ID '{seasonId}' not found");
         }

         var halDocument = CreateHalDocument(UriHelper.GetSeasonUri(gameId, seasonId), gameInfo);

         var resource = new SeasonStatisticsMapper(UriHelper).Map(
            seasonStatistics,
            SeasonStatisticsMapper.SeasonName,
            SeasonStatisticsMapper.NationalChampion,
            SeasonStatisticsMapper.NationalChampionRunnerUp,
            SeasonStatisticsMapper.NationalCupWinner,
            SeasonStatisticsMapper.NationalCupRunnerUp);
         halDocument.AddResource("rel:season", resource);

         var response = GetResponse(halDocument);
         return response;
      }

      public Response GetSeasonTeamStats(string gameId, string seasonId, string teamId)
      {
         return new Response(HttpStatusCode.NotImplemented);
      }
   }
}