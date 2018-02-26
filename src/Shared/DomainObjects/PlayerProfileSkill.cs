namespace TwoNil.Shared.DomainObjects
{
   public class PlayerProfileSkill : DomainObjectBase
   {
      public PlayerProfileSkill(PlayerSkill playerSkill, ProfileSkillPriority profileSkillPriority)
      {
         Skill = playerSkill;
         SkillPriority = profileSkillPriority;
      }

      public PlayerSkill Skill { get; private set; }
      public ProfileSkillPriority SkillPriority { get; set; }
   }
}
