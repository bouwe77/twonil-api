using System;

namespace TwoNil.Logic.Players
{
   public class AgeRange
   {
      public int MinimumAge { get; private set; }
      public int MaximumAge { get; private set; }

      public AgeRange(int minimumAge, int maximumAge)
      {
         if (minimumAge > maximumAge) throw new Exception("Minimum age cannot be larger than maximum age.");
         if (minimumAge < 16) throw new Exception("Minimum age cannot be lower than 16");
         if (maximumAge > 40) throw new Exception("Maximum age cannot be higher than 40");

         MinimumAge = minimumAge;
         MaximumAge = maximumAge;
      }
   }
}
