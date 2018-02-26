using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   public interface ICompetitionService
   {
      IEnumerable<Competition> GetAll();
      Competition Get(string competitionId);
   }
}
