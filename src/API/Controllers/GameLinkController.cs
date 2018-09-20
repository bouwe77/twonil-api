using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dolores.Responses;
using Shally.Hal;
using TwoNil.API.Helpers;
using TwoNil.Logic.Services;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Controllers
{
    public class GameLinkController : ControllerBase
    {
        public Response GetCollection(string gameId)
        {
            RequestHelper.ValidateId(gameId);

            var gameInfo = GetGameInfo(gameId);

            var halDocument = CreateHalDocument(UriHelper.GetGameLinksUri(gameId), gameInfo);

            CreateGameMenu(gameInfo, halDocument);

            var response = GetResponse(halDocument);

            return response;
        }

        private void CreateGameMenu(GameInfo gameInfo, Resource halDocument)
        {
            var serviceFactory = new ServiceFactory();

            // Create the game menu.
            var gameLinks = new List<Link>();

            // The game dashboard.
            var gameLink = new Link(UriHelper.GetGameUri(gameInfo.Id)) { Title = "Dashboard" };
            gameLinks.Add(gameLink);

            /*



             // The manager's squad.
             var teamPlayersLink = new Link(UriHelper.GetTeamPlayersUri(gameInfo.Id, gameInfo.CurrentTeamId)) { Title = "Players" };
             gameLinks.Add(teamPlayersLink);

             // League tables for the current season.
             var seasonService = serviceFactory.CreateSeasonService(gameInfo);
             var currentSeason = seasonService.GetCurrentSeason();
             var leagueTableLink = new Link(UriHelper.GetSeasonLeagueTablesUri(gameInfo.Id, currentSeason.Id)) { Title = "League Tables" };
             gameLinks.Add(leagueTableLink);

             // Matches of the manager's team in the current season.
             var teamMatchesLink = new Link(UriHelper.GetSeasonTeamMatchesUri(gameInfo.Id, currentSeason.Id, gameInfo.CurrentTeamId)) { Title = "Matches" };
             gameLinks.Add(teamMatchesLink);

             // Other teams, starting with the manager's team.
             var otherTeamsLink = new Link(UriHelper.GetTeamUri(gameInfo.Id, gameInfo.CurrentTeamId)) { Title = "Other Teams" };
             gameLinks.Add(otherTeamsLink);

             // Seasons, starting with the current season.
             var seasonLink = new Link(UriHelper.GetSeasonUri(gameInfo.Id, currentSeason.Id)) { Title = "Seasons" };
             gameLinks.Add(seasonLink);


      */

            halDocument.AddLink("game", gameLinks);
        }
    }
}
