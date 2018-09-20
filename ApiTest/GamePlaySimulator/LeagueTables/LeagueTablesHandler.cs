using ApiTest.GamePlaySimulator.Interfaces;
using Newtonsoft.Json.Linq;

namespace ApiTest.GamePlaySimulator.LeagueTables
{
    public class LeagueTablesHandler : ILinkHandler
    {
        public JToken GetLink(JObject json)
        {
            return json["_embedded"]["rel:leaguetable"]["_links"]["leaguetables"];
        }
    }
}
