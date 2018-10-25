using Dolores.Responses;
using TwoNil.API.Resources;
using TwoNil.Services;

namespace TwoNil.API.Controllers
{
    internal class HomeController : ControllerBase
    {
        public HomeController(ServiceFactory serviceFactory, UriHelper uriHelper)
            : base(serviceFactory, uriHelper)
        {
        }

        public Response Home()
        {
            var halDocument = CreateHalDocument(UriHelper.GetHomeUri());
            var response = GetResponse(halDocument);
            return response;
        }
    }
}