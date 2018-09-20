using Dolores;
using Dolores.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoNil.API.Helpers;
using TwoNil.Logic.Exceptions;

namespace TwoNil.API.Controllers
{
    public class GameDateTimeController : ControllerBase
    {
        public Response PostEnd(string gameId)
        {
            RequestHelper.ValidateId(gameId);
            var gameInfo = GetGameInfo(gameId);

            var service = ServiceFactory.CreateGameDateTimeService(gameInfo);

            try
            {
                service.EndNow();
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
