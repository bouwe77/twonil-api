using System;
using System.Linq;
using System.Threading.Tasks;
using Randomization;
using TwoNil.Data;
using TwoNil.Data.Repositories;
using TwoNil.Logic.Competitions;
using TwoNil.Logic.Players;
using TwoNil.Logic.Teams;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic
{
    public class GameCreationManager
    {
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly INumberRandomizer _numberRandomizer;
        private readonly SeasonManager _seasonManager;
        private readonly TeamManager _teamManager;
        private readonly PlayerManager _playerManager;
        private readonly StartingLineupGenerator _startingLineupGenerator;

        public GameCreationManager(IUnitOfWorkFactory uowFactory, INumberRandomizer numberRandomizer, SeasonManager seasonManager, 
            TeamManager teamManager, PlayerManager playerManager, StartingLineupGenerator startingLineupGenerator)
        {
            _uowFactory = uowFactory;
            _numberRandomizer = numberRandomizer;
            _seasonManager = seasonManager;
            _teamManager = teamManager;
            _playerManager = playerManager;
            _startingLineupGenerator = startingLineupGenerator;
        }

        public async Task<Game> CreateGame()
        {
            var game = new Game();

            using (var uow = _uowFactory.Create())
            {
                // Create the Game, generate game data and save it to the database.
                await CreateGameData(game.Id);

                // ===================================================================
                //TODO, OK let MODDERVOKKIN op
                game.UserId = "17eqhq";
                // ===================================================================

                // Insert the game in the database.
                uow.Games.Add(game);
            }

            return game;
        }

        private async Task CreateGameData(string gameId)
        {
            using (var uow = _uowFactory.Create())
            {
                // Create GameInfo.
                // Overrule the GameInfo Id with the Game Id.
                var gameInfo = new GameInfo
                {
                    Name = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    Id = gameId
                };

                // Create teams and players.
                var teamsAndPlayers = CreateTeamsAndPlayers();

                // ===================================================================
                //TODO OK, let MODDERVOKKIN op
                int randomIndex = _numberRandomizer.GetNumber(0, Constants.HowManyTeamsPerLeague * Constants.HowManyLeagues - 1);
                gameInfo.CurrentTeam = teamsAndPlayers.Teams[randomIndex];
                // ===================================================================

                uow.GameInfos.Add(gameInfo);

                // Insert teams.
                uow.Teams.Add(teamsAndPlayers.Teams);

                // Insert players.
                uow.Players.Add(teamsAndPlayers.Players);

                // Create a season with match schedules.
                _seasonManager.CreateFirstSeason(teamsAndPlayers.Teams, uow);

                await uow.CommitAsync();
            }
        }

        private TeamsAndPlayers CreateTeamsAndPlayers()
        {
            var teamsAndPlayers = new TeamsAndPlayers();

            // Generate all teams for this game.
            const int howManyTeams = Constants.HowManyTeamsPerLeague * Constants.HowManyLeagues;
            var teams = _teamManager.Create(howManyTeams).ToList();

            // Assign team names.
            using (var uow = _uowFactory.Create())
            {
                var teamNames = new TeamNameRepository().GetAll();
                for (int i = 0; i < teams.Count; i++)
                {
                    teams[i].Name = teamNames[i];
                }
            }

            teamsAndPlayers.Teams = teams;

            var averageRatingsPerLeague = new[] { 10, 40, 70, 100 };
            int averageRatingIndex = 0;

            foreach (var team in teams)
            {
                int teamIndex = teams.IndexOf(team);
                if (teamIndex > 0 && teamIndex % Constants.HowManyLeagues == 0)
                {
                    averageRatingIndex++;
                }

                int currentAverageRating = averageRatingsPerLeague[averageRatingIndex];
                var players = _playerManager.GenerateSquad(team, currentAverageRating).ToList();

                var players2 = _startingLineupGenerator.GenerateStartingLineup(players, team.Formation);

                teamsAndPlayers.Players.AddRange(players2);

                _teamManager.UpdateRating(team, players.Where(p => p.InStartingEleven).ToList());
            }

            return teamsAndPlayers;
        }
    }
}
