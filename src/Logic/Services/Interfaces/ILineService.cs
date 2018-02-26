using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   public interface ILineService
   {
      IEnumerable<Line> GetAll();
   }
}
