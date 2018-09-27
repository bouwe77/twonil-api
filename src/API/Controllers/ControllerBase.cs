using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Dolores;
using Dolores.Exceptions;
using Dolores.Http;
using Dolores.Responses;
using Shally.Hal;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;
using TwoNil.Logic.Exceptions;
using TwoNil.Services;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Controllers
{
   /// <summary>
   /// Base class for all TwoNil ApiControllers to allow working with a current logged in user and its game.
   /// </summary>
   public abstract class ControllerBase : DoloresHandler
   {
      protected readonly ServiceFactory ServiceFactory;
      protected HalDocumentFactory HalDocumentFactory;
      //protected new virtual MyPrincipal User => HttpContext.Current == null ? null : HttpContext.Current.User as MyPrincipal;

      protected UriHelper UriHelper;

      protected ControllerBase()
      {
         ServiceFactory = new ServiceFactory();
         UriHelper = new UriHelper(RouteHelper);
      }

      internal GameInfo GetGameInfo(string gameId)
      {
         var gameService = ServiceFactory.CreateGameService();

         //TODO Let MODDERVOKKIN op
         string userId = "17eqhq";

         var gameInfo = gameService.GetGame(gameId, userId);

         if (gameInfo == null)
         {
            throw ResponseHelper.Get404NotFound($"Game '{gameId}' not found");
         }

         return gameInfo;
      }

      protected HttpException Handle(BusinessLogicException businessLogicException)
      {
         if (businessLogicException is NotFoundException)
         {
            return ResponseHelper.Get404NotFound(businessLogicException.Message);
         }

         if (businessLogicException is ConflictException)
         {
            return ResponseHelper.Get409Conflict(businessLogicException.Message);
         }

         return ResponseHelper.Get500InternalServerError("Unknown business logic exception");
      }

      protected Response GetResponse(Resource halDocument)
      {
         var response = new Response(Dolores.Http.HttpStatusCode.Ok)
         {
            MessageBody = new MemoryStream(Encoding.UTF8.GetBytes(halDocument.Json.ToString()))
         };

         response.SetContentTypeHeader("application/hal+json; charset=utf-8");
         AddAccessControlAllowOriginHeader(response);

         return response;
      }

      protected static void AddAccessControlAllowOriginHeader(Response response)
      {
         response.SetHeader(HttpResponseHeaderFields.AccessControlAllowOrigin, "http://localhost:3000");
      }

      protected Resource CreateHalDocument(string selfHref, GameInfo gameInfo = null)
      {
         //TODO Let MODDERVOKKIN op
         //bool loggedIn = User != null;
         bool loggedIn = true;

         var halDocumentFactory = new HalDocumentFactory(loggedIn, UriHelper);
         return halDocumentFactory.Create(selfHref, gameInfo);
      }
   }
}