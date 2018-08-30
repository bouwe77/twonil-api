using Newtonsoft.Json.Linq;

namespace ApiTest.GamePlaySimulator
{
public interface IFormHandler
    {
        JToken GetForm(JObject json);
    }
}
