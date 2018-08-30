using System.Collections.Generic;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
   public class LineService : ServiceBase
   {
      public IEnumerable<Line> GetAll()
      {
         using (var lineRepository = new RepositoryFactory().CreateLineRepository())
         {
            var lines = lineRepository.GetAll();
            return lines;
         }
      }
   }
}
