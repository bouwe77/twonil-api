using System.Linq;
using Dolores.Responses;
using Shally.Hal;
using TwoNil.API.Helpers;
using TwoNil.API.Resources;
using TwoNil.Services;
using TwoNil.Shared.DomainObjects;

//TODO Alle controllers controleren of ze wel alle argumenten valideren en checken of ze bij de user horen

namespace TwoNil.API.Controllers
{
    public class PlayerController : ControllerBase
    {
        public PlayerController(ServiceFactory serviceFactory, UriHelper uriHelper)
            : base(serviceFactory, uriHelper)
        {
        }

        public Response GetTeamPlayers(string gameId, string teamId)
        {
            var game = GetGameInfo(gameId);

            RequestHelper.ValidateId(teamId);

            var teamService = ServiceFactory.CreateTeamService(game);
            var team = teamService.GetTeam(teamId);
            if (team == null)
            {
                throw ResponseHelper.Get404NotFound($"Team with ID '{teamId}' not found");
            }

            // Get the team's players from the database.
            var playerService = ServiceFactory.CreatePlayerService(game);
            var players = playerService.GetByTeam(team).OrderBy(player => player.TeamOrder);

            // Map the players to player resources.
            var playerResources = players.Select(GetPlayerResource).ToList();

            var halDocument = CreateHalDocument(UriHelper.GetTeamPlayersUri(gameId, teamId), game);
            halDocument.AddResource("rel:players", playerResources);

            var teamListResourceFactory = new TeamListResourceFactory(game, UriHelper, UriHelper.GetTeamPlayersUri(gameId, "###teamid###"));
            halDocument.AddResource("rel:teams", teamListResourceFactory.Create());

            var response = GetResponse(halDocument);
            return response;
        }

        private Resource GetPlayerResource(Player player)
        {
            var playerResource = new PlayerMapper(UriHelper).Map(
               player,
               PlayerMapper.Name,
               PlayerMapper.Age,
               PlayerMapper.PreferredPosition,
               PlayerMapper.CurrentPosition,
               PlayerMapper.Rating,
               PlayerMapper.Skills);

            // Team.
            if (player.Team != null)
            {
                var teamResource = new TeamMapper(UriHelper).Map(player.Team, TeamMapper.TeamName);
                playerResource.AddResource("rel:team", teamResource);
            }

            return playerResource;
        }
    }
}
