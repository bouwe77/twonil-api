using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Matches
{
   public class MatchPlayer
   {
      private readonly IMatchPlayerRandomizer _randomizer;
      private readonly IPenaltyTaker _penaltyTaker;

      public MatchPlayer()
         : this(new MatchPlayerRandomizer(), new PenaltyTaker())
      {
      }

      internal MatchPlayer(IMatchPlayerRandomizer randomizer, IPenaltyTaker penaltyTaker)
      {
         _randomizer = randomizer;
         _penaltyTaker = penaltyTaker;
      }

      public void Play(Match match)
      {
         // Determine randomly, though weighted, which team wins based on the Team Rating.
         var stuff = new Dictionary<bool, float>
         {
            { true, (float)match.HomeTeam.Rating },
            { false, (float)match.AwayTeam.Rating }
         };
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
            _penaltyTaker.TakePenalties(match);
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
