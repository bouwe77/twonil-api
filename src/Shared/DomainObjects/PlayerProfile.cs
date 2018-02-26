using System.Collections.Generic;

namespace TwoNil.Shared.DomainObjects
{
   public class PlayerProfile : DomainObjectBase
   {
      public string Name { get; set; }
      public List<Line> Lines { get; set; }
      public List<Position> Positions { get; set; }
      public List<PlayerProfileSkill> PlayerProfileSkills { get; set; }
   }
}
