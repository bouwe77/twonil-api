using Dolores;
using Dolores.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;
using TwoNil.Logic.Exceptions;
using TwoNil.Services;

namespace TwoNil.API.Controllers
{
    public class GameDateTimeController : ControllerBase
    {
        public GameDateTimeController(ServiceFactory serviceFactory, UriHelper uriHelper)
            : base(serviceFactory, uriHelper)
        {

        }
        public Response PostEnd(string gameId)
        {
            RequestHelper.ValidateId(gameId);
            var gameInfo = GetGameInfo(gameId);

            var service = ServiceFactory.CreateGameDateTimeService(gameInfo);

            try
            {
                service.NavigateToNext();
            }
            catch (BusinessLogicException businessLogicException)
            {
                throw Handle(businessLogicException);
            }

            var now = service.GetNow();

            string locationUri = UriHelper.GetPresentGameDateTimeUri(gameId, now.Id);
            return new CreatedResponse(locationUri);
        }
    }
}
