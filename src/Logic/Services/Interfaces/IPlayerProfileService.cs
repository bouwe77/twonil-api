using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   internal interface IPlayerProfileService
   {
      PlayerProfile PickRandom();
      PlayerProfile PickRandom(Position position);
   }
}
