using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Matches
{
   internal class MatchPlayer
   {
      private readonly IMatchPlayerRandomizer _randomizer;

      public MatchPlayer()
         : this(new MatchPlayerRandomizer())
      {
      }

      internal MatchPlayer(IMatchPlayerRandomizer randomizer)
      {
         _randomizer = randomizer;
      }

      public void Play(Match match)
      {
         // Determine randomly, though weighted, which team wins based on the Team Rating.
         var stuff = new Dictionary<bool, float>();
         stuff.Add(true, (float)match.HomeTeam.Rating);
         stuff.Add(false, (float)match.AwayTeam.Rating);
         bool homeTeamWins = _randomizer.HomeTeamWins(stuff);

         // Determine the score of the match.
         // Possible stuff contains 5 zeroes, 4 ones, 3 twos, 3 threes, etc.
         var possibleStuff = new Dictionary<int, float> { { 0, 5 }, { 1, 4 }, { 2, 3 }, { 3, 3 }, { 4, 2 }, { 5, 1 } };

         match.HomeScore = _randomizer.HomeScore(possibleStuff);
         match.AwayScore = _randomizer.AwayScore(possibleStuff);

         // Take penalty shoot out, if necessary.
         bool matchEndedInDraw = match.HomeScore == match.AwayScore;
         if (matchEndedInDraw && !match.DrawPermitted)
         {
            var possiblePenaltyScores = new Dictionary<int, float> { { 5, 5 }, { 4, 5 }, { 3, 4 }, { 2, 1 }, { 1, 1 } };

            match.HomePenaltyScore = _randomizer.HomePenaltyScore(possiblePenaltyScores);
            match.AwayPenaltyScore = _randomizer.AwayPenaltyScore(possiblePenaltyScores);
            match.PenaltiesTaken = true;

            // Randomly pick a winner if the penalty shootout also ended undecisive.
            bool penaltyDraw = match.HomePenaltyScore == match.AwayPenaltyScore;
            if (penaltyDraw)
            {
               bool homeTeamWinsPenalties = _randomizer.HomeTeamWinsPenalties();
               if (homeTeamWinsPenalties)
               {
                  match.HomePenaltyScore++;
               }
               else
               {
                  match.AwayPenaltyScore++;
               }
            }
         }

         // Swap home and awayscore if necessary.
         if ((homeTeamWins && match.AwayScore > match.HomeScore)
             || (!homeTeamWins && match.HomeScore > match.AwayScore))
         {
            int temp = match.HomeScore;
            match.HomeScore = match.AwayScore;
            match.AwayScore = temp;
         }

         match.MatchStatus = MatchStatus.Ended;
      }
   }
}
