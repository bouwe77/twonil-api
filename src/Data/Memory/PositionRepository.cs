using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Memory
{
   public interface IPositionRepository : IRepository<Position>
   {
      Position GetGoalkeeper();
      Position GetSweeper();
      Position GetCentreBack();
      Position GetWingBack();
      Position GetDefensiveMidfield();
      Position GetCentralMidfield();
      Position GetWideMidfield();
      Position GetForwardMidfield();
      Position GetStriker();
      Position GetCentreForward();
      Position GetWinger();
   }

   public class PositionRepository : MemoryRepository<Position>, IPositionRepository
   {
      public PositionRepository()
      {
         Entities = new List<Position>
         {
            GetGoalkeeper(),
            GetSweeper(),
            GetCentreBack(),
            GetWingBack(),
            GetDefensiveMidfield(),
            GetCentralMidfield(),
            GetWideMidfield(),
            GetForwardMidfield(),
            GetStriker(),
            GetCentreForward(),
            GetWinger()
         };
      }

      public Position GetGoalkeeper()
      {
         return InMemoryData.GetGoalkeeper();
      }

      public Position GetSweeper()
      {
         return InMemoryData.GetSweeper();
      }

      public Position GetCentreBack()
      {
         return InMemoryData.GetCentreBack();
      }

      public Position GetWingBack()
      {
         return InMemoryData.GetWingBack();
      }

      public Position GetDefensiveMidfield()
      {
         return InMemoryData.GetDefensiveMidfield();
      }

      public Position GetCentralMidfield()
      {
         return InMemoryData.GetCentralMidfield();
      }

      public Position GetWideMidfield()
      {
         return InMemoryData.GetWideMidfield();
      }

      public Position GetForwardMidfield()
      {
         return InMemoryData.GetForwardMidfield();
      }

      public Position GetCentreForward()
      {
         return InMemoryData.GetCentreForward();
      }

      public Position GetStriker()
      {
         return InMemoryData.GetStriker();
      }

      public Position GetWinger()
      {
         return InMemoryData.GetWinger();
      }
   }
}