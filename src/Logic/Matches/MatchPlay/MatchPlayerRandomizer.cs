using System.Collections.Generic;
using Randomization;

namespace TwoNil.Logic.Matches.MatchPlay
{
   public class MatchPlayerRandomizer
   {
      private IRandomizer _randomizer;

      public MatchPlayerRandomizer()
      {
         _randomizer = new Randomizer();
      }

      public int HomeScore(Dictionary<int, float> stuff)
      {
         return stuff.RandomElementByWeight(x => x.Value).Key;
      }

      public int AwayScore(Dictionary<int, float> stuff)
      {
         return stuff.RandomElementByWeight(x => x.Value).Key;
      }

      public bool HomeTeamWins(Dictionary<bool, float> stuff)
      {
         return stuff.RandomElementByWeight(x => x.Value).Key;
      }

      public int HomePenaltyScore(Dictionary<int, float> stuff)
      {
         return stuff.RandomElementByWeight(x => x.Value).Key;
      }

      public int AwayPenaltyScore(Dictionary<int, float> stuff)
      {
         return stuff.RandomElementByWeight(x => x.Value).Key;
      }

      public bool HomeTeamWinsPenalties()
      {
         return _randomizer.GetRandomBoolean();
      }
   }
}
