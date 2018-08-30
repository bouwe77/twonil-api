using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
   public class PlayerProfileRepository : MemoryRepository<PlayerProfile>
   {
      public PlayerProfileRepository()
      {
         Entities = new List<PlayerProfile>
         {
            GetGoalkeeperProfile(),
            GetSweeperProfile(),
            GetCentreBackProfile(),
            GetWingBackProfile(),
            GetDefensiveMidfieldProfile(),
            GetCentralMidfieldProfile(),
            GetWideMidfieldProfile(),
            GetForwardMidfieldProfile(),
            GetStrikerProfile(),
            GetCentreForwardProfile(),
            GetWingerProfile(),
            GetVersatileProfile()
         };
      }

      /// <summary>
      /// Gets the profile for a goalkeeper.
      /// </summary>
      public PlayerProfile GetGoalkeeperProfile()
      {
         return InMemoryData.GetGoalkeeperProfile();
      }

      /// <summary>
      /// Gets the profile for a sweeper.
      /// </summary>
      public PlayerProfile GetSweeperProfile()
      {
         return InMemoryData.GetSweeperProfile();
      }

      /// <summary>
      /// Gets the profile for a centre back.
      /// </summary>
      public PlayerProfile GetCentreBackProfile()
      {
         return InMemoryData.GetCentreBackProfile();
      }

      /// <summary>
      /// Gets the profile for a Wing back.
      /// </summary>
      public PlayerProfile GetWingBackProfile()
      {
         return InMemoryData.GetWingBackProfile();
      }

      /// <summary>
      /// Gets the profile for a Defensive midfield.
      /// </summary>
      public PlayerProfile GetDefensiveMidfieldProfile()
      {
         return InMemoryData.GetDefensiveMidfieldProfile();
      }

      /// <summary>
      /// Gets the profile for a Central midfield.
      /// </summary>
      public PlayerProfile GetCentralMidfieldProfile()
      {
         return InMemoryData.GetCentralMidfieldProfile();
      }

      /// <summary>
      /// Gets the profile for a Forward midfield.
      /// </summary>
      public PlayerProfile GetForwardMidfieldProfile()
      {
         return InMemoryData.GetForwardMidfieldProfile();
      }

      /// <summary>
      /// Gets the profile for a Wide midfield.
      /// </summary>
      public PlayerProfile GetWideMidfieldProfile()
      {
         return InMemoryData.GetWideMidfieldProfile();
      }

      /// <summary>
      /// Gets the profile for a Striker.
      /// </summary>
      public PlayerProfile GetStrikerProfile()
      {
         return InMemoryData.GetStrikerProfile();
      }

      /// <summary>
      /// Gets the profile for a False nine.
      /// </summary>
      public PlayerProfile GetCentreForwardProfile()
      {
         return InMemoryData.GetCentreForwardProfile();
      }

      /// <summary>
      /// Gets the profile for a Winger.
      /// </summary>
      public PlayerProfile GetWingerProfile()
      {
         return InMemoryData.GetWingerProfile();
      }

      /// <summary>
      /// Gets a profile for a versatile field player.
      /// </summary>
      public PlayerProfile GetVersatileProfile()
      {
         return InMemoryData.GetVersatileProfile();
      }
   }
}
