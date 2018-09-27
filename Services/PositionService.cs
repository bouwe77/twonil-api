using System.Collections.Generic;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
   public class PositionService : ServiceBase
   {
      public IEnumerable<Position> GetAll()
      {
         using (var positionRepository = new RepositoryFactory().CreatePositionRepository())
         {
            var positions = positionRepository.GetAll();
            return positions;
         }
      }

      public IEnumerable<Position> GetByLine(Line line)
      {
         using (var positionRepository = new RepositoryFactory().CreatePositionRepository())
         {
            var positions = positionRepository.Find(x => x.Line.Equals(line));
            return positions;
         }
      }
   }
}
