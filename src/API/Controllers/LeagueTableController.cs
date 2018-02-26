using Sally.Hal;
using System.Collections.Generic;
using Dolores.Responses;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;
using TwoNil.Logic.Exceptions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Controllers
{

   public class LeagueTableController : ControllerBase
   {
      public Response GetBySeasonAndCompetition(string gameId, string seasonId, string competitionId)
      {
         var game = GetGameInfo(gameId);

         RequestHelper.ValidateId(seasonId);
         RequestHelper.ValidateId(competitionId);

         //TODO Check season exists and belongs to user.
         var seasonService = ServiceFactory.CreateSeasonService(game);
         var season = seasonService.Get(seasonId);

         // Check competition exists.
         var competitionService = ServiceFactory.CreateCompetitionService();
         var competition = competitionService.Get(competitionId);
         if (competition == null)
         {
            throw ResponseHelper.Get404NotFound($"Competition with id '{competitionId}' does not exist");
         }

         LeagueTable leagueTable;
         try
         {
            var leagueTableService = ServiceFactory.CreateLeagueTableService(game);
            leagueTable = leagueTableService.GetBySeasonAndCompetition(seasonId, competitionId);
         }
         catch (BusinessLogicException businessLogicException)
         {
            throw Handle(businessLogicException);
         }

         var leagueTableResource = new LeagueTableMapper().Map(leagueTable);

         var response = GetResponse(leagueTableResource);
         return response;
      }

      public Response GetBySeason(string gameId, string seasonId)
      {
         var game = GetGameInfo(gameId);

         RequestHelper.ValidateId(seasonId);

         //TODO Check season exists and belongs to user.
         var seasonService = ServiceFactory.CreateSeasonService(game);
         var season = seasonService.Get(seasonId);

         IEnumerable<LeagueTable> leagueTables;
         try
         {
            var leagueTableService = ServiceFactory.CreateLeagueTableService(game);
            leagueTables = leagueTableService.GetBySeason(seasonId);
         }
         catch (BusinessLogicException businessLogicException)
         {
            throw Handle(businessLogicException);
         }

         var mapper = new LeagueTableMapper();
         var leagueTableResources = new List<Resource>();
         foreach (var leagueTable in leagueTables)
         {
            var resource = mapper.Map(leagueTable);
            leagueTableResources.Add(resource);
         }

         var halDocument = CreateHalDocument(UriFactory.GetSeasonLeagueTablesUri(gameId, seasonId), game);

         var competitionService = ServiceFactory.CreateCompetitionService();
         var currentCompetitionName = competitionService.Get(game.CurrentTeam.CurrentLeagueCompetitionId).Name;
         halDocument.AddProperty("current-competition-name", currentCompetitionName);

         halDocument.AddResource("rel:leaguetables", leagueTableResources);

         var response = GetResponse(halDocument);
         return response;
      }
   }
}