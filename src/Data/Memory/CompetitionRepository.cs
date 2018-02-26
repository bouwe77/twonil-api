using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Memory
{
   public class CompetitionRepository : MemoryRepository<Competition>
   {
      internal CompetitionRepository()
      {
         Entities = new List<Competition>
         {
            GetLeague1(),
            GetLeague2(),
            GetLeague3(),
            GetLeague4(),
            GetNationalCup(),
            GetFriendly()
         };
      }

      public Competition GetLeague1()
      {
         return InMemoryData.GetLeague1();
      }

      public Competition GetLeague2()
      {
         return InMemoryData.GetLeague2();
      }

      public Competition GetLeague3()
      {
         return InMemoryData.GetLeague3();
      }

      public Competition GetLeague4()
      {
         return InMemoryData.GetLeague4();
      }

      public Competition GetNationalCup()
      {
         return InMemoryData.GetNationalCup();
      }

      public Competition GetFriendly()
      {
         return InMemoryData.GetFriendly();
      }

      public Competition GetNationalSuperCup()
      {
         return InMemoryData.GetNationalSuperCup();
      }

      public IEnumerable<Competition> GetByCompetitionType(CompetitionType competitionType)
      {
         return Entities.Where(x => x.CompetitionType == competitionType).OrderBy(x => x.Order);
      }
   }
}
