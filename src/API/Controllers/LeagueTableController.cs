﻿using Shally.Hal;
using System.Collections.Generic;
using Dolores.Responses;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;
using TwoNil.API.Resources.TwoNil.API.Resources;
using TwoNil.Logic.Exceptions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Controllers
{

   public class LeagueTableController : ControllerBase
   {
      public Response GetBySeason(string gameId, string seasonId)
      {
         var game = GetGameInfo(gameId);

         RequestHelper.ValidateId(seasonId);

         //TODO Check season exists and belongs to user.
         //var seasonService = ServiceFactory.CreateSeasonService(game);
         //var season = seasonService.Get(seasonId);

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

         var mapper = new LeagueTableMapper(UriHelper);
         var leagueTableResources = new List<Resource>();
         foreach (var leagueTable in leagueTables)
         {
            var resource = mapper.Map(leagueTable, LeagueTableMapper.FullDetails);
            leagueTableResources.Add(resource);
         }

         var halDocument = CreateHalDocument(UriHelper.GetSeasonLeagueTablesUri(gameId, seasonId), game);

         var competitionService = ServiceFactory.CreateCompetitionService();
         var currentCompetitionName = competitionService.Get(game.CurrentTeam.CurrentLeagueCompetitionId).Name;
         halDocument.AddProperty("current-competition-name", currentCompetitionName);

         halDocument.AddResource("rel:leaguetables", leagueTableResources);

         var seasonListResourceFactory = new SeasonListResourceFactory(game, UriHelper, UriHelper.GetSeasonLeagueTablesUri(gameId, "###seasonid###"));
         halDocument.AddResource("rel:seasons", seasonListResourceFactory.Create());

         var response = GetResponse(halDocument);
         return response;
      }
   }
}