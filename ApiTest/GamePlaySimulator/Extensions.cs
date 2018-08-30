using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ApiTest.GamePlaySimulator
{
    public static class Extensions
    {
        public static JObject GetJson(this Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                return JObject.Parse(json);
            }
        }

        public static T DeserializeFromStream<T>(this Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                T deserializedObject = (T)serializer.Deserialize(jsonTextReader, typeof(T));
                return deserializedObject;
            }
        }
    }
}
