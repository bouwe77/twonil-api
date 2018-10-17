using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
    public interface ILineRepository
    {
        Line GetAttack();
        Line GetDefence();
        Line GetGoalkeeper();
        Line GetMidfield();
    }

    public class LineRepository : MemoryRepository<Line>, ILineRepository
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
