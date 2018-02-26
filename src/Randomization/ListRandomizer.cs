using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomization
{
   public interface IListRandomizer
   {
      T GetItem<T>(IEnumerable<T> list);
   }

   public class ListRandomizer : IListRandomizer
   {
      private static Randomizer _randomizer = new Randomizer();

      public T GetItem<T>(IEnumerable<T> list)
      {
         if (!list.Any())
         {
            throw new Exception("No items in list");
         }

         int maxIndex = list.Count() - 1;
         int randomIndex = _randomizer.GetRandomNumber(0, maxIndex);
         var randomListItem = list.ToList()[randomIndex];

         return randomListItem;
      }
   }
}
