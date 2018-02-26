using TwoNil.Data.Memory;

namespace TwoNil.Data
{
   public class MemoryRepositoryFactory
   {
      public PlayerSkillRepository CreatePlayerSkillRepository()
      {
         return new PlayerSkillRepository();
      }

      public LineRepository CreateLineRepository()
      {
         return new LineRepository();
      }

      public FormationRepository CreateFormationRepository()
      {
         return new FormationRepository();
      }

      public CompetitionRepository CreateCompetitionRepository()
      {
         return new CompetitionRepository();
      }

      public PlayerProfileRepository CreatePlayerProfileRepository()
      {
         return new PlayerProfileRepository();
      }

      public IPositionRepository CreatePositionRepository()
      {
         return new PositionRepository();
      }

      public NameRepository CreateNameRepository()
      {
         return new NameRepository();
      }
   }
}
