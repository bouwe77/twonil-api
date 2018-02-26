using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomization
{
   /// <summary>
   /// Wrapper class for the <see cref="ThreadSafeRandom"/> class.
   /// </summary>
   public class Randomizer : IRandomizer
   {
      /// <summary>
      /// Gets a random number between the given numbers where both minValue and maxValue are included in the possible outcome.
      /// </summary>
      public int GetRandomNumber(int minValue, int maxValue)
      {
         return GetRandomNumber(minValue, maxValue, true);
      }

      /// <summary>
      /// Gets a random number between the given numbers where both minValue and maxValue are included in the possible outcome.
      /// </summary>
      private int GetRandomNumber(int minValue, int maxValue, bool includeMaxValue)
      {
         if (includeMaxValue)
         {
            // Add one to the maxValue because the .NET Framework's Random works with an exclusive maxValue.
            maxValue++;
         }

         return ThreadSafeRandom.ThisThreadsRandom.Next(minValue, maxValue);
      }

      /// <summary>
      /// Gets a random boolean.
      /// </summary>
      public bool GetRandomBoolean()
      {
         return ThreadSafeRandom.ThisThreadsRandom.NextDouble() > 0.5;
      }

      /// <summary>
      /// Extension method to get a random element from an <see cref="IEnumerable"/>.
      /// </summary>
      public T RandomElementByWeight<T>(IEnumerable<T> sequence, Func<T, float> weightSelector)
      {
         /*
          * Uitleg/voorbeeld:
          * Maak een IDictionary met als key het type dat random bepaald moet worden.
          * Value van de dictionary is van type float en bepaald het gewicht.
          * Vervolgens roep je deze methode aan op de dictionary, waarbij je de Value als FUNC meegeeft.
          * 
          *    var stuff = new Dictionary<int, float>();
          *    stuff.Add(1, 5.8f); // 5.8 keer het getal 1
          *    stuff.Add(2, 1.2f); // 1.2 keer het getal 2
          *    stuff.Add(3, 10); // 10 keer het getal 3
          *    int pickedRandom = RandomElementByWeight(stuff, x => x.Value);
          *    
          */

         float totalWeight = sequence.Sum(weightSelector);

         // The weight we are after...
         var itemWeightIndex = (float)(ThreadSafeRandom.ThisThreadsRandom.NextDouble() * totalWeight);

         float currentWeightIndex = 0;

         foreach (var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) })
         {
            currentWeightIndex += item.Weight;

            // If we've hit or passed the weight we are after for this item then it's the one we want....
            if (currentWeightIndex >= itemWeightIndex)
               return item.Value;
         }

         return default(T);
      }
   }
}