using System.Collections.Generic;

namespace TwoNil.Shared.DomainObjects
{
   public class Position : DomainObjectBase
   {
      public string Name { get; set; }
      public string ShortName { get; set; }
      public Line Line { get; set; }
      public List<PlayerSkill> PrimarySkills { get; set; }
   }
}
