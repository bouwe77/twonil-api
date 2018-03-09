﻿using System.Linq;
using Shally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class SeasonTeamStatisticsMapper : IResourceMapper<SeasonTeamStatistics>
   {
      private readonly UriHelper _uriHelper;
      public static string SeasonName = "season-name";
      public static string CurrentLeagueTablePosition = "league-position";
      public static string LeagueName = "league-name";
      public static string LeagueTablePositions = "league-positions-history";
      public static string MatchResults = "league-results-history";

      public SeasonTeamStatisticsMapper(UriHelper uriHelper)
      {
         _uriHelper = uriHelper;
      }

      public Resource Map(SeasonTeamStatistics seasonTeamStatistics, params string[] properties)
      {
         var resource = new Resource(new Link(_uriHelper.GetSeasonTeamStatsUri(seasonTeamStatistics.GameId, seasonTeamStatistics.SeasonId, seasonTeamStatistics.TeamId)));

         if (properties.Contains(SeasonName))
         {
            resource.AddProperty(SeasonName, seasonTeamStatistics.Season.Name);
         }

         if (properties.Contains(CurrentLeagueTablePosition))
         {
            resource.AddProperty(CurrentLeagueTablePosition, seasonTeamStatistics.CurrentLeagueTablePosition);
         }

         if (properties.Contains(LeagueTablePositions))
         {
            resource.AddProperty(LeagueTablePositions, seasonTeamStatistics.LeagueTablePositions);
         }

         if (properties.Contains(LeagueName))
         {
            resource.AddProperty(LeagueName, seasonTeamStatistics.LeagueName);
         }

         if (properties.Contains(MatchResults))
         {
            resource.AddProperty(MatchResults, seasonTeamStatistics.MatchResults);
         }

         return resource;
      }
   }
}