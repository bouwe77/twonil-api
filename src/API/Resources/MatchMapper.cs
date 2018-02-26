using System.Linq;
using Sally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class MatchMapper : IResourceMapper<Match>
   {
      public static string HomeScore = "home-score";
      public static string AwayScore = "away-score";
      public static string HomePenaltyScore = "home-penalty-score";
      public static string AwayPenaltyScore = "away-penalty-score";
      public static string Round = "round";
      public static string CompetitionName = "competition-name";
      public static string CompetitionType = "competition-type";
      public static string Date = "date";
      public static string Played = "played";
      public static string PenaltiesTaken = "penalties-taken";

      public Resource Map(Match match, params string[] properties)
      {
         var resource = new Resource(new Link(UriFactory.GetMatchUri(match.GameId, match.Id)));

         if (properties.Contains(HomeScore))
         {
            resource.AddProperty(HomeScore, match.HomeScore);
         }

         if (properties.Contains(AwayScore))
         {
            resource.AddProperty(AwayScore, match.AwayScore);
         }

         if (properties.Contains(CompetitionName))
         {
            resource.AddProperty(CompetitionName, match.Round.CompetitionName);
         }

         if (properties.Contains(CompetitionType))
         {
            resource.AddProperty(CompetitionType, match.Round.CompetitionType.ToString());
         }

         if (properties.Contains(Round))
         {
            resource.AddProperty(Round, match.Round.Name);
         }

         if (properties.Contains(Date))
         {
            resource.AddProperty(Date, match.Date.ToString("dd-MMM"));
         }

         if (properties.Contains(Played))
         {
            resource.AddProperty(Played, match.Played);
         }

         if (properties.Contains(PenaltiesTaken))
         {
            resource.AddProperty(PenaltiesTaken, match.PenaltiesTaken);
         }

         if (properties.Contains(HomePenaltyScore))
         {
            resource.AddProperty(HomePenaltyScore, match.HomePenaltyScore);
         }

         if (properties.Contains(AwayPenaltyScore))
         {
            resource.AddProperty(AwayPenaltyScore, match.AwayPenaltyScore);
         }

         return resource;
      }
   }
}