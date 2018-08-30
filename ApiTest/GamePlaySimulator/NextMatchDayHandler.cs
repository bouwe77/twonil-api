using Dolores.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TwoNil.API.Controllers;

namespace ApiTest.GamePlaySimulator
{
    public class NextMatchDayHandler : IFormHandler, IUrlParser
    {
        public void InvokeControllerMethod(string url)
        {
            // Example URL: /games/xa4pv/days/0001101120/matches
            var splittedUrl = url.Split('/', System.StringSplitOptions.RemoveEmptyEntries);
            string gameId = splittedUrl[1];
            string dayId = splittedUrl[3];

            var response = new MatchController().PostPlayDayMatches(gameId, dayId);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        public JToken GetForm(JObject json)
        {
            if (json["_embedded"]["rel:next-match-day"] == null)
                return null;

            return json["_embedded"]["rel:next-match-day"]["_forms"]["play-match-day"];
        }
    }
}
