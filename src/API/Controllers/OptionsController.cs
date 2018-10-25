using Dolores.Http;
using Dolores.Responses;
using TwoNil.API.Resources;
using TwoNil.Services;

namespace TwoNil.API.Controllers
{
    public class OptionsController : ControllerBase
    {
        public OptionsController(ServiceFactory serviceFactory, UriHelper uriHelper)
            : base(serviceFactory, uriHelper)
        {
        }

        public Response Options()
        {
            return CreateCorsResponse();
        }

        public Response Options(string arg1)
        {
            return CreateCorsResponse();
        }

        public Response Options(string arg1, string arg2)
        {
            return CreateCorsResponse();
        }

        public Response Options(string arg1, string arg2, string arg3)
        {
            return CreateCorsResponse();
        }

        /// <summary>
        /// Use this method to make cross-origin POST requests possible.
        /// The browser will issue an OPTIONS request which is handled in this method so the browser will continue with the POST.
        /// See also https://stackoverflow.com/a/43881141
        /// </summary>
        /// <returns></returns>
        private Response CreateCorsResponse()
        {
            var response = new Response(HttpStatusCode.Ok);

            AddAccessControlAllowOriginHeader(response);
            response.SetHeader("Access-Control-Allow-Methods", "POST");
            response.SetHeader("Access-Control-Allow-Headers", "Content-Type, Authorization");

            return response;
        }
    }
}
