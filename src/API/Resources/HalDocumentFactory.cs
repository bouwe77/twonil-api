using System.Collections.Generic;
using Sally.Hal;
using TwoNil.Logic.Services;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class HalDocumentFactory
   {
      private bool _loggedIn;

      public HalDocumentFactory(bool loggedIn)
      {
         _loggedIn = loggedIn;
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
         var curiesLink = new Link(UriFactory.GetRelationsUri()) { Templated = true };
         halDocument.AddLink("curies", curiesLink);

         var mainLinks = new List<Link>();

         // Add the home link to the main links.
         var homeLink = new Link(UriFactory.GetHomeUri()) { Title = "Home" };
         mainLinks.Add(homeLink);

         if (_loggedIn)
         {
            // Add the Games link.
            var gamesLink = new Link(UriFactory.GetGamesUri()) { Title = "Games" };
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
         var currentTeamLink = new Link(UriFactory.GetTeamUri(gameInfo.Id, gameInfo.CurrentTeamId));
         halDocument.AddLink("rel:managers-team", currentTeamLink);

         // Create the game menu.
         var gameLinks = new List<Link>();

         // The game dashboard.
         var gameLink = new Link(UriFactory.GetGameUri(gameInfo.Id)) { Title = "Dashboard" };
         gameLinks.Add(gameLink);

         // The manager's squad.
         var teamPlayersLink = new Link(UriFactory.GetTeamPlayersUri(gameInfo.Id, gameInfo.CurrentTeamId)) { Title = "Squad" };
         gameLinks.Add(teamPlayersLink);

         // League tables for the current season.
         var seasonService = serviceFactory.CreateSeasonService(gameInfo);
         var currentSeason = seasonService.GetCurrentSeason();
         var leagueTableLink = new Link(UriFactory.GetSeasonLeagueTablesUri(gameInfo.Id, currentSeason.Id)) { Title = "League Tables" };
         gameLinks.Add(leagueTableLink);

         // Matches of the manager's team in the current season.
         var teamMatchesLink = new Link(UriFactory.GetSeasonTeamMatchesUri(gameInfo.Id, currentSeason.Id, gameInfo.CurrentTeamId)) { Title = "Matches" };
         gameLinks.Add(teamMatchesLink);

         // Other teams, starting with the manager's team.
         var otherTeamsLink = new Link(UriFactory.GetTeamUri(gameInfo.Id, gameInfo.CurrentTeamId)) { Title = "Other Teams" };
         gameLinks.Add(otherTeamsLink);

         halDocument.AddLink("game", gameLinks);
      }
   }
}