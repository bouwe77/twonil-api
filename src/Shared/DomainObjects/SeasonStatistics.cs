using SQLite;

namespace TwoNil.Shared.DomainObjects
{
   [Table("SeasonStatistics")]
   public class SeasonStatistics : DomainObjectBase
   {
      public SeasonStatistics()
      {
      }

      public SeasonStatistics(
         Season season,
         string league1LeagueTableId,
         string league2LeagueTableId,
         string league3LeagueTableId,
         string league4LeagueTableId)
      {
         Season = season;
         League1LeagueTableId = league1LeagueTableId;
         League2LeagueTableId = league2LeagueTableId;
         League3LeagueTableId = league3LeagueTableId;
         League4LeagueTableId = league4LeagueTableId;
      }

      public string League1LeagueTableId { get; set; }
      public string League2LeagueTableId { get; set; }
      public string League3LeagueTableId { get; set; }
      public string League4LeagueTableId { get; set; }

      private Season _season;
      public string SeasonId { get; private set; }

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
            SeasonId = value?.Id;
         }
      }

      private Team _cupWinner;
      public string CupWinnerTeamId { get; private set; }

      [Ignore]
      public Team CupWinner
      {
         get
         {
            return _cupWinner;
         }
         set
         {
            _cupWinner = value;
            CupWinnerTeamId = value?.Id;
         }
      }

      private Team _cupRunnerUp;
      public string CupRunnerUpTeamId { get; private set; }

      [Ignore]
      public Team CupRunnerUp
      {
         get
         {
            return _cupRunnerUp;
         }
         set
         {
            _cupRunnerUp = value;
            CupRunnerUpTeamId = value?.Id;
         }
      }

      private Team _nationalChampion;
      public string NationalChampionTeamId { get; private set; }

      [Ignore]
      public Team NationalChampion
      {
         get
         {
            return _nationalChampion;
         }
         set
         {
            _nationalChampion = value;
            NationalChampionTeamId = value?.Id;
         }
      }

      private Team _nationalChampionRunnerUp;
      public string NationalChampionRunnerUpTeamId { get; private set; }

      [Ignore]
      public Team NationalChampionRunnerUp
      {
         get
         {
            return _nationalChampionRunnerUp;
         }
         set
         {
            _nationalChampionRunnerUp = value;
            NationalChampionRunnerUpTeamId = value?.Id;
         }
      }

      private Team _nationalSuperCupWinner;
      public string NationalSuperCupWinnerTeamId { get; private set; }

      [Ignore]
      public Team NationalSuperCupWinner
      {
         get
         {
            return _nationalSuperCupWinner;
         }
         set
         {
            _nationalSuperCupWinner = value;
            NationalSuperCupWinnerTeamId = value?.Id;
         }
      }
   }
}
