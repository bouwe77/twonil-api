using System;
using System.Linq;
using Randomization;
using TwoNil.Data;
using TwoNil.Data.Repositories;
using TwoNil.Logic.Functionality.Competitions;
using TwoNil.Logic.Functionality.Players;
using TwoNil.Logic.Functionality.Teams;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality
{
    public class GameCreationManager
    {
        public Game CreateGame()
        {
            var game = new Game();

            var repositoryFactory = new RepositoryFactory(game.Id);
            var gameRepository = repositoryFactory.CreateGameRepository();
            try
            {
                // Create the Game, generate game data and save it to the database.
                gameRepository.CreateGame(game.Id);
                CreateGameData(repositoryFactory, game.Id);

                // ===================================================================
                //TODO, OK let MODDERVOKKIN op
                game.UserId = "17eqhq";
                // ===================================================================

                // Insert the game in the database.
                InsertGame(game);
            }
            catch (Exception exception)
            {
                gameRepository.DeleteGame(game.Id);
                throw;
            }

            return game;
        }

        private void InsertGame(Game game)
        {
            using (var gameRepository = new RepositoryFactory().CreateGameRepository())
            {
                gameRepository.InsertGame(game);
            }
        }

        private void CreateGameData(RepositoryFactory repositoryFactory, string gameId)
        {
            using (var transactionManager = repositoryFactory.CreateTransactionManager())
            {
                // Create GameInfo.
                var gameInfo = new GameInfo
                {
                    Name = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    Id = gameId
                };

                // Overrule the GameInfo Id with the Game Id.

                transactionManager.RegisterInsert(gameInfo);

                // Create teams and players.
                var teamsAndPlayers = CreateTeamsAndPlayers(repositoryFactory);

                // ===================================================================
                //TODO OK, let MODDERVOKKIN op
                var numberRandomizer = new NumberRandomizer();
                int randomIndex = numberRandomizer.GetNumber(0, Constants.HowManyTeamsPerLeague * Constants.HowManyLeagues - 1);
                gameInfo.CurrentTeam = teamsAndPlayers.Teams[randomIndex];
                // ===================================================================

                // Insert teams.
                transactionManager.RegisterInsert(teamsAndPlayers.Teams);

                // Insert players.
                transactionManager.RegisterInsert(teamsAndPlayers.Players);

                // Create a season with match schedules.
                var seasonManager = new SeasonManager(repositoryFactory);
                seasonManager.CreateFirstSeason(teamsAndPlayers.Teams, transactionManager);

                transactionManager.Save();
            }
        }

        private TeamsAndPlayers CreateTeamsAndPlayers(RepositoryFactory repositoryFactory)
        {
            var teamManager = new TeamManager(repositoryFactory);
            var playerManager = new PlayerManager();

            var teamsAndPlayers = new TeamsAndPlayers();

            // Generate all teams for this game.
            const int howManyTeams = Constants.HowManyTeamsPerLeague * Constants.HowManyLeagues;
            var teams = teamManager.Create(howManyTeams).ToList();

            // Assign team names.
            var teamNames = new TeamNameRepository().GetAll();
            for (int i = 0; i < teams.Count; i++)
            {
                teams[i].Name = teamNames[i];
            }

            teamsAndPlayers.Teams = teams;

            var averageRatingsPerLeague = new[] { 10, 40, 70, 100 };
            int averageRatingIndex = 0;

            var startingLineupGenerator = new StartingLineupGenerator();

            foreach (var team in teams)
            {
                int teamIndex = teams.IndexOf(team);
                if (teamIndex > 0 && teamIndex % Constants.HowManyLeagues == 0)
                {
                    averageRatingIndex++;
                }

                int currentAverageRating = averageRatingsPerLeague[averageRatingIndex];
                var players = playerManager.GenerateSquad(team, currentAverageRating).ToList();

                var players2 = startingLineupGenerator.GenerateStartingLineup(players, team.Formation);

                teamsAndPlayers.Players.AddRange(players2);

                teamManager.UpdateRating(team, players.Where(p => p.InStartingEleven).ToList());
            }

            return teamsAndPlayers;
        }
    }
}
