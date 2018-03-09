using System.Collections.Generic;
using Shally.Hal;
using TwoNil.Logic.Services;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class HalDocumentFactory
   {
      private readonly bool _loggedIn;
      private readonly UriHelper _uriHelper;

      public HalDocumentFactory(bool loggedIn, UriHelper uriHelper)
      {
         _loggedIn = loggedIn;
         _uriHelper = uriHelper;
      }

      public Resource Create(string selfUri)
      {
         return Create(selfUri, null);
      }

      public Resource Create(string selfUri, GameInfo gameInfo)
      {
         // Create the HAL document with the given self URI.
         var halDocument = new Resource(new Link(selfUri));

         // Add the curies link.
         var curiesLink = new Link(_uriHelper.GetRelationsUri()) { Templated = true };
         halDocument.AddLink("curies", curiesLink);

         var mainLinks = new List<Link>();

         // Add the home link to the main links.
         var homeLink = new Link(_uriHelper.GetHomeUri()) { Title = "Home" };
         mainLinks.Add(homeLink);

         if (_loggedIn)
         {
            // Add the Games link.
            var gamesLink = new Link(_uriHelper.GetGamesUri()) { Title = "Games" };
            mainLinks.Add(gamesLink);

            // Add game specific links.
            if (gameInfo != null)
            {
               CreateGameMenu(gameInfo, halDocument);
            }
         }

         halDocument.AddLink("main", mainLinks);

         return halDocument;
      }

      private void CreateGameMenu(GameInfo gameInfo, Resource halDocument)
      {
         var serviceFactory = new ServiceFactory();

         // The manager's current team. This is a link that is not part of the game menu.
         var currentTeamLink = new Link(_uriHelper.GetTeamUri(gameInfo.Id, gameInfo.CurrentTeamId));
         halDocument.AddLink("rel:managers-team", currentTeamLink);

         // Create the game menu.
         var gameLinks = new List<Link>();

         // The game dashboard.
         var gameLink = new Link(_uriHelper.GetGameUri(gameInfo.Id)) { Title = "Dashboard" };
         gameLinks.Add(gameLink);

         // The manager's squad.
         var teamPlayersLink = new Link(_uriHelper.GetTeamPlayersUri(gameInfo.Id, gameInfo.CurrentTeamId)) { Title = "Squad" };
         gameLinks.Add(teamPlayersLink);

         // League tables for the current season.
         var seasonService = serviceFactory.CreateSeasonService(gameInfo);
         var currentSeason = seasonService.GetCurrentSeason();
         var leagueTableLink = new Link(_uriHelper.GetSeasonLeagueTablesUri(gameInfo.Id, currentSeason.Id)) { Title = "League Tables" };
         gameLinks.Add(leagueTableLink);

         // Matches of the manager's team in the current season.
         var teamMatchesLink = new Link(_uriHelper.GetSeasonTeamMatchesUri(gameInfo.Id, currentSeason.Id, gameInfo.CurrentTeamId)) { Title = "Matches" };
         gameLinks.Add(teamMatchesLink);

         // Other teams, starting with the manager's team.
         var otherTeamsLink = new Link(_uriHelper.GetTeamUri(gameInfo.Id, gameInfo.CurrentTeamId)) { Title = "Other Teams" };
         gameLinks.Add(otherTeamsLink);

         halDocument.AddLink("game", gameLinks);
      }
   }
}