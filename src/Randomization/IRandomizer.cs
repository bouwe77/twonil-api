using System;
using System.Collections.Generic;

namespace Randomization
{
   public interface IRandomizer
   {
      int GetRandomNumber(int minValue, int maxValue);
      bool GetRandomBoolean();
      T RandomElementByWeight<T>(IEnumerable<T> sequence, Func<T, float> weightSelector);
   }
}
