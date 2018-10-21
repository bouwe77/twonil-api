using System;
using System.Collections.Generic;
using Randomization;
using TwoNil.Logic.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Competitions
{
   public class SingleRoundTournamentManager
   {
      private readonly IRandomizer _randomizer;

      public SingleRoundTournamentManager(IRandomizer randomizer)
      {
         _randomizer = randomizer;
      }

      /// <summary>
      /// Returns a list of matches where each team from list1 plays against a team from teams2.
      /// All teams from both lists play one match. Whether a team plays home or away is determined randomly.
      /// The total number of teams for both lists must be an even number and both lists must contain an equal number of teams.
      /// </summary>
      public List<Match> GetMatches(IList<Team> teams1, IList<Team> teams2)
      {
         // Total number of teams must be even and both lists same number of teams.
         bool isValid = (teams1.Count + teams2.Count) % 2 == 0 
                        && teams1.Count == teams2.Count;
         if (!isValid)
         {
            throw new Exception("Even number of teams expected and both lists same number of teams");
         }

         teams1.Shuffle();
         teams2.Shuffle();

         var matches = new List<Match>();
         for (int i = 0; i < teams1.Count; i++)
         {
            // Optionally swith home/away
            var homeTeam = teams1[i];
            var awayTeam = teams2[i];
            bool switchHomeAndAway = _randomizer.GetRandomBoolean();
            if (switchHomeAndAway)
            {
               homeTeam = teams2[i];
               awayTeam = teams1[i];
            }

            var match = MatchFactory.CreateMatch(homeTeam, awayTeam);

            matches.Add(match);
         }

         return matches;
      }

      /// <summary>
      /// Returns a list of matches where all teams play one match.
      /// Whether a team plays home or away is determined randomly.
      /// The given team list must contain an even number of teams.
      /// </summary>
      public List<Match> GetMatches(IList<Team> teams)
      {
         // Even number of teams expected.
         bool isValid = teams.Count % 2 == 0;
         if (!isValid)
         {
            throw new Exception("Even number of teams expected");
         }

         teams.Shuffle();

         var matches = new List<Match>();
         for (int i = 0; i < teams.Count; i += 2)
         {
            var match = MatchFactory.CreateMatch(teams[i], teams[i + 1]);
            matches.Add(match);
         }

         return matches;
      }
   }
}
