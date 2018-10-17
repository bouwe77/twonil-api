namespace TwoNil.Shared.DomainObjects
{
   public class Player : DomainObjectBase
   {
      public string Name { get; set; }
      public int Age { get; set; }

      /// <summary>
      /// The order of the player within the team squad.
      /// </summary>
      public int TeamOrder { get; set; }
      public bool InStartingEleven { get; set; }

      public int SkillGoalkeeping { get; set; }
      public int SkillDefending { get; set; }
      public int SkillPassing { get; set; }
      public int SkillSpeed { get; set; }
      public int SkillTechnique { get; set; }
      public int SkillShooting { get; set; }
      public int SkillHeading { get; set; }
      public int SkillTactics { get; set; }
      
      // Physical skills:
      public int SkillFitness { get; set; }
      public int SkillPace { get; set; }
      public int SkillStamina { get; set; }
      public int SkillStrength { get; set; }

      // Mental skills:
      public int SkillForm { get; set; }
      public int SkillConfidence { get; set; }
      public int SkillMentality { get; set; }
      public int SkillIntelligence { get; set; }

      public string PlayerProfile { get; set; }

      public decimal Rating { get; set; }
      public decimal RatingGoalkeeping { get; set; }
      public decimal RatingDefence { get; set; }
      public decimal RatingMidfield { get; set; }
      public decimal RatingAttack { get; set; }

      private Team _team;
      public string TeamId { get; set; }

      public Team Team
      {
         get => _team;
         set
         {
            _team = value;
            TeamId = value?.Id;
         }
      }

      private Position _preferredPosition;
      public string PreferredPositionId { get; set; }

      public Position PreferredPosition
      {
         get => _preferredPosition;
         set
         {
            _preferredPosition = value;
            PreferredPositionId = value?.Id;
         }
      }

      private Position _currentPosition;
      public string CurrentPositionId { get; set; }

      public Position CurrentPosition
      {
         get => _currentPosition;
         set
         {
            _currentPosition = value;
            CurrentPositionId = value?.Id;
         }
      }

      public override string ToString()
      {
         return $"Name = '{Name}'";
      }
   }
}
