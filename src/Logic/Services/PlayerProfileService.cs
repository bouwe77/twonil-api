using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
   internal class PlayerProfileService : ServiceBase
   {
      /// <summary>
      /// Picks a random <see cref="PlayerProfile"/>.
      /// </summary>
      public PlayerProfile PickRandom()
      {
         using (var playerProfileRepository = new RepositoryFactory().CreatePlayerProfileRepository())
         {
            var playerProfiles = playerProfileRepository.GetAll();
            var pickedPlayerProfile = GetRandomPlayerProfile(playerProfiles);

            return pickedPlayerProfile;
         }
      }

      /// <summary>
      /// Picks a random <see cref="PlayerProfile"/> which has the specified Position.
      /// </summary>
      public PlayerProfile PickRandom(Position position)
      {
         using (var playerProfileRepository = new RepositoryFactory().CreatePlayerProfileRepository())
         {
            var playerProfiles = playerProfileRepository.Find(playerProfile => playerProfile.Positions.Contains(position)).ToList();
            var pickedPlayerProfile = GetRandomPlayerProfile(playerProfiles);

            return pickedPlayerProfile;
         }
      }

      private PlayerProfile GetRandomPlayerProfile(IEnumerable<PlayerProfile> playerProfiles)
      {
         var randomPlayerProfile = ListRandomizer.GetItem(playerProfiles);
         return randomPlayerProfile;
      }
   }
}
