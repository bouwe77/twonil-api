using System.Collections.Generic;
using Shally.Hal;
using TwoNil.Services;
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

         // The manager's current team.
         if (gameInfo != null)
         {
            var currentTeamLink = new Link(_uriHelper.GetTeamUri(gameInfo.Id, gameInfo.CurrentTeamId));
            halDocument.AddLink("rel:managers-team", currentTeamLink);
         }

         //         var mainLinks = new List<Link>();

         // Add the home link to the main links.
         //         var homeLink = new Link(_uriHelper.GetHomeUri()) { Title = "Home" };
         //         mainLinks.Add(homeLink);

         //         if (_loggedIn)
         //         {
         // Add the Games link.
         //            var gamesLink = new Link(_uriHelper.GetGamesUri()) { Title = "Games" };
         //            mainLinks.Add(gamesLink);

         // Add game specific links.
         //            if (gameInfo != null)
         //            {
         //               CreateGameMenu(gameInfo, halDocument);
         //            }
         //         }

         //         halDocument.AddLink("main", mainLinks);

         return halDocument;
      }
   }
}