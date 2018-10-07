using ApiTest.GamePlaySimulator.Games;
using ApiTest.GamePlaySimulator.Interfaces;
using Newtonsoft.Json.Linq;

namespace ApiTest.GamePlaySimulator.Hypermedia
{
    public class Links
    {
        private GameHandler _gameHandler;

        public Links(GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
        }

        public string GetLinkUrl(ILinkHandler linkHandler)
        {
            var gameResponse = _gameHandler.GetGame();

            // Find Link
            JToken link;
            using (var stream = gameResponse.MessageBody)
            {
                var json = stream.GetJson();
                link = linkHandler.GetLink(json);

                if (link == null)
                    return null;
            }

            return (string)link["href"];
        }

    }
}
