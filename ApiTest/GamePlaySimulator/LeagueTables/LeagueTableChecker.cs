using ApiTest.GamePlaySimulator.Controllers;
using ApiTest.GamePlaySimulator.Games;
using ApiTest.GamePlaySimulator.Hypermedia;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ApiTest.GamePlaySimulator.LeagueTables
{
    internal class LeagueTableChecker
    {
        private readonly GameHandler _gameHandler;
        private readonly ControllerHandler _requestHandler;
        private readonly string _seasonId;
        private readonly Links _links;

        public LeagueTableChecker(string gameId)
        {
            _gameHandler = new GameHandler(gameId);
            _links = new Links(_gameHandler);
            _requestHandler = new ControllerHandler(gameId);
        }

        public void CheckBeginOfSeason(string seasonId)
        {
            RequestLeagueTables(false, seasonId);

            throw new NotImplementedException();
        }

        public void CheckEndOfSeason(string seasonId)
        {
            RequestLeagueTables(true, seasonId);

            throw new NotImplementedException();
        }

        private void RequestLeagueTables(bool endOfSeason, string seasonId)
        {
            // Find "leaguetables" link
            var url = _links.GetLinkUrl(new LeagueTablesHandler());
            Assert.IsNotNull(url);

            // Follow link to get all league tables of the season.
            var response = _requestHandler.GetLeagueTables(seasonId);

            // Store 4 leaguetables in private field (depending on boolean argument)
            JArray leagueTables;
            using (var stream = response.MessageBody)
            {
                var json = stream.GetJson();
                leagueTables = (JArray)json["_embedded"]["rel:leaguetables"];
            }

            foreach (var thingy in leagueTables)
            {
//                var positions = leagueTables["bla"];
//                foreach (var position in positions)
//                {
//                    //enz...
//                }
            }
        }
    }
}
