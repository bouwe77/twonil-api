using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   public interface IPositionService
   {
      IEnumerable<Position> GetAll();
      IEnumerable<Position> GetByLine(Line line);
   }
}
