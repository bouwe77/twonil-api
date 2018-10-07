using System;
using ApiTest.GamePlaySimulator.Games;
using ApiTest.GamePlaySimulator.Interfaces;
using ApiTest.GamePlaySimulator.Matches;
using Newtonsoft.Json.Linq;

namespace ApiTest.GamePlaySimulator.Hypermedia
{
    public class Forms
    {
        private readonly GameHandler _gameHandler;

        public Forms(GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
        }

        public string GetFormUrl(IFormHandler formHandler)
        {
            var gameResponse = _gameHandler.GetGame();

            // Find Form
            JToken form;
            using (var stream = gameResponse.MessageBody)
            {
                var json = stream.GetJson();
                form = formHandler.GetForm(json);

                if (form == null)
                    return null;
            }

            return (string)form["action"];
        }
    }
}
