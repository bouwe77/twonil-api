using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
    public interface IFormationRepository : IGenericRepository<Formation>
    {
        Formation Get343();
        Formation Get433();
        Formation Get442();
        Formation Get451();
        Formation Get541();
    }

    public class FormationRepository : MemoryRepository<Formation>, IFormationRepository
    {
      public FormationRepository()
      {
         Entities = new List<Formation>
         {
            Get442(),
            Get433(),
            Get451(),
            Get541(),
            Get343(),
         };
      }

      public Formation Get541()
      {
         return InMemoryData.Get541();
      }

      public Formation Get451()
      {
         return InMemoryData.Get451();
      }

      public Formation Get442()
      {
         return InMemoryData.Get442();
      }

      public Formation Get433()
      {
         return InMemoryData.Get433();
      }

      public Formation Get343()
      {
         return InMemoryData.Get343();
      }
   }
}
