using Newtonsoft.Json.Linq;

namespace ApiTest.GamePlaySimulator.Interfaces
{
    public interface IFormHandler
    {
        JToken GetForm(JObject json);
    }
}
