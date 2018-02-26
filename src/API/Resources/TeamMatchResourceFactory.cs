using System.Collections.Generic;
using Sally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   /// <summary>
   /// This is a factory class instead of a IResourceMapper implementation because this resource does not correspond with a DomainObject.
   /// This class creates team match resources.
   /// </summary>
   public class TeamMatchResourceFactory
   {
      public IEnumerable<Resource> Create(IEnumerable<TeamRoundMatch> matches, string gameId, string seasonId, string teamId)
      {
         var teamMapper = new TeamMapper();

         var matchResources = new List<Resource>();
         foreach (var match in matches)
         {
            var matchResource = new Resource(new Link(UriFactory.GetHomeUri()));

            matchResource.AddProperty("date", match.MatchDate.ToString("dd-MMM"));
            matchResource.AddProperty("competition-name", match.CompetitionName);
            matchResource.AddProperty("round", match.RoundName);
            matchResource.AddProperty("home-score", match.HomeScore);
            matchResource.AddProperty("away-score", match.AwayScore);
            matchResource.AddProperty("penalties-taken", match.PenaltiesTaken);
            matchResource.AddProperty("home-penalty-score", match.HomePenaltyScore);
            matchResource.AddProperty("away-penalty-score", match.AwayPenaltyScore);
            matchResource.AddProperty("played", match.Played);

            if (match.HomeTeam != null)
            {
               var homeTeamResource = teamMapper.Map(match.HomeTeam, TeamMapper.TeamName);
               matchResource.AddResource("home-team", homeTeamResource);
            }

            if (match.AwayTeam != null)
            {
               var awayTeamResource = teamMapper.Map(match.AwayTeam, TeamMapper.TeamName);
               matchResource.AddResource("away-team", awayTeamResource);
            }

            matchResources.Add(matchResource);
         }

         return matchResources;
      }
   }
}