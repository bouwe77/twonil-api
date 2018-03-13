using Dolores.Responses;

namespace TwoNil.API.Controllers
{
   internal class HomeController : ControllerBase
   {
      public Response Home()
      {
         var halDocument = CreateHalDocument(UriHelper.GetHomeUri());
         var response = GetResponse(halDocument);
         return response;
      }
   }
}