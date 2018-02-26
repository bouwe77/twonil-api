using System;
using System.Collections.Generic;
using SQLite;

namespace TwoNil.Shared.DomainObjects
{
   [Table("Rounds")]
   public class Round : DomainObjectBase
   {
      public Round()
      {
         Matches = new List<Match>();
      }

      public const string Final = "Final";

      public string Name { get; set; }

      private SeasonCompetition _seasonCompetition;

      public string SeasonCompetitionId { get; private set; }

      private Season _season;

      public string SeasonId { get; private set; }

      public int Order { get; set; }

      public string CompetitionId { get; set; }

      public string CompetitionName { get; set; }

      public CompetitionType CompetitionType { get; set; }

      [Ignore]
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

      [Ignore]
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
      /// If a round does not have matches when it is created, typically for knockout rounds, the date of the matches is stored here.
      /// Also, it is used to determine which rounds are played on the same moment.
      /// NOTE: This field is not intended to be displayed i.e. returned by the REST API.
      /// </summary>
      public DateTime MatchDate { get; set; }

      /// <summary>
      /// This is a helper property that is not stored in the database.
      /// </summary>
      [Ignore]
      public List<Match> Matches { get; set; }
   }
}
