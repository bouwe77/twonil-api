using ApiTest.GamePlaySimulator.Hypermedia;
using ApiTest.GamePlaySimulator.Interfaces;
using Dolores.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TwoNil.API.Controllers;

namespace ApiTest.GamePlaySimulator.Matches
{
    public class NextMatchDayHandler : IFormHandler, IUrlParser
    {
        public void InvokeControllerMethod(string url)
        {
            var parsedUrl = UrlParser.Parse(url);

            var response = new MatchController().PostPlayDayMatches(parsedUrl["games"], parsedUrl["days"]);
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
