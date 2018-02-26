using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions
{
   internal class LeagueTableManager
   {
      public void UpdateLeagueTable(LeagueTable leagueTable, IEnumerable<Match> matches)
      {
         foreach (var match in matches)
         {
            // Determine the home and away team.
            var homeTeam = leagueTable.LeagueTablePositions.First(pos => pos.Team.Equals(match.HomeTeam));
            var awayTeam = leagueTable.LeagueTablePositions.First(pos => pos.Team.Equals(match.AwayTeam));

            // Increment number of played matches.
            homeTeam.Matches++;
            awayTeam.Matches++;

            // Calculate goals scored, conceded and differential for both teams.
            homeTeam.GoalsScored += match.HomeScore;
            homeTeam.GoalsConceded += match.AwayScore;
            homeTeam.GoalDifference = homeTeam.GoalsScored - homeTeam.GoalsConceded;
            awayTeam.GoalsScored += match.AwayScore;
            awayTeam.GoalsConceded += match.HomeScore;
            awayTeam.GoalDifference = awayTeam.GoalsScored - awayTeam.GoalsConceded;

            // Determine the number of points for both teams.
            // Increment the number of wins, losses and/or draws for both teams.
            int homePoints = 1;
            int awayPoints = 1;
            if (match.HomeScore == match.AwayScore)
            {
               homeTeam.Draws++;
               awayTeam.Draws++;
            }
            else
            {
               bool homeTeamWins = match.HomeScore > match.AwayScore;
               bool awayTeamWins = match.AwayScore > match.HomeScore;
               if (homeTeamWins)
               {
                  homePoints = 3;
                  awayPoints = 0;
                  homeTeam.Wins++;
                  awayTeam.Losses++;
               }
               else if (awayTeamWins)
               {
                  homePoints = 0;
                  awayPoints = 3;
                  homeTeam.Losses++;
                  awayTeam.Wins++;
               }
            }

            homeTeam.Points += homePoints;
            awayTeam.Points += awayPoints;
         }

         // Sort the league table.
         leagueTable.LeagueTablePositions.Sort((x, y) =>
         {
            // Points will be sorted ascending, hence Y followed by X.
            int result = y.Points.CompareTo(x.Points);
            
            // If necessary, matches will be sorted descending, hence X followed by Y.
            if (result == 0)
            {
               result = x.Matches.CompareTo(y.Matches);
            }

            // If necessary, goal difference will be sorted.
            if (result == 0)
            {
               result = y.GoalDifference.CompareTo(x.GoalDifference);
            }

            // If necessary, goals scored will be sorted.
            if (result == 0)
            {
               result = y.GoalsScored.CompareTo(x.GoalsScored);
            }

            // For now, if everything is equal, sort alphabetically to determine which team goes first.
            if (result == 0)
            {
               result = x.Team.Name.CompareTo(y.Team.Name);
            }

            return result;
         });

         // Update position numbers, starting with 1.
         int position = 1;
         foreach (var leagueTablePosition in leagueTable.LeagueTablePositions)
         {
            leagueTablePosition.Position = position;
            leagueTablePosition.Team.CurrentLeaguePosition = position;
            position++;
         }
      }
   }
}
