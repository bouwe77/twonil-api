using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Data.Repositories;
using TwoNil.Logic.Exceptions;
using TwoNil.Logic.Functionality.Calendar;
using TwoNil.Logic.Functionality.Competitions.Friendlies;
using TwoNil.Logic.Functionality.Teams;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions
{
    internal class SeasonManager
    {
        private readonly LeagueManager _leagueManager;
        private readonly RepositoryFactory _repositoryFactory;

        public SeasonManager(RepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _leagueManager = new LeagueManager();
        }

        public void CreateFirstSeason(List<Team> teams, TransactionManager transactionManager)
        {
            // First check the number of teams can be evenly divided into the number of leagues.
            bool teamsOk = teams.Count % Constants.HowManyLeagues == 0;
            if (!teamsOk)
            {
                throw new Exception($"The number of teams must be divided by {Constants.HowManyLeagues}");
            }

            var newSeasonInfo = new NewSeasonInfo { StartYear = DateTime.Now.Year };

            // Divide all teams between the four leagues based on the team rating.
            teams.Sort((team1, team2) => team2.Rating.CompareTo(team1.Rating));
            int countTeamsPerLeague = teams.Count / Constants.HowManyLeagues;
            newSeasonInfo.TeamsLeague1 = teams.Take(countTeamsPerLeague).ToList();
            newSeasonInfo.TeamsLeague2 = teams.Skip(countTeamsPerLeague).Take(countTeamsPerLeague).ToList();
            newSeasonInfo.TeamsLeague3 = teams.Skip(countTeamsPerLeague * 2).Take(countTeamsPerLeague).ToList();
            newSeasonInfo.TeamsLeague4 = teams.Skip(countTeamsPerLeague * 3).ToList();

            // The teams have been sorted on rating, so given them an initial league table position.
            AssignInitialLeagueTablePosition(teams);

            // In the first season there are no champion and cup winner yet, so pick the two best teams.
            // Because the champion and cup winner are determined via the previous season's statistics, create a dummy PreviousSeasonStatistics for this.
            var dummySeasonStatistics = new SeasonStatistics() { NationalChampion = teams[0], CupWinner = teams[1], NationalChampionRunnerUp = teams[1] };
            newSeasonInfo.PreviousSeasonStatistics = dummySeasonStatistics;

            // Now all teams have been placed in the right leagues, so create match schedules for all competitions.
            var seasonAndCompetitionSchedules = CreateSeasonAndCompetitionSchedules(newSeasonInfo);

            // Insert the season and all competition schedules.
            InsertSeasonAndCompetitionSchedule(transactionManager, seasonAndCompetitionSchedules);

            InsertGameDateTimes(transactionManager, seasonAndCompetitionSchedules.MatchDates);

            // Insert statistics.
            InsertSeasonRelatedStatistics(transactionManager, teams, seasonAndCompetitionSchedules);
            InsertTeamStatistics(transactionManager, teams);
        }

        public void CreateNextSeason(Season previousSeason, TransactionManager transactionManager)
        {
            if (previousSeason.SeasonStatus != SeasonStatus.Ended)
            {
                throw new ConflictException("Season must be ended before a new one can be created");
            }

            var newSeasonInfo = new NewSeasonInfo
            {
                StartYear = previousSeason.StartYear + 1
            };

            // Determine which teams promote and relegate.
            List<Team> allTeamsSortedOnLeagueAndPosition;
            using (var teamRepository = _repositoryFactory.CreateTeamRepository())
            {
                allTeamsSortedOnLeagueAndPosition = teamRepository.GetTeams().OrderBy(x => x.CurrentLeagueCompetition.Order).ThenBy(x => x.CurrentLeaguePosition).ToList();
            }

            var teamsGroupedPerLeague = allTeamsSortedOnLeagueAndPosition.GroupBy(t => t.CurrentLeagueCompetitionId).Select(grp => grp.ToList()).ToList();
            var newLeagues = _leagueManager.PromoteAndRelegateTeams(teamsGroupedPerLeague, Constants.HowManyTeamsPromoteOrRelegate);

            newSeasonInfo.TeamsLeague1 = newLeagues[0];
            newSeasonInfo.TeamsLeague2 = newLeagues[1];
            newSeasonInfo.TeamsLeague3 = newLeagues[2];
            newSeasonInfo.TeamsLeague4 = newLeagues[3];

            // Determine the previous season's statistics.
            using (var seasonStatisticsRepository = _repositoryFactory.CreateSeasonStatisticsRepository())
            {
                var seasonStatistics = seasonStatisticsRepository.GetBySeason(previousSeason.Id);
                newSeasonInfo.PreviousSeasonStatistics = seasonStatistics;
            }

            // Now all teams have been placed in the right leagues, so create match schedules for all competitions.
            var seasonAndCompetitionSchedules = CreateSeasonAndCompetitionSchedules(newSeasonInfo);

            // Insert the season and all competition schedules.
            InsertSeasonAndCompetitionSchedule(transactionManager, seasonAndCompetitionSchedules);

            InsertGameDateTimes(transactionManager, seasonAndCompetitionSchedules.MatchDates);

            // Insert statistics.
            InsertSeasonRelatedStatistics(transactionManager, allTeamsSortedOnLeagueAndPosition, seasonAndCompetitionSchedules);
        }

        private void InsertSeasonRelatedStatistics(TransactionManager transactionManager, IEnumerable<Team> teams, SeasonAndCompetitionSchedules seasonAndCompetitionSchedules)
        {
            string league1LeagueTableId = seasonAndCompetitionSchedules.LeaguesSchedule.LeagueTables[0].Id;
            string league2LeagueTableId = seasonAndCompetitionSchedules.LeaguesSchedule.LeagueTables[1].Id;
            string league3LeagueTableId = seasonAndCompetitionSchedules.LeaguesSchedule.LeagueTables[2].Id;
            string league4LeagueTableId = seasonAndCompetitionSchedules.LeaguesSchedule.LeagueTables[3].Id;

            // SeasonStatistics.
            var seasonStatistics = new SeasonStatistics(seasonAndCompetitionSchedules.Season, league1LeagueTableId, league2LeagueTableId, league3LeagueTableId, league4LeagueTableId);
            transactionManager.RegisterInsert(seasonStatistics);

            // SeasonTeamStatistics.
            foreach (var team in teams)
            {
                var seasonTeamStatistic = new SeasonTeamStatistics(seasonAndCompetitionSchedules.Season, team, team.CurrentLeagueCompetition.Name);
                transactionManager.RegisterInsert(seasonTeamStatistic);
            }
        }

        private void InsertTeamStatistics(TransactionManager transactionManager, List<Team> teams)
        {
            foreach (var team in teams)
            {
                var teamStatistics = new TeamStatistics(team);
                transactionManager.RegisterInsert(teamStatistics);
            }
        }

        private static void AssignInitialLeagueTablePosition(List<Team> teams)
        {
            int position = 1;
            int i = 0;
            foreach (var team in teams)
            {
                if (i % Constants.HowManyTeamsPerLeague == 0)
                {
                    position = 1;
                }

                team.CurrentLeaguePosition = position;

                position++;
                i++;
            }
        }

        private void InsertSeasonAndCompetitionSchedule(TransactionManager transactionManager, SeasonAndCompetitionSchedules seasonAndCompetitionSchedules)
        {
            // Insert the season.
            transactionManager.RegisterInsert(seasonAndCompetitionSchedules.Season);

            // Insert league schedules.
            SaveCompetitionSchedule(seasonAndCompetitionSchedules.LeaguesSchedule, transactionManager);

            // Insert cup schedule.
            SaveCompetitionSchedule(seasonAndCompetitionSchedules.NationalCupSchedule, transactionManager);

            // Insert super cup schedule.
            SaveCompetitionSchedule(seasonAndCompetitionSchedules.NationalSuperCupSchedule, transactionManager);

            // Insert pre-season friendly schedule.
            SaveCompetitionSchedule(seasonAndCompetitionSchedules.PreSeasonFriendliesSchedule, transactionManager);

            // Insert during season friendlies.
            SaveCompetitionSchedule(seasonAndCompetitionSchedules.DuringSeasonFriendliesSchedule, transactionManager);

            // Update the teams.
            transactionManager.RegisterUpdate(seasonAndCompetitionSchedules.Teams);
        }

        private SeasonAndCompetitionSchedules CreateSeasonAndCompetitionSchedules(NewSeasonInfo newSeasonInfo)
        {
            var matchDateManager = new MatchDateManager(newSeasonInfo.StartYear);
            matchDateManager.Initialize();

            // The season officially ends a day after the last match so there is a possibility to add events between the last match and the end of the season.
            var endSeasonDateTime = matchDateManager.GetAllMatchDates().Last().AddDays(1);

            var season = new Season
            {
                StartYear = newSeasonInfo.StartYear,
                SeasonStatus = SeasonStatus.Started,
                EndDateTime = endSeasonDateTime
            };

            var seasonAndCompetitionSchedules = new SeasonAndCompetitionSchedules { Season = season };

            // Add all dates that are determined here to the schedule so the datetime navigation can be updated.
            seasonAndCompetitionSchedules.MatchDates = matchDateManager.GetAllMatchDates();
            seasonAndCompetitionSchedules.OtherDates = new List<DateTime> { endSeasonDateTime };

            // Create leagues and schedule.
            var leagueManager = new LeagueManager();
            seasonAndCompetitionSchedules.LeaguesSchedule = leagueManager.CreateSchedules(newSeasonInfo.TeamsLeague1, newSeasonInfo.TeamsLeague2, newSeasonInfo.TeamsLeague3, newSeasonInfo.TeamsLeague4, season, matchDateManager);

            // Create a national cup tournament.
            var nationalCupManager = new NationalCupManager(_repositoryFactory);
            seasonAndCompetitionSchedules.NationalCupSchedule = nationalCupManager.CreateSchedule(newSeasonInfo.AllTeams.ToList(), season, matchDateManager);

            // Create pre-season friendlies.
            var preSeasonFriendlyManager = new PreSeasonFriendlyManager();
            seasonAndCompetitionSchedules.PreSeasonFriendliesSchedule = preSeasonFriendlyManager.CreatePreSeasonSchedule(newSeasonInfo.AllTeams.ToList(), season, matchDateManager);

            // Create friendlies during the season.
            // Determine on which dates these friendlies can be played. For now, this is only during the national cup tournament, except the first round and the final.
            var cupDates = seasonAndCompetitionSchedules.NationalCupSchedule.Rounds.Select(r => r.MatchDate).Skip(1).Take(seasonAndCompetitionSchedules.NationalCupSchedule.Rounds.Count - 2).ToList();
            var friendlyRoundsManager = new DuringSeasonFriendlyRoundsManager();
            seasonAndCompetitionSchedules.DuringSeasonFriendliesSchedule = friendlyRoundsManager.CreateDuringSeasonFriendlyRounds(seasonAndCompetitionSchedules.PreSeasonFriendliesSchedule.SeasonCompetitions.First(), cupDates, Constants.HowManyPreSeasonFriendlies + 1);

            // Create Super Cup.
            var nationalSuperCupManager = new NationalSuperCupManager();
            // The home team is always the national champion and the away team is either the cup winner or the league 1 runner up if the champion also won the cup.
            var homeTeam = newSeasonInfo.PreviousSeasonStatistics.NationalChampion;
            var awayTeam = newSeasonInfo.PreviousSeasonStatistics.NationalChampion.Equals(newSeasonInfo.PreviousSeasonStatistics.CupWinner) ? newSeasonInfo.PreviousSeasonStatistics.NationalChampionRunnerUp : newSeasonInfo.PreviousSeasonStatistics.CupWinner;
            seasonAndCompetitionSchedules.NationalSuperCupSchedule = nationalSuperCupManager.CreateSchedule(homeTeam, awayTeam, season, matchDateManager);

            // In the meantime data of the teams has changed, so add them to the SeasonAndCompetitionSchedules object so they can be updated in the database.
            seasonAndCompetitionSchedules.Teams = newSeasonInfo.AllTeams;

            return seasonAndCompetitionSchedules;
        }

        public void EndSeason(Season season, TransactionManager transactionManager)
        {
            // Get all league tables and update the team statistics.
            using (var leagueTableRepository = _repositoryFactory.CreateLeagueTableRepository())
            {
                var leagueTables = leagueTableRepository.GetBySeason(season.Id);
                var teamStatisticsManager = new TeamStatisticsManager(transactionManager, _repositoryFactory);
                teamStatisticsManager.Update(leagueTables);
            }

            // End the season by updating the status.
            season.SeasonStatus = SeasonStatus.Ended;
            transactionManager.RegisterUpdate(season);
        }

        private void SaveCompetitionSchedule(CompetitionSchedule competitionSchedule, TransactionManager transactionManager)
        {
            transactionManager.RegisterInsert(competitionSchedule.LeagueTables);
            foreach (var leagueTable in competitionSchedule.LeagueTables)
            {
                transactionManager.RegisterInsert(leagueTable.LeagueTablePositions);
            }

            transactionManager.RegisterInsert(competitionSchedule.SeasonCompetitions);

            transactionManager.RegisterInsert(competitionSchedule.SeasonCompetitionTeams);

            transactionManager.RegisterInsert(competitionSchedule.Rounds);

            transactionManager.RegisterInsert(competitionSchedule.Matches);
        }

        private static void InsertGameDateTimes(TransactionManager transactionManager, IEnumerable<DateTime> matchDates)
        {
            var gameDateTimes = new List<GameDateTime>();
            foreach (var matchDate in matchDates)
            {
                var gameDateTime = GameDateTimeFactory.Create(matchDate, GameDateTimeEventStatus.ToDo);
                gameDateTimes.Add(gameDateTime);
            }

            gameDateTimes.OrderBy(g => g.DateTime).First().Status = GameDateTimeStatus.Now;

            transactionManager.RegisterInsert(gameDateTimes);
        }
    }
}
