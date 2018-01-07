using Dolores;
using Dolores.Http;
using Dolores.Responses;
using System.IO;
using System.Text;

namespace API.Controllers
{
   internal class HomeController : DoloresHandler
   {
      public Response Home()
      {
         var response = new Response(HttpStatusCode.Ok)
         {
            MessageBody = new MemoryStream(Encoding.UTF8.GetBytes("Hello World"))
         };

         response.SetContentTypeHeader("text/html");

         return response;
      }
   }
}