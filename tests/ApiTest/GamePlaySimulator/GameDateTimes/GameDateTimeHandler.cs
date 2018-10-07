using ApiTest.GamePlaySimulator.Hypermedia;
using ApiTest.GamePlaySimulator.Interfaces;
using Dolores.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TwoNil.API.Controllers;

namespace ApiTest.GamePlaySimulator.GameDateTimes
{
    public class GameDateTimeHandler : IFormHandler, IUrlParser
    {
        public void InvokeControllerMethod(string url)
        {
            var parsedUrl = UrlParser.Parse(url);

            var response = new GameDateTimeController().PostEnd(parsedUrl["games"]);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        public JToken GetForm(JObject json)
        {
            var form = json["_embedded"]["rel:game-datetime-navigation"]["_forms"]["navigate-to-next-game-datetime"];

            Assert.IsNotNull(form);

            // If the form is not enabled, consider it to be not present, which is fine.
            bool enabled = (bool)json["_embedded"]["rel:game-datetime-navigation"]["_forms"]["navigate-to-next-game-datetime"]["enabled"];
            if (enabled)
                return form;

            return null;
        }
    }
}
