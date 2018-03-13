﻿using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Data.Database;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions
{
   internal class LeagueTableManager
   {
      /// <summary>
      /// Updates the existing league table with the given match results.
      /// </summary>
      /// <param name="leagueTable">The league table.</param>
      /// <param name="matches">The match results to update the league table with.</param>
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
         leagueTable.LeagueTablePositions.Sort((pos1, pos2) =>
         {
            int result = SortOnPointsAndGoals(pos1, pos2);

            // If both teams still have the same results they will be sorted alphabetically to determine which team goes first.
            if (result == 0)
            {
               result = string.Compare(pos1.Team.Name, pos2.Team.Name, StringComparison.Ordinal);
            }

            return result;
         });

         UpdatePositionNumbers(leagueTable);
      }

      /// <summary>
      /// Corrects the league table positions if necessary.
      /// This is necessary when two teams have exact the same amount of points, goal difference and goals scored.
      /// The <see cref="UpdateLeagueTable"/> method will sort these teams alphabetically.
      /// What this method does is look at the match results between the two teams and corrects the position if necessary.
      /// Note this method has two limitations, which normally should not cause any problems:
      /// - It does not correct the positions if no or just one match has been played.
      /// - It does not correct the positions if more than 2 teams have exact the same points and goals.
      /// </summary>
      /// <param name="leagueTable"></param>
      /// <param name="repositoryFactory"></param>
      public void CorrectPositionsIfNecessary(LeagueTable leagueTable, IDatabaseRepositoryFactory repositoryFactory)
      {
         // Only correct if 2 or more matches have been played.
         if (leagueTable.LeagueTablePositions.All(ltp => ltp.Matches > 1))
         {
            var matchRepository = repositoryFactory.CreateMatchRepository();

            // Sort the league table.
            leagueTable.LeagueTablePositions.Sort((pos1, pos2) =>
            {
               int result = SortOnPointsAndGoals(pos1, pos2);

               // If both teams still have the same results look at the match results between the teams. 
               // If this yields no winner the sort result stays the same and the teams are not re-sorted.
               if (result == 0)
               {
                  result = CheckMatchResults(leagueTable, pos1, pos2, matchRepository);
               }

               return result;
            });

            UpdatePositionNumbers(leagueTable);
         }
      }

      private static int CheckMatchResults(LeagueTable leagueTable, LeagueTablePosition pos1, LeagueTablePosition pos2, IMatchRepository matchRepository)
      {
         int result = 0;

         var team1 = pos1.TeamId;
         var team2 = pos2.TeamId;
         var matches = matchRepository.GetMatchesBetweenTeams(leagueTable.SeasonCompetition, team1, team2).ToList();

         if (matches.Any())
         {
            int team1GoalDiff = 0;
            int team2GoalDiff = 0;
            foreach (var match in matches)
            {
               if (match.HomeTeamId == team1)
               {
                  team1GoalDiff += match.HomeScore - match.AwayScore;
                  team2GoalDiff += match.AwayScore - match.HomeScore;
               }
               else
               {
                  team1GoalDiff += match.AwayScore - match.HomeScore;
                  team2GoalDiff += match.HomeScore - match.AwayScore;
               }
            }

            result = team2GoalDiff.CompareTo(team1GoalDiff);
         }

         return result;
      }

      private static void UpdatePositionNumbers(LeagueTable leagueTable)
      {
         int position = 1;
         foreach (var leagueTablePosition in leagueTable.LeagueTablePositions)
         {
            leagueTablePosition.Position = position;
            leagueTablePosition.Team.CurrentLeaguePosition = position;
            position++;
         }
      }

      private int SortOnPointsAndGoals(LeagueTablePosition pos1, LeagueTablePosition pos2)
      {
         // Points will be sorted ascending, hence Y followed by X.
         int result = pos2.Points.CompareTo(pos1.Points);

         // If necessary, matches will be sorted descending, hence X followed by Y.
         if (result == 0)
         {
            result = pos1.Matches.CompareTo(pos2.Matches);
         }

         // If necessary, goal difference will be sorted.
         if (result == 0)
         {
            result = pos2.GoalDifference.CompareTo(pos1.GoalDifference);
         }

         // If necessary, goals scored will be sorted.
         if (result == 0)
         {
            result = pos2.GoalsScored.CompareTo(pos1.GoalsScored);
         }

         return result;
      }
   }
}
