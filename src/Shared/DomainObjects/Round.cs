using System;
using System.Collections.Generic;

namespace TwoNil.Shared.DomainObjects
{
   public class Round : DomainObjectBase
   {
      public Round()
      {
         Matches = new List<Match>();
      }

      public const string Final = "Final";

      public string Name { get; set; }

      private SeasonCompetition _seasonCompetition;

      public string SeasonCompetitionId { get; set; }

      private Season _season;

      public string SeasonId { get; set; }

      public int Order { get; set; }

      public string CompetitionId { get; set; }

      public string CompetitionName { get; set; }

      public CompetitionType CompetitionType { get; set; }

      public SeasonCompetition SeasonCompetition
      {
         get
         {
            return _seasonCompetition;
         }
         set
         {
            _seasonCompetition = value;
            SeasonCompetitionId = value != null ? value.Id : null;
         }
      }

      public Season Season
      {
         get
         {
            return _season;
         }
         set
         {
            _season = value;
            SeasonId = value != null ? value.Id : null;
         }
      }

      /// <summary>
      /// If a round does not have matches when it is created, typically for knockout rounds, the date of the matches yet to be determined is stored here.
      /// Also, it is used to determine which rounds are played on the same moment.
      /// </summary>
      public DateTime MatchDate { get; set; }

      /// <summary>
      /// This is a helper property that is not stored in the database.
      /// </summary>
      public List<Match> Matches { get; set; }
   }
}
