﻿using System.Collections.Generic;
using System.Linq;
using Shally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class TeamMapper : IResourceMapper<Team>
   {
      private readonly UriHelper _uriHelper;
      public static string TeamName = "name";
      public static string Rating = "rating";
      public static string RatingGoalkeeper = "rating-goalkeeper";
      public static string RatingDefence = "rating-defence";
      public static string RatingMidfield = "rating-midfield";
      public static string RatingAttack = "rating-attack";
      public static string RatingPercentage = "rating-percentage";
      public static string LeagueName = "league-name";
      public static string CurrentLeaguePosition = "current-league-position";

      public TeamMapper(UriHelper uriHelper)
      {
         _uriHelper = uriHelper;
      }

      public Resource Map(Team team, params string[] properties)
      {
         var resource = new Resource(new Link(_uriHelper.GetTeamUri(team.GameId, team.Id)));

         if (properties.Contains(TeamName))
         {
            resource.AddProperty(TeamName, team.Name);
         }

         if (properties.Contains(Rating))
         {
            resource.AddProperty(Rating, team.Rating);
         }

         if (properties.Contains(RatingGoalkeeper))
         {
            resource.AddProperty(RatingGoalkeeper, team.RatingGoalkeeper);
         }

         if (properties.Contains(RatingDefence))
         {
            resource.AddProperty(RatingDefence, team.RatingDefence);
         }

         if (properties.Contains(RatingMidfield))
         {
            resource.AddProperty(RatingMidfield, team.RatingMidfield);
         }

         if (properties.Contains(RatingAttack))
         {
            resource.AddProperty(RatingAttack, team.RatingAttack);
         }

         if (properties.Contains(RatingPercentage))
         {
            var ratingPercentage = (team.Rating / 20) * 100;
            resource.AddProperty(RatingPercentage, ratingPercentage);
         }

         if (properties.Contains(LeagueName))
         {
            resource.AddProperty(LeagueName, team.CurrentLeagueCompetition.Name);
         }

         if (properties.Contains(CurrentLeaguePosition))
         {
            resource.AddProperty(CurrentLeaguePosition, team.CurrentLeaguePosition);
         }

         return resource;
      }

      public IEnumerable<Resource> Map(IEnumerable<Team> teams, params string[] properties)
      {
         var teamResources = new List<Resource>();
         foreach (var t in teams)
         {
            teamResources.Add(Map(t, properties));
         }

         return teamResources;
      }
   }
}