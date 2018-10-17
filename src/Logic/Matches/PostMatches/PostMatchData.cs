using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Matches.PostMatches
{
    // Contains data that must be persisted to the database after matches have been played.
    public class PostMatchData
    {
        public PostMatchData(
            string competitionIdLeague1,
            string competitionIdLeague2,
            string competitionIdLeague3,
            string competitionIdLeague4,
            string competitionIdNationalCup,
            string competitionIdNationalSuperCup,
            string competitionIdFriendlies)
        {
            CompetitionIdLeague1 = competitionIdLeague1;
            CompetitionIdLeague2 = competitionIdLeague2;
            CompetitionIdLeague3 = competitionIdLeague3;
            CompetitionIdLeague4 = competitionIdLeague4;
            CompetitionIdNationalCup = competitionIdNationalCup;
            CompetitionIdNationalSuperCup = competitionIdNationalSuperCup;
            CompetitionIdFriendlies = competitionIdFriendlies;

            CupMatchesNextRound = new List<Match>();
            DuringSeasonFriendlies = new List<Match>();
        }

        public Dictionary<string, IEnumerable<Match>> Matches { get; set; }
        public Dictionary<string, Round> Rounds { get; set; }
        public Dictionary<string, Team> Teams { get; set; }
        public Dictionary<string, LeagueTable> LeagueTables { get; set; }
        public Dictionary<string, League> Leagues { get; set; }
        public DateTime MatchDateTime { get; set; }
        public Team ManagersTeam { get; set; }
        public Season Season { get; set; }
        public SeasonStatistics SeasonStatistics { get; set; }
        public Dictionary<string, SeasonTeamStatistics> SeasonTeamStatistics { get; set; }
        public Dictionary<string, TeamStatistics> TeamStatistics { get; set; }

        public IEnumerable<Match> CupMatchesNextRound { get; set; }
        public IEnumerable<Match> DuringSeasonFriendlies { get; set; }

        public string CompetitionIdLeague1 { get; }
        public string CompetitionIdLeague2 { get; }
        public string CompetitionIdLeague3 { get; }
        public string CompetitionIdLeague4 { get; }
        public string CompetitionIdNationalCup { get; }
        public string CompetitionIdNationalSuperCup { get; }
        public string CompetitionIdFriendlies { get; }

        public LeagueTable LeagueTableLeague1 => GetItemFromDictionary(LeagueTables, CompetitionIdLeague1);
        public LeagueTable LeagueTableLeague2 => GetItemFromDictionary(LeagueTables, CompetitionIdLeague2);
        public LeagueTable LeagueTableLeague3 => GetItemFromDictionary(LeagueTables, CompetitionIdLeague3);
        public LeagueTable LeagueTableLeague4 => GetItemFromDictionary(LeagueTables, CompetitionIdLeague4);

        public IEnumerable<Match> MatchesLeague1 => GetCollectionFromDictionary(Matches, CompetitionIdLeague1);
        public IEnumerable<Match> MatchesLeague2 => GetCollectionFromDictionary(Matches, CompetitionIdLeague2);
        public IEnumerable<Match> MatchesLeague3 => GetCollectionFromDictionary(Matches, CompetitionIdLeague3);
        public IEnumerable<Match> MatchesLeague4 => GetCollectionFromDictionary(Matches, CompetitionIdLeague4);
        public IEnumerable<Match> MatchesNationalCup => GetCollectionFromDictionary(Matches, CompetitionIdNationalCup);
        public IEnumerable<Match> MatchesNationalSuperCup => GetCollectionFromDictionary(Matches, CompetitionIdNationalSuperCup);

        public Round RoundNationalCup => GetItemFromDictionary(Rounds, CompetitionIdNationalCup);

        public bool League1Finished { get; set; }
        public bool NationalCupFinalHasBeenPlayed => RoundNationalCup?.Name == Round.Final;

        public IEnumerable<DateTime> NewManagerMatchDates
        {
            get
            {
                var newManagerMatchDates = new List<DateTime>();

                var newFriendlyMatchForManager = DuringSeasonFriendlies.SingleOrDefault(m => m.HomeTeamId == ManagersTeam.Id || m.AwayTeamId == ManagersTeam.Id);
                if (newFriendlyMatchForManager != null)
                    newManagerMatchDates.Add(newFriendlyMatchForManager.Date);

                var newCupMatchForManager = CupMatchesNextRound.SingleOrDefault(m => m.HomeTeamId == ManagersTeam.Id || m.AwayTeamId == ManagersTeam.Id);
                if (newCupMatchForManager != null)
                    newManagerMatchDates.Add(newCupMatchForManager.Date);

                return newManagerMatchDates;
            }
        }

        private IEnumerable<T> GetCollectionFromDictionary<T>(Dictionary<string, IEnumerable<T>> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];

            return new List<T>();
        }

        private T GetItemFromDictionary<T>(Dictionary<string, T> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];

            return default(T);
        }
    }
}
