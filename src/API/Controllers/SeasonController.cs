using Dolores.Http;
using Dolores.Responses;
using System.Linq;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;
using TwoNil.API.Resources.TwoNil.API.Resources;
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

            var season = seasonService.Get(seasonId);
            if (season == null)
            {
                throw ResponseHelper.Get404NotFound($"Season '{seasonId}' does not exist");
            }

            var currentSeason = seasonService.GetCurrentSeason();
            if (!currentSeason.Equals(season))
            {
                throw ResponseHelper.Get409Conflict("This is not the current season");
            }

            bool seasonEnded = seasonService.DetermineSeasonEnded(season.Id);
            if (!seasonEnded)
            {
                throw ResponseHelper.Get400BadRequest("The season is not finished yet");
            }

            seasonService.EndSeasonAndCreateNext(seasonId);

            var seasonUri = UriHelper.GetSeasonUri(gameId, seasonId);

            var response = new Response(HttpStatusCode.Created);
            response.Headers.Add("Location", seasonUri);

            return response;
        }

        public Response GetSeasonItem(string gameId, string seasonId)
        {
            RequestHelper.ValidateId(gameId);
            RequestHelper.ValidateId(seasonId);

            var gameInfo = GetGameInfo(gameId);
            RequestHelper.ValidateId(seasonId);

            var statisticsService = ServiceFactory.CreateStatisticsService(gameInfo);
            var seasonStatistics = statisticsService.GetSeasonStatistics(seasonId);

            if (seasonStatistics == null)
            {
                throw ResponseHelper.Get404NotFound($"Season with ID '{seasonId}' not found");
            }

            var halDocument = CreateHalDocument(UriHelper.GetSeasonUri(gameId, seasonId), gameInfo);

            var resource = new SeasonStatisticsMapper(UriHelper).Map(seasonStatistics);
            halDocument.AddResource("rel:season", resource);

            var seasonListResourceFactory = new SeasonListResourceFactory(gameInfo, UriHelper, UriHelper.GetSeasonUri(gameId, "###seasonid###"));
            halDocument.AddResource("rel:seasons", seasonListResourceFactory.Create());

            var response = GetResponse(halDocument);
            return response;
        }

        public Response GetSeasonCollection(string gameId)
        {
            RequestHelper.ValidateId(gameId);

            var gameInfo = GetGameInfo(gameId);

            var seasonService = ServiceFactory.CreateSeasonService(gameInfo);
            var seasons = seasonService.GetAll();

            var halDocument = CreateHalDocument(UriHelper.GetSeasonsUri(gameId), gameInfo);

            var seasonListResourceFactory = new SeasonListResourceFactory(gameInfo, UriHelper, UriHelper.GetSeasonUri(gameId, "###seasonid###"));
            halDocument.AddResource("rel:seasons", seasonListResourceFactory.Create());

            var response = GetResponse(halDocument);
            return response;
        }
    }
}