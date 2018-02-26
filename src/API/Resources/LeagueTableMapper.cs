using System.Collections.Generic;
using Sally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public class LeagueTableMapper : IResourceMapper<LeagueTable>
   {
      public Resource Map(LeagueTable leagueTable, params string[] properties)
      {
         var teamMapper = new TeamMapper();

         var leagueTableLink = new Link(UriFactory.GetCompetitionLeagueTableUri(leagueTable.GameId, leagueTable.SeasonCompetition.SeasonId, leagueTable.SeasonCompetition.CompetitionId));
         var resource = new Resource(leagueTableLink);

         resource.AddProperty("competition-name", leagueTable.CompetitionName);

         var positions = new List<Resource>();
         foreach (var leagueTablePosition in leagueTable.LeagueTablePositions)
         {
            var position = new Resource(leagueTableLink);

            position.AddResource("team", teamMapper.Map(leagueTablePosition.Team, TeamMapper.TeamName));
            position.AddProperty("position", leagueTablePosition.Position);
            position.AddProperty("played", leagueTablePosition.Matches);
            position.AddProperty("points", leagueTablePosition.Points);
            position.AddProperty("wins", leagueTablePosition.Wins);
            position.AddProperty("draws", leagueTablePosition.Draws);
            position.AddProperty("losses", leagueTablePosition.Losses);
            position.AddProperty("goals-scored", leagueTablePosition.GoalsScored);
            position.AddProperty("goals-conceded", leagueTablePosition.GoalsConceded);

            string goalDifference = leagueTablePosition.GoalDifference.ToString();
            if (leagueTablePosition.GoalDifference > 0)
            {
               goalDifference = $"+{goalDifference}";
            }
            position.AddProperty("goal-difference", goalDifference);

            positions.Add(position);
         }

         resource.AddResource("positions", positions.ToArray());

         return resource;
      }
   }
}