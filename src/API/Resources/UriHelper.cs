using Dolores.Routing;

namespace TwoNil.API.Resources
{
   public class UriHelper
   {
      private readonly IRouteHelper _routeHelper;

      public UriHelper(IRouteHelper routeHelper)
      {
         _routeHelper = routeHelper;
      }

      //TODO Dolores RouteHelper hier gebruiken!!!

      private string GetUri(string uri, params object[] parameters)
      {
         string formattedUri = string.Format(uri, parameters);
         return formattedUri;
      }

      public string GetSubstitutionUri(string teamId, string gameId)
      {
         string uri = GetUri("/games/{0}/teams/{1}/substitution", gameId, teamId);
         return uri;
      }

      public string GetUserTeamUri(string gameId, string userId)
      {
         string uri = GetUri("/games/{0}/users/{1}/team", gameId, userId);
         return uri;
      }

      public string GetTeamUri(string gameId, string teamId)
      {
         string uri = GetUri("/games/{0}/teams/{1}", gameId, teamId);
         return uri;
      }

      public string GetUserUri(string userId)
      {
         string uri = GetUri("/users/{0}", userId);
         return uri;
      }

      public string GetPlayerUri(string gameId, string playerId)
      {
         string uri = GetUri("/games/{0}/players/{1}", gameId, playerId);
         return uri;
      }

      //public string GetFormationUri(string formationId)
      //{
      //   string uri = GetUri("/formations/{0}", formationId);
      //   return uri;
      //}

      public string GetTeamPlayersUri(string gameId, string teamId)
      {
         string uri = GetUri("/games/{0}/teams/{1}/players", gameId, teamId);
         return uri;
      }

      public string GetGameTeamsUri(string gameId)
      {
         string uri = GetUri("/games/{0}/teams", gameId);
         return uri;
      }

      public string GetGameUri(string gameId)
      {
         string uri = GetUri("/games/{0}", gameId);
         return uri;
      }

      public string GetGameLinksUri(string gameId)
      {
         string uri = GetUri("/games/{0}/links", gameId);
         return uri;
      }

      public string GetGamesUri()
      {
         string uri = GetUri("/games");
         return uri;
      }

      public string GetHomeUri()
      {
         return GetUri("/");
      }

      public string GetSeasonTeamMatchesUri(string gameId, string seasonId, string teamId)
      {
         string uri = GetUri("/games/{0}/seasons/{1}/teams/{2}/matches", gameId, seasonId, teamId);
         return uri;
      }

      public string GetMatchDayUri(string gameId, string dayId)
      {
         string uri = GetUri("/games/{0}/days/{1}/matches", gameId, dayId);
         return uri;
      }

      public string GetCompetitionLeagueTableUri(string gameId, string seasonId, string competitionId)
      {
         string uri = GetUri("/games/{0}/seasons/{1}/competitions/{2}/leaguetable", gameId, seasonId, competitionId);
         return uri;
      }

      public string GetSeasonLeagueTablesUri(string gameId, string seasonId)
      {
         string uri = GetUri("/games/{0}/seasons/{1}/leaguetables", gameId, seasonId);
         return uri;
      }

      public string GetMatchUri(string gameId, string matchId)
      {
         string uri = GetUri("/games/{0}/matches/{1}", gameId, matchId);
         return uri;
      }

      public string GetSeasonUri(string gameId, string seasonId)
      {
         string uri = GetUri("/games/{0}/seasons/{1}", gameId, seasonId);
         return uri;
      }

      public string GetSeasonStatsUri(string gameId, string seasonId)
      {
         string uri = GetUri("/games/{0}/seasons/{1}/stats", gameId, seasonId);
         return uri;
      }

      public string GetSeasonTeamStatsUri(string gameId, string seasonId, string teamId)
      {
         string uri = GetUri("/games/{0}/seasons/{1}/teams/{2}/stats", gameId, seasonId, teamId);
         return uri;
      }

      /// <summary>
      /// Returns a templated rels URI, to be used with curies.
      /// </summary>
      /// <returns>The rels URI.</returns>
      public string GetRelationsUri()
      {
         string uri = GetUri("/rels/{{rel}}");
         return uri;
      }

      /// <summary>
      /// Returns a rels URI with the given relations name, typically to be used with Forms.
      /// </summary>
      /// <returns>The rels URI.</returns>
      public string GetRelationsUri(string relationName)
      {
         string uri = GetUri($"/rels/{relationName}");
         return uri;
      }
   }
}