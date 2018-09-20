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

        [TestMethod]
        public void StartGamePlaySimulation()
        {
            // =========== CONFIG ===============================================================================
            // Note that it might not always be necessary to execute this simulation when running all unit tests
            const bool enableSimulation = true;
            const string gameId = "nczmy";
            const int howManySeasons = 1;
            // ==================================================================================================

            if (!enableSimulation)
                return;

            Start(gameId, howManySeasons);
        }


        public void Start(string gameId, int howManySeasons)
        {
            if (string.IsNullOrWhiteSpace(gameId))
                throw new ArgumentException("GameId can not be empty");

            if (howManySeasons < 1)
                throw new ArgumentException("HowManySeasons must be at least 1");

            _gameHandler = new GameHandler(gameId);
            _leagueTableChecker = new LeagueTableChecker(gameId);
            _howManySeasons = howManySeasons;
            _forms = new Forms(_gameHandler);
            _links = new Links(_gameHandler);

            PlaySeasons();
        }

        private void PlaySeasons()
        {
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
                    // Navigating to the next GameDateTime is not yet possible, there are no matches to play,
                    // so the only possibility now is that the season can be ended.
                    string endSeasonUrl = _forms.GetFormUrl(new EndOfSeasonHandler());
                    Assert.IsNotNull(endSeasonUrl);
                    //CheckLeagueTableOfCurrentSeason(endSeasonUrl);
                    EndSeason(endSeasonUrl);
                    //CheckLeagueTableOfNewSeason();

                    seasonEnded = true;
                }
            }
        }

        private void NavigateToNextDateTimeIfPossible(string navigateToNextDateTimeUrl)
        {
            new GameDateTimeHandler().InvokeControllerMethod(navigateToNextDateTimeUrl);
        }

        private void CheckLeagueTableOfNewSeason()
        {
            // _leagueTableChecker.CheckBeginOfSeason();
        }

        private void CheckLeagueTableOfCurrentSeason(string endSeasonUrl)
        {
            var parsedUrl = UrlParser.Parse(endSeasonUrl);
            _leagueTableChecker.CheckEndOfSeason(parsedUrl["seasons"]);
        }

        private static void EndSeason(string endSeasonUrl)
        {
            new EndOfSeasonHandler().InvokeControllerMethod(endSeasonUrl);
        }

        private static void PlayMatches(string nextMatchUrl)
        {
            new NextMatchDayHandler().InvokeControllerMethod(nextMatchUrl);
        }
    }
}
