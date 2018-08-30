using ApiTest.GamePlaySimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;

namespace ApiTest
{
    internal class Simulator
    {
        private readonly string _gameId;
        private readonly int _howManySeasons;

        public Simulator(string gameId, int howManySeasons)
        {
            if (string.IsNullOrWhiteSpace(gameId))
                throw new ArgumentException("GameId can not be empty");

            if (howManySeasons < 1)
                throw new ArgumentException("HowManySeasons must be at least 1");

            _gameId = gameId;
            _howManySeasons = howManySeasons;
        }

        public void Start()
        {
            for (int i = 0; i < _howManySeasons; i++)
            {
                PlaySeason();
            }
        }

        private void PlaySeason()
        {
            bool matchesFound = true;

            while (matchesFound)
            {
                string nextMatchUrl = GetNextMatchUrl();
                matchesFound = nextMatchUrl != null;

                if (matchesFound)
                    PlayMatches(nextMatchUrl);
            }

            string endSeasonUrl = GetEndSeasonUrl();
            Assert.IsNotNull(endSeasonUrl);
            EndSeason(endSeasonUrl);
        }

        private string GetFormUrl(IFormHandler formHandler)
        {
            var requestHandler = new RequestHandler(_gameId);

            // GET dasboard /games/:gameId
            var gameResponse = requestHandler.GetGame();

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


        private string GetNextMatchUrl()
        {
            return GetFormUrl(new NextMatchDayHandler());
        }

        private string GetEndSeasonUrl()
        {
            return GetFormUrl(new EndOfSeasonHandler());
        }

        private void EndSeason(string url)
        {
            new EndOfSeasonHandler().InvokeControllerMethod(url);
        }

        private void PlayMatches(string url)
        {
            new NextMatchDayHandler().InvokeControllerMethod(url);
        }
    }
}