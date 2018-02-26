using System.Collections.Generic;

namespace TwoNil.Shared.DomainObjects
{
   public class Formation : DomainObjectBase
   {
      public string Name { get; set; }
      public int Defenders { get; set; }
      public int Midfielders { get; set; }
      public int Attackers { get; set; }
      public List<Position> Positions { get; set; } 
   }
}
