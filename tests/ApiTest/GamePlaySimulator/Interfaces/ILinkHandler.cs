using Newtonsoft.Json.Linq;

namespace ApiTest.GamePlaySimulator.Interfaces
{
    public interface ILinkHandler
    {
        JToken GetLink(JObject json);
    }
}
