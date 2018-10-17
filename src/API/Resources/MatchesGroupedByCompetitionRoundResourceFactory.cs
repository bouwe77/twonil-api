using System;
using System.Collections.Generic;
using System.Linq;
using Shally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   /// <summary>
   /// This is a factory class instead of a IResourceMapper implementation because this resource does not correspond with a DomainObject.
   /// It groups matches by competition.
   /// </summary>
   public class MatchesGroupedByCompetitionResourceFactory
   {
      private readonly UriHelper _uriHelper;

      public MatchesGroupedByCompetitionResourceFactory(UriHelper uriHelper)
      {
         _uriHelper = uriHelper;
      }

      public IEnumerable<Resource> Create(IEnumerable<Match> matches, string gameId, string teamIdOnTop, out int numberOfMatches)
      {
         numberOfMatches = 0;

         var matchMapper = new MatchMapper(_uriHelper);
         var teamMapper = new TeamMapper(_uriHelper);

         var matchesGroupedByCompetitionId = matches.ToLookup(m => m.CompetitionId);
         var competitionResources = new List<Resource>();
         foreach (var group in matchesGroupedByCompetitionId)
         {
            bool teamIdFound = false;

            var matchesForOneCompetitionResource = new Resource(new Link(_uriHelper.GetHomeUri()));

            var matchResources = new List<Resource>();

            bool first = true;
            foreach (var match in group)
            {
               if (first)
               {
                  matchesForOneCompetitionResource.AddProperty("competition-name", match.Round.CompetitionName);
                  matchesForOneCompetitionResource.AddProperty("round-name", match.Round.Name);
               }

               if (match.HomeTeamId == teamIdOnTop
                   || match.AwayTeamId == teamIdOnTop)
               {
                  teamIdFound = true;
               }

               var matchResource = matchMapper.Map(match, MatchMapper.HomeScore, MatchMapper.AwayScore, MatchMapper.Played, MatchMapper.HomePenaltyScore, MatchMapper.AwayPenaltyScore, MatchMapper.PenaltiesTaken);
               matchResource.AddResource("home-team", teamMapper.Map(match.HomeTeam, TeamMapper.TeamName));
               matchResource.AddResource("away-team", teamMapper.Map(match.AwayTeam, TeamMapper.TeamName));
               matchResources.Add(matchResource);

               numberOfMatches++;

               first = false;
            }

            matchesForOneCompetitionResource.AddResource("rel:matches", matchResources);

            if (teamIdFound)
            {
               competitionResources.Insert(0, matchesForOneCompetitionResource);
            }
            else
            {
               competitionResources.Add(matchesForOneCompetitionResource);
            }
         }

         return competitionResources;
      }
   }
}