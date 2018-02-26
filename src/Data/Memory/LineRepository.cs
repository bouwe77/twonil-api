﻿using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Memory
{
   public class LineRepository : MemoryRepository<Line>
   {
      public LineRepository()
      {
         Entities = new List<Line>
         {
            GetGoalkeeper(),
            GetDefence(),
            GetMidfield(),
            GetAttack()
         };
      }

      public Line GetGoalkeeper()
      {
         return InMemoryData.GetGoalkeeperLine();
      }

      public Line GetDefence()
      {
         return InMemoryData.GetDefence();
      }

      public Line GetMidfield()
      {
         return InMemoryData.GetMidfield();
      }

      public Line GetAttack()
      {
         return InMemoryData.GetAttack();
      }
   }
}
