using System.Collections.Generic;
using System.Linq;
using TwoNil.Logic.Functionality.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions
{
   internal class RoundRobinTournamentManager
   {
      /// <summary> 
      /// Round-Robin magic:
      /// The first team of teams1 stays where he is, but all
      /// other teams move one position clockwise across the two lists.
      /// All items of teams1 (except the first) move 1 position to the RIGHT, so 
      /// the last item of teams1 becomes the last item of teams2.
      /// All items of teams2 move 1 position to the LEFT, so the first item of teams2
      /// becomes the second item of teams1.
      ///
      /// EXAMPLE :)
      ///
      /// Round 1. (1 plays 14, 2 plays 13, ... )
      /// 1  2  3  4  5  6  7
      /// 14 13 12 11 10 9  8   
      /// The first item stays and rotate the others clockwise one position
      /// Round 2. (1 plays 13, 14 plays 12, ... )
      /// 1  14 2  3  4  5  6
      /// 13 12 11 10 9  8  7
      /// 
      /// http://en.wikipedia.org/wiki/Round-robin_tournament#Scheduling_algorithm 
      /// </summary>
      /// <param name="teams"></param>
      /// <returns></returns>
      public Dictionary<int, List<Match>> GetSchedule(List<Team> teams)
      {
         var matchSchedule = new Dictionary<int, List<Match>>();

         Team dummyTeam = null;

         // If odd number of teams, add a dummy, meaning in every round one team does not play a match
         int numberOfTeams = teams.Count;
         if (numberOfTeams % 2 != 0)
         {
            dummyTeam = GetDummyTeam();
            teams.Add(dummyTeam);
            numberOfTeams++;
         }

         int numberOfRounds = (numberOfTeams - 1);
         int numberOfTeamsHalf = numberOfTeams / 2;

         // Divide the list of teams in two lists and reverse the second list.
         var teams1 = teams.GetRange(0, numberOfTeams / 2);
         var teams2 = teams.GetRange(numberOfTeams / 2, numberOfTeams / 2);
         teams2.Reverse();

         // Keep track of the number of home matches of each team
         // so an as evenly as possible home-away schedule can be created.
         Dictionary<Team, int> homeMatchCounter = teams.ToDictionary(k => k, v => 0);

         // Create a match schedule for every round.
         for (int roundNr = 0; roundNr < numberOfRounds; roundNr++)
         {
            var matches = new List<Match>();
            Match dummyMatch = null;

            // Combine the teams in both lists into matches.
            for (int teamIndex = 0; teamIndex < numberOfTeamsHalf; teamIndex++)
            {
               // Default the first team plays at home.
               var homeTeam = teams1[teamIndex];
               var awayTeam = teams2[teamIndex];

               // The team that has had the least number of home matches will play at home, so swap if necessary.
               if (roundNr != 0 && homeMatchCounter[homeTeam] > homeMatchCounter[awayTeam])
               {
                  var tempTeam = homeTeam;
                  homeTeam = awayTeam;
                  awayTeam = tempTeam;
               }

               // Create match.
               var match = MatchFactory.CreateMatch(homeTeam, awayTeam);
               matches.Add(match);

               // Remember the dummy match (if applicable) for future reference.
               if (match.HomeTeam.Equals(dummyTeam) || match.AwayTeam.Equals(dummyTeam))
               {
                  dummyMatch = match;
               }

               // Raise the home count for the home team.
               homeMatchCounter[match.HomeTeam]++;
            }

            // Remove the dummy match if applicable.
            if (dummyMatch != null)
            {
               matches.Remove(dummyMatch);
            }

            matchSchedule.Add(roundNr, matches);

            // Before continuing to the next round, do the Round-Robin magic as described above...
            // Although this implementation works, it sucks big time... :)

            // Insert the first item of teams2 on the second position of teams1
            teams1.Insert(1, teams2[0]);
            // Remove the first item of teams2

            teams2.RemoveAt(0);

            // Add the last item of teams1 at the end of teams2 and remove it from teams1
            var lastTeam1 = teams1[teams1.Count - 1];
            teams2.Add(lastTeam1);
            teams1.Remove(lastTeam1);
         }

         // Now the schedule is complete for the first half of the season, add the return matches.
         var returnMatchSchedule = new Dictionary<int, List<Match>>();

         foreach (var round in matchSchedule)
         {
            // Create matches where the home team is the away team and vice versa.
            var matches = round.Value.Select(match => MatchFactory.CreateMatch(homeTeam: match.AwayTeam, awayTeam: match.HomeTeam)).ToList();

            int returnRoundNr = round.Key + numberOfRounds;
            returnMatchSchedule.Add(returnRoundNr, matches);
         }

         // Merge the two dictionaries into one match schedule and return it.
         returnMatchSchedule.ToList().ForEach(x => matchSchedule.Add(x.Key, x.Value));
         return matchSchedule;
      }

      private Team GetDummyTeam()
      {
         return new Team { Id = "dUmMy", Name = "dUmm47e@m!@#$" };
      }
   }
}
