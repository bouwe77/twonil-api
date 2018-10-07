using ApiTest.GamePlaySimulator.Controllers;
using ApiTest.GamePlaySimulator.Games;
using ApiTest.GamePlaySimulator.Hypermedia;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiTest.GamePlaySimulator.LeagueTables
{
    internal class LeagueTableChecker
    {
        private readonly GameHandler _gameHandler;
        private readonly ControllerHandler _requestHandler;
        private readonly Links _links;
        private readonly bool _checkEnabled;
        private List<string> _leagueTablePreviousSeason = new List<string>();

        public LeagueTableChecker(string gameId, bool checkEnabled)
        {
            _gameHandler = new GameHandler(gameId);
            _links = new Links(_gameHandler);
            _requestHandler = new ControllerHandler(gameId);
            _checkEnabled = checkEnabled;
        }

        public void CompareLeagueTables(string seasonId)
        {
            if (!_checkEnabled)
                return;

            var newLeagueTablePositions = RequestLeagueTables(seasonId);

            // Compare new league table with saved league table of previous season
            CheckNewLeagueTable(newLeagueTablePositions);
        }

        private void CheckNewLeagueTable(IEnumerable<string> newLeagueTablePositions)
        {
            // Based on the previous league table: determine new positions.
            var expected = _leagueTablePreviousSeason;

            var temp = expected[3];
            expected[3] = expected[4];
            expected[4] = temp;

            temp = expected[7];
            expected[7] = expected[8];
            expected[8] = temp;

            temp = expected[11];
            expected[11] = expected[12];
            expected[12] = temp;

            //Assert these determined positions against the new one.
            Assert.IsTrue(newLeagueTablePositions.SequenceEqual(expected), "There is a serious bug in the promotion/relegation algorythm");
        }

        public void SaveEndOfSeason(string seasonId)
        {
            if (!_checkEnabled)
                return;

            _leagueTablePreviousSeason = RequestLeagueTables(seasonId);
        }

        private List<string> RequestLeagueTables(string seasonId)
        {
            var teams = new List<string>();

            // Find "leaguetables" link
            var url = _links.GetLinkUrl(new LeagueTablesHandler());
            Assert.IsNotNull(url);

            // Follow link to get all league tables of the season.
            var response = _requestHandler.GetLeagueTables(seasonId);

            // Store 4 leaguetables in private field (depending on boolean argument)
            var leagueTables = new List<JArray>();
            using (var stream = response.MessageBody)
            {
                var json = stream.GetJson();

                for (int i = 0; i < 4; i++)
                    leagueTables.Add((JArray)json["_embedded"]["rel:leaguetables"][i]["_embedded"]["positions"]);
            }

            // Create a list of TeamIds over all leagues ordered by their position in the league.
            foreach (var leagueTable in leagueTables)
            {
                foreach (var position in leagueTable)
                {
                    var teamId = position["_embedded"]["team"]["_links"]["self"]["href"].ToString();
                    teams.Add(teamId);
                }
            }

            return teams;
        }
    }
}
