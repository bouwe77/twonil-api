using ApiTest.GamePlaySimulator.Hypermedia;
using ApiTest.GamePlaySimulator.Interfaces;
using Dolores.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TwoNil.API.Controllers;

namespace ApiTest.GamePlaySimulator.Seasons
{
    public class EndOfSeasonHandler : IFormHandler, IUrlParser
    {
        public void InvokeControllerMethod(string url)
        {
            var parsedUrl = UrlParser.Parse(url);

            var response = new SeasonController().PostEndSeasonItem(parsedUrl["games"], parsedUrl["seasons"]);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        public JToken GetForm(JObject json)
        {
            return json["_embedded"]["rel:current-season"]["_forms"]["end-season"];
        }
    }
}
