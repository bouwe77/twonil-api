using Dolores.Responses;
using Microsoft.Extensions.Logging;
using TwoNil.API.Resources;

namespace TwoNil.API.Controllers
{
   internal class HomeController : ControllerBase
   {
      public Response Home()
      {
         Logger.LogInformation("==========> hoi :)");
         var halDocument = CreateHalDocument(UriFactory.GetHomeUri());
         var response = GetResponse(halDocument);
         return response;
      }
   }
}