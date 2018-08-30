using Dolores.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TwoNil.API.Controllers;

namespace ApiTest.GamePlaySimulator
{
    public class EndOfSeasonHandler : IFormHandler, IUrlParser
    {
        public void InvokeControllerMethod(string url)
        {
            // Example URL: /games/xa4pv/seasons/2w3rt
            var splittedUrl = url.Split('/', System.StringSplitOptions.RemoveEmptyEntries);
            string gameId = splittedUrl[1];
            string seasonId = splittedUrl[3];

            var response = new SeasonController().PostEndSeasonItem(gameId, seasonId);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        public JToken GetForm(JObject json)
        {
            return json["_embedded"]["rel:current-season"]["_forms"]["end-season"];
        }
    }
}
