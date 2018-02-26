using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality
{
   internal class TeamsAndPlayers
   {
      public TeamsAndPlayers()
      {
         Teams = new List<Team>();
         Players = new List<Player>();
      }

      public List<Team> Teams { get; set; }

      public List<Player> Players { get; set; }
   }
}
