using System;
using Shally.Forms;
using Shally.Hal;

namespace TwoNil.API.Resources
{
   /// <summary>
   /// This is a factory class instead of a IResourceMapper implementation because this resource does not correspond with a DomainObject.
   /// </summary>
   public class MatchDayResourceFactory
   {
      private readonly UriHelper _uriHelper;
      private readonly string _gameId;
      private readonly string _matchDayId;

      public MatchDayResourceFactory(UriHelper uriHelper, string gameId, DateTime nextMatchDate)
      {
         _uriHelper = uriHelper;
         _gameId = gameId;
         _matchDayId = nextMatchDate.ToString("yyyyMMddHH");
      }

      public Resource Create()
      {
         return new Resource(new Link(_uriHelper.GetMatchDayUri(_gameId, _matchDayId)));
      }

      public Form GetForm()
      {
         var form = new Form("play-match-day")
         {
            Action = _uriHelper.GetMatchDayUri(_gameId, _matchDayId),
            Method = "post",
            Title = "Play matches"
         };

         return form;
      }

      public Link GetLink()
      {
         var link = new Link(_uriHelper.GetMatchDayUri(_gameId, _matchDayId));
         return link;
      }
   }
}