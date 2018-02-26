using SQLite;

namespace TwoNil.Shared.DomainObjects
{
   [Table("Teams")]
   public class Team : DomainObjectBase
   {
      public string Name { get; set; }

      private Competition _currentLeagueCompetition;
      public string CurrentLeagueCompetitionId { get; private set; }

      public int CurrentLeaguePosition { get; set; }

      private Formation _formation;
      public string FormationId { get; private set; }

      // deze is denk ik tijdelijk, maar ik had even wat nodig om globaal aan tegeven hoe goed of slecht een team is...
      public decimal Rating { get; set; }

      [Ignore]
      public Competition CurrentLeagueCompetition
      {
         get
         {
            return _currentLeagueCompetition;
         }
         set
         {
            _currentLeagueCompetition = value;
            CurrentLeagueCompetitionId = value != null ? value.Id : null;
         }
      }

      [Ignore]
      public Formation Formation
      {
         get
         {
            return _formation;
         }
         set
         {
            _formation = value;
            FormationId = value != null ? value.Id : null;
         }
      }

      // De volgende properties moeten calculated properties worden denk ik
      //public TeamSkill GoalKeeping { get; set; }
      //public TeamSkill Midfield { get; set; }
      //public TeamSkill Attack { get; set; }
      //public TeamSkill Fitness { get; set; }
      //public TeamSkill Confidence { get; set; }
   }
}
