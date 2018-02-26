using System;
using SQLite;

namespace TwoNil.Shared.DomainObjects
{
   [Table("Matches")]
   public class Match : DomainObjectBase
   {
      public int HomeScore { get; set; }
      public int AwayScore { get; set; }
      private Team _homeTeam;
      private Team _awayTeam;
      private Round _round;
      private Season _season;
      public DateTime Date { get; set; }
      public string CompetitionId { get; set; }

      public bool DrawPermitted { get; set; }
      public bool PenaltiesTaken { get; set; }
      public int HomePenaltyScore { get; set; }
      public int AwayPenaltyScore { get; set; }

      public MatchStatus MatchStatus { get; set; }
      public string HomeTeamId { get; private set; }
      public string AwayTeamId { get; private set; }
      public string RoundId { get; private set; }
      public string SeasonId { get; private set; }

      [Ignore]
      public bool Played
      {
         get
         {
            return MatchStatus == MatchStatus.Ended;
         }
      }

      [Ignore]
      public Team HomeTeam
      {
         get
         {
            return _homeTeam;
         }
         set
         {
            _homeTeam = value;
            HomeTeamId = value != null ? value.Id : null;
         }
      }

      [Ignore]
      public Team AwayTeam
      {
         get
         {
            return _awayTeam;
         }
         set
         {
            _awayTeam = value;
            AwayTeamId = value != null ? value.Id : null;
         }
      }

      [Ignore]
      public Round Round
      {
         get
         {
            return _round;
         }
         set
         {
            _round = value;
            RoundId = value != null ? value.Id : null;
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
   }
}
