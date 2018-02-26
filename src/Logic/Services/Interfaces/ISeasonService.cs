using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   public interface ISeasonService
   {
      bool DetermineSeasonEnded(string seasonId);
      void EndSeasonAndCreateNext(string seasonId);
      Season Get(string seasonId);
      Season GetCurrentSeason();
   }
}
