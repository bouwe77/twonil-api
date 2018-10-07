using ApiTest.GamePlaySimulator.Interfaces;
using Newtonsoft.Json.Linq;

namespace ApiTest.GamePlaySimulator.Seasons
{
    public class CurrentSeasonHandler : ILinkHandler
    {
        public JToken GetLink(JObject json)
        {
            return json["_embedded"]["rel:current-season"]["_links"]["self"];
        }
    }
}
