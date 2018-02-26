using Dolores.Http;
using Dolores.Responses;

namespace TwoNil.API.Controllers
{
   public class RelationController : ControllerBase
   {
      public Response GetRelationItem(string relationName)
      {
         return new Response(HttpStatusCode.NotImplemented);
      }
   }
}
