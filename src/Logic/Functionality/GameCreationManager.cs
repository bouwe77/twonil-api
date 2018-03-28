using System;
using System.Linq;
using TwoNil.Data;
using TwoNil.Data.Memory;
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

         var repositoryFactory = new DatabaseRepositoryFactory(game.Id);
         var gameDatabaseManager = repositoryFactory.CreateGameDatabaseManager();
         try
         {
            // Create the Game database.
            gameDatabaseManager.Create();

            // Generate and insert all game data.
            CreateGameData(repositoryFactory, game.Id);

            // ===================================================================
            //TODO, OK let MODDERVOKKIN op
            game.UserId = "17eqhq";
            // ===================================================================

            // Insert the Game record in the Master database.
            InsertGame(game);
         }
         catch (Exception exception)
         {
            gameDatabaseManager.Delete();
            throw;
         }

         return game;
      }

      private static void InsertGame(Game game)
      {
         using (var repository = new MasterRepositoryFactory().CreateTransactionManager())
         {
            repository.RegisterInsert(game);
            repository.Save();
         }
      }

      private void CreateGameData(DatabaseRepositoryFactory repositoryFactory, string gameId)
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
            gameInfo.CurrentTeam = teamsAndPlayers.Teams.First();
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

      private TeamsAndPlayers CreateTeamsAndPlayers(DatabaseRepositoryFactory repositoryFactory)
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

         var averageRatingsPerLeague = new[] { 5, 9, 13, 17 };
         int averageRatingIndex = 0;
         foreach (var team in teams)
         {
            int teamIndex = teams.IndexOf(team);
            if (teamIndex > 0 && teamIndex % Constants.HowManyLeagues == 0)
            {
               averageRatingIndex++;
            }

            int currentAverageRating = averageRatingsPerLeague[averageRatingIndex];
            var players = playerManager.GenerateSquad(team, currentAverageRating);
            teamsAndPlayers.Players.AddRange(players);

            teamManager.UpdateRating(team, players);
         }

         return teamsAndPlayers;
      }
   }
}
