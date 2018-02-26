using System.Linq;
using Sally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class SeasonStatisticsMapper : IResourceMapper<SeasonStatistics>
   {
      public static string SeasonName = "name";
      public static string NationalChampion = "national-champion";
      public static string NationalChampionRunnerUp = "national-champion-runnerup";
      public static string NationalCupWinner = "national-cup-winner";
      public static string NationalCupRunnerUp = "national-cup-runnerup";

      public Resource Map(SeasonStatistics seasonStatistics, params string[] properties)
      {
         var teamMapper = new TeamMapper();

         var resource = new Resource(new Link(UriFactory.GetSeasonUri(seasonStatistics.GameId, seasonStatistics.Id)));

         if (properties.Contains(SeasonName))
         {
            resource.AddProperty(SeasonName, seasonStatistics.Season.Name);
         }

         if (properties.Contains(NationalChampion) && seasonStatistics.NationalChampion != null)
         {
            var team = teamMapper.Map(seasonStatistics.NationalChampion, TeamMapper.TeamName);
            resource.AddResource(NationalChampion, team);
         }

         if (properties.Contains(NationalChampionRunnerUp) && seasonStatistics.NationalChampionRunnerUp != null)
         {
            var team = teamMapper.Map(seasonStatistics.NationalChampionRunnerUp, TeamMapper.TeamName);
            resource.AddResource(NationalChampionRunnerUp, team);
         }

         if (properties.Contains(NationalCupWinner) && seasonStatistics.CupWinner != null)
         {
            var team = teamMapper.Map(seasonStatistics.CupWinner, TeamMapper.TeamName);
            resource.AddResource(NationalCupWinner, team);
         }

         if (properties.Contains(NationalCupRunnerUp) && seasonStatistics.CupRunnerUp != null)
         {
            var team = teamMapper.Map(seasonStatistics.CupRunnerUp, TeamMapper.TeamName);
            resource.AddResource(NationalCupRunnerUp, team);
         }

         return resource;
      }
   }
}