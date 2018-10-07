using System;
using System.Collections.Generic;

namespace ApiTest.GamePlaySimulator.Hypermedia
{
    internal class UrlParser
    {
        public static Dictionary<string, string> Parse(string url)
        {
            // Example URL: /games/xa4pv/days/0001101120/matches results in the following dictionary:
            // ["games"] = "xa4pv"
            // ["days"] = "0001101120"
            // ["matches"] = null

            var splittedUrl = url.Split('/', StringSplitOptions.RemoveEmptyEntries);

            var parsedUrl = new Dictionary<string, string>();
            for (int i = 0; i < splittedUrl.Length; i++)
            {
                if (i % 2 == 0)
                {
                    parsedUrl.Add(splittedUrl[i], null);
                }
                else
                {
                    parsedUrl[splittedUrl[i - 1]] = splittedUrl[i];
                }
            }

            return parsedUrl;
        }
    }
}
