using System.Collections.Generic;
using System.Linq;
using Shally.Hal;
using TwoNil.Logic.Services;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class TeamListResourceFactory
   {
      private readonly GameInfo _gameInfo;
      private readonly TeamMapper _teamMapper;
      private readonly string _contextUriPlaceholder;

      public TeamListResourceFactory(GameInfo gameInfo, UriHelper uriHelper, string contextUriPlaceholder)
      {
         _gameInfo = gameInfo;
         _teamMapper = new TeamMapper(uriHelper);
         _contextUriPlaceholder = contextUriPlaceholder;
      }

      public IEnumerable<Resource> Create()
      {
         var teamService = new ServiceFactory().CreateTeamService(_gameInfo);
         var teams = teamService.GetGroupedByLeague().ToList();

         var teamResources = _teamMapper.Map(teams, TeamMapper.TeamName, TeamMapper.LeagueName).ToList();

         for (int i = 0; i < teamResources.Count; i++)
         {
            string contextUri = _contextUriPlaceholder.Replace("###teamid###", teams[i].Id);
            teamResources[i].AddLink("context", new Link(contextUri));
         }

         return teamResources;
      }
   }
}
