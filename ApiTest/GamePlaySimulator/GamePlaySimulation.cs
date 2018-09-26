using ApiTest.GamePlaySimulator.GameDateTimes;
using ApiTest.GamePlaySimulator.Games;
using ApiTest.GamePlaySimulator.Hypermedia;
using ApiTest.GamePlaySimulator.LeagueTables;
using ApiTest.GamePlaySimulator.Matches;
using ApiTest.GamePlaySimulator.Seasons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ApiTest.GamePlaySimulator
{
    [TestClass]
    public class GamePlaySimulation
    {
        private GameHandler _gameHandler;
        private LeagueTableChecker _leagueTableChecker;
        private int _howManySeasons;
        private Forms _forms;
        private Links _links;
        private string _currentSeasonId;

        [TestMethod]
        public void StartGamePlaySimulation()
        {
            const bool enableSimulation = false;

            var config = new Config(
                gameId: "1aiwp1",
                howManySeasons: 3,
                checkLeagueTables: true);

            if (!enableSimulation)
                return;

            Start(config);
        }

        public void Start(Config config)
        {
            if (string.IsNullOrWhiteSpace(config.GameId))
                throw new ArgumentException("GameId can not be empty");

            if (config.HowManySeasons < 1)
                throw new ArgumentException("HowManySeasons must be at least 1");

            _gameHandler = new GameHandler(config.GameId);
            _leagueTableChecker = new LeagueTableChecker(config.GameId, config.CheckLeagueTables);
            _howManySeasons = config.HowManySeasons;
            _forms = new Forms(_gameHandler);
            _links = new Links(_gameHandler);

            PlaySeasons();
        }

        private void PlaySeasons()
        {
            SetCurrentSeasonId();

            for (int i = 0; i < _howManySeasons; i++)
            {
                PlaySeason();
            }
        }

        private void PlaySeason()
        {
            bool seasonEnded = false;

            while (!seasonEnded)
            {
                // Navigate to the next GameDateTime if possible.
                string navigateToNextDateTimeUrl = _forms.GetFormUrl(new GameDateTimeHandler());
                if (navigateToNextDateTimeUrl != null)
                {
                    NavigateToNextDateTimeIfPossible(navigateToNextDateTimeUrl);
                    continue;
                }

                // Play matches, if applicable.
                string nextMatchUrl = _forms.GetFormUrl(new NextMatchDayHandler());
                if (nextMatchUrl != null)
                    PlayMatches(nextMatchUrl);
                else
                {
                    // Navigating to the next GameDateTime is not yet possible and there are no matches to play,
                    // so the only possibility now is that the season can be ended.
                    string endSeasonUrl = _forms.GetFormUrl(new EndOfSeasonHandler());
                    Assert.IsNotNull(endSeasonUrl);

                    SaveLeagueTableOfCurrentSeason();

                    EndSeason(endSeasonUrl);
                    seasonEnded = true;

                    SetCurrentSeasonId();
                    CompareLeagueTablesOfCurrentAndPreviousSeasons();
                }
            }
        }

        private void NavigateToNextDateTimeIfPossible(string navigateToNextDateTimeUrl)
        {
            new GameDateTimeHandler().InvokeControllerMethod(navigateToNextDateTimeUrl);
        }

        private void CompareLeagueTablesOfCurrentAndPreviousSeasons()
        {
            _leagueTableChecker.CompareLeagueTables(_currentSeasonId);
        }

        private void SaveLeagueTableOfCurrentSeason()
        {
            _leagueTableChecker.SaveEndOfSeason(_currentSeasonId);
        }

        private static void EndSeason(string endSeasonUrl)
        {
            new EndOfSeasonHandler().InvokeControllerMethod(endSeasonUrl);
        }

        private static void PlayMatches(string nextMatchUrl)
        {
            new NextMatchDayHandler().InvokeControllerMethod(nextMatchUrl);
        }

        private void SetCurrentSeasonId()
        {
            var currentSeasonHref = new Links(_gameHandler).GetLinkUrl(new CurrentSeasonHandler());
            var paredUrl = UrlParser.Parse(currentSeasonHref);
            _currentSeasonId = paredUrl["seasons"];
        }
    }
}
