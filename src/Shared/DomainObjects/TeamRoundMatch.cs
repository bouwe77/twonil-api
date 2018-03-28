using System;

namespace TwoNil.Shared.DomainObjects
{
   /// <summary>
   /// Represents a round and (if applicable) a match of a team. This class kind of combines the <see cref="Round"/> and <see cref="Match"/> domain objects.
   /// This class does not derive from DomainObjectBase because it is not stored in the database.
   /// </summary>
   public class TeamRoundMatch
   {
      public string GameId { get; set; }
      public string MatchId { get; set; }
      public DateTime MatchDate { get; set; }
      public string CompetitionId { get; set; }
      public string CompetitionName { get; set; }
      public string RoundName { get; set; }
      public string HomeTeamId { get; set; }
      public string AwayTeamId { get; set; }
      public int HomeScore { get; set; }
      public int AwayScore { get; set; }
      public bool PenaltiesTaken { get; set; }
      public int HomePenaltyScore { get; set; }
      public int AwayPenaltyScore { get; set; }
      public MatchStatus MatchStatus { get; set; }

      public bool Played => MatchStatus == MatchStatus.Ended;

      private Team _homeTeam;
      private Team _awayTeam;

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
   }
}
