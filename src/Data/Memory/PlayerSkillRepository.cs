using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Memory
{
   public class PlayerSkillRepository : MemoryRepository<PlayerSkill>
   {
      public PlayerSkillRepository()
      {
         Entities = new List<PlayerSkill>
         {
            GetGoalkeeping(),
            GetDefending(),
            GetPassing(),
            GetSpeed(),
            GetTechnique(),
            GetShooting(),
            GetHeading(),
            GetTactics(),
            GetFitness(),
            GetTalent(),
            GetForm(),
            GetConfidence()
         };
      }

      public PlayerSkill GetGoalkeeping()
      {
         return InMemoryData.GetGoalkeeping();
      }

      public PlayerSkill GetDefending()
      {
         return InMemoryData.GetDefending();
      }

      public PlayerSkill GetPassing()
      {
         return InMemoryData.GetPassing();
      }

      public PlayerSkill GetSpeed()
      {
         return InMemoryData.GetSpeed();
      }

      public PlayerSkill GetTechnique()
      {
         return InMemoryData.GetTechnique();
      }

      public PlayerSkill GetShooting()
      {
         return InMemoryData.GetShooting();
      }

      public PlayerSkill GetHeading()
      {
         return InMemoryData.GetHeading();
      }

      public PlayerSkill GetTactics()
      {
         return InMemoryData.GetTactics();
      }

      public PlayerSkill GetFitness()
      {
         return InMemoryData.GetFitness();
      }

      public PlayerSkill GetTalent()
      {
         return InMemoryData.GetTalent();
      }

      public PlayerSkill GetForm()
      {
         return InMemoryData.GetForm();
      }

      public PlayerSkill GetConfidence()
      {
         return InMemoryData.GetConfidence();
      }

      public PlayerSkill GetPace()
      {
         return InMemoryData.GetPace();
      }

      public PlayerSkill GetStamina()
      {
         return InMemoryData.GetStamina();
      }

      public PlayerSkill GetStrength()
      {
         return InMemoryData.GetStrength();
      }

      public PlayerSkill GetMentality()
      {
         return InMemoryData.GetMentality();
      }

      public PlayerSkill GetIntelligence()
      {
         return InMemoryData.GetIntelligence();
      }
   }
}
