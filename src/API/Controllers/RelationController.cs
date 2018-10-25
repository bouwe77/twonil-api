using Dolores.Http;
using Dolores.Responses;
using TwoNil.API.Resources;
using TwoNil.Services;

namespace TwoNil.API.Controllers
{
    public class RelationController : ControllerBase
    {
        public RelationController(ServiceFactory serviceFactory, UriHelper uriHelper)
            : base(serviceFactory, uriHelper)
        {
        }

        public Response GetRelationItem(string relationName)
        {
            return new Response(HttpStatusCode.NotImplemented);
        }
    }
}
