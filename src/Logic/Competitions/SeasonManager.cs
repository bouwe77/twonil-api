using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Exceptions;
using TwoNil.Logic.Calendar;
using TwoNil.Logic.Competitions.Friendlies;
using TwoNil.Logic.Matches;
using TwoNil.Logic.Teams;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Competitions
{
    public interface ISeasonManager
    {
        void CreateFirstSeason(List<Team> teams, IUnitOfWork uow);
        void CreateNextSeason(Season previousSeason, IUnitOfWork uow);
        void EndSeason(Season season, IUnitOfWork uow);
    }

    public class SeasonManager : ISeasonManager
    {
        private readonly ILeagueManager _leagueManager;
        private readonly IPreSeasonFriendlyManager _preSeasonFriendlyManager;
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly INationalCupManager _nationalCupManager;
        private readonly INationalSuperCupManager _nationalSuperCupManager;
        private readonly IDuringSeasonFriendlyRoundsManager _duringSeasonFriendlyRoundsManager;
        private readonly IGameDateTimeMutationManager _gameDateTimeMutationManager;

        public SeasonManager(
            IUnitOfWorkFactory uowFactory,
            ILeagueManager leagueManager,
            IPreSeasonFriendlyManager preSeasonFriendlyManager,
            INationalCupManager nationalCupManager,
            INationalSuperCupManager nationalSuperCupManager,
            IDuringSeasonFriendlyRoundsManager duringSeasonFriendlyRoundsManager,
            IGameDateTimeMutationManager gameDateTimeMutationManager)
        {
            _uowFactory = uowFactory;
            _leagueManager = leagueManager;
            _preSeasonFriendlyManager = preSeasonFriendlyManager;
            _nationalCupManager = nationalCupManager;
            _nationalSuperCupManager = nationalSuperCupManager;
            _duringSeasonFriendlyRoundsManager = duringSeasonFriendlyRoundsManager;
            _gameDateTimeMutationManager = gameDateTimeMutationManager;
        }

        public void CreateFirstSeason(List<Team> teams, IUnitOfWork uow)
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
            InsertSeasonAndCompetitionSchedule(seasonAndCompetitionSchedules, uow);

            InsertGameDateTimes(seasonAndCompetitionSchedules, uow);

            // Insert statistics.
            InsertSeasonRelatedStatistics(teams, seasonAndCompetitionSchedules, uow);
            InsertTeamStatistics(teams, uow);
        }

        public void CreateNextSeason(Season previousSeason, IUnitOfWork uow)
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
            allTeamsSortedOnLeagueAndPosition = uow.Teams.GetTeams().OrderBy(x => x.CurrentLeagueCompetition.Order).ThenBy(x => x.CurrentLeaguePosition).ToList();

            var teamsGroupedPerLeague = allTeamsSortedOnLeagueAndPosition.GroupBy(t => t.CurrentLeagueCompetitionId).Select(grp => grp.ToList()).ToList();
            var newLeagues = _leagueManager.PromoteAndRelegateTeams(teamsGroupedPerLeague, Constants.HowManyTeamsPromoteOrRelegate);

            newSeasonInfo.TeamsLeague1 = newLeagues[0];
            newSeasonInfo.TeamsLeague2 = newLeagues[1];
            newSeasonInfo.TeamsLeague3 = newLeagues[2];
            newSeasonInfo.TeamsLeague4 = newLeagues[3];

            // Determine the previous season's statistics.
            var seasonStatistics = uow.SeasonStatics.GetBySeason(previousSeason.Id);
            newSeasonInfo.PreviousSeasonStatistics = seasonStatistics;

            // Now all teams have been placed in the right leagues, so create match schedules for all competitions.
            var seasonAndCompetitionSchedules = CreateSeasonAndCompetitionSchedules(newSeasonInfo);

            // Insert the season and all competition schedules.
            InsertSeasonAndCompetitionSchedule(seasonAndCompetitionSchedules, uow);

            InsertGameDateTimes(seasonAndCompetitionSchedules, uow);

            // Insert statistics.
            InsertSeasonRelatedStatistics(allTeamsSortedOnLeagueAndPosition, seasonAndCompetitionSchedules, uow);
        }

        private void InsertSeasonRelatedStatistics(IEnumerable<Team> teams, SeasonAndCompetitionSchedules seasonAndCompetitionSchedules, IUnitOfWork uow)
        {
            string league1LeagueTableId = seasonAndCompetitionSchedules.LeaguesSchedule.LeagueTables[0].Id;
            string league2LeagueTableId = seasonAndCompetitionSchedules.LeaguesSchedule.LeagueTables[1].Id;
            string league3LeagueTableId = seasonAndCompetitionSchedules.LeaguesSchedule.LeagueTables[2].Id;
            string league4LeagueTableId = seasonAndCompetitionSchedules.LeaguesSchedule.LeagueTables[3].Id;

            // SeasonStatistics.
            var seasonStatistics = new SeasonStatistics(seasonAndCompetitionSchedules.Season, league1LeagueTableId, league2LeagueTableId, league3LeagueTableId, league4LeagueTableId);
            uow.SeasonStatics.Add(seasonStatistics);

            // SeasonTeamStatistics.
            foreach (var team in teams)
            {
                var seasonTeamStatistic = new SeasonTeamStatistics(seasonAndCompetitionSchedules.Season, team, team.CurrentLeagueCompetition.Name);
                uow.SeasonTeamStatistics.Add(seasonTeamStatistic);
            }
        }

        private void InsertTeamStatistics(List<Team> teams, IUnitOfWork uow)
        {
            foreach (var team in teams)
            {
                var teamStatistics = new TeamStatistics(team);
                uow.TeamStatistics.Add(teamStatistics);
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

        private void InsertSeasonAndCompetitionSchedule(SeasonAndCompetitionSchedules seasonAndCompetitionSchedules, IUnitOfWork uow)
        {
            // Insert the season.
            uow.Seasons.Add(seasonAndCompetitionSchedules.Season);

            // Insert league schedules.
            SaveCompetitionSchedule(seasonAndCompetitionSchedules.LeaguesSchedule, uow);

            // Insert cup schedule.
            SaveCompetitionSchedule(seasonAndCompetitionSchedules.NationalCupSchedule, uow);

            // Insert super cup schedule.
            SaveCompetitionSchedule(seasonAndCompetitionSchedules.NationalSuperCupSchedule, uow);

            // Insert pre-season friendly schedule.
            SaveCompetitionSchedule(seasonAndCompetitionSchedules.PreSeasonFriendliesSchedule, uow);

            // Insert during season friendlies.
            SaveCompetitionSchedule(seasonAndCompetitionSchedules.DuringSeasonFriendliesSchedule, uow);

            // Update the teams.
            uow.Teams.Update(seasonAndCompetitionSchedules.Teams);
        }

        private SeasonAndCompetitionSchedules CreateSeasonAndCompetitionSchedules(NewSeasonInfo newSeasonInfo)
        {
            var matchDateManager = new MatchDateManager(newSeasonInfo.StartYear);
            matchDateManager.Initialize();

            // The season officially ends a day after the last match so there is a possibility to add events between the last match and the end of the season.
            var endSeasonDateTime = matchDateManager.GetAllMatchDates().OrderBy(d => d).Last().AddDays(1);

            var season = new Season
            {
                StartYear = newSeasonInfo.StartYear,
                SeasonStatus = SeasonStatus.Started,
                EndDateTime = endSeasonDateTime
            };

            var seasonAndCompetitionSchedules = new SeasonAndCompetitionSchedules { Season = season };

            // Create leagues and schedule.
            seasonAndCompetitionSchedules.LeaguesSchedule = _leagueManager.CreateSchedules(newSeasonInfo.TeamsLeague1, newSeasonInfo.TeamsLeague2, newSeasonInfo.TeamsLeague3, newSeasonInfo.TeamsLeague4, season, matchDateManager);

            // Create a national cup tournament.
            seasonAndCompetitionSchedules.NationalCupSchedule = _nationalCupManager.CreateSchedule(newSeasonInfo.AllTeams.ToList(), season, matchDateManager);

            // Create pre-season friendlies.
            seasonAndCompetitionSchedules.PreSeasonFriendliesSchedule = _preSeasonFriendlyManager.CreatePreSeasonSchedule(newSeasonInfo.AllTeams.ToList(), season, matchDateManager);

            // Create friendlies during the season.
            // Determine on which dates these friendlies can be played. For now, this is only during the national cup tournament, except the first round and the final.
            var cupDates = seasonAndCompetitionSchedules.NationalCupSchedule.Rounds.Select(r => r.MatchDate).Skip(1).Take(seasonAndCompetitionSchedules.NationalCupSchedule.Rounds.Count - 2).ToList();
            seasonAndCompetitionSchedules.DuringSeasonFriendliesSchedule = _duringSeasonFriendlyRoundsManager.CreateDuringSeasonFriendlyRounds(seasonAndCompetitionSchedules.PreSeasonFriendliesSchedule.SeasonCompetitions.First(), cupDates, Constants.HowManyPreSeasonFriendlies + 1);

            // Create Super Cup.
            // The home team is always the national champion and the away team is either the cup winner or the league 1 runner up if the champion also won the cup.
            var homeTeam = newSeasonInfo.PreviousSeasonStatistics.NationalChampion;
            var awayTeam = newSeasonInfo.PreviousSeasonStatistics.NationalChampionTeamId == newSeasonInfo.PreviousSeasonStatistics.CupWinnerTeamId ? newSeasonInfo.PreviousSeasonStatistics.NationalChampionRunnerUp : newSeasonInfo.PreviousSeasonStatistics.CupWinner;
            seasonAndCompetitionSchedules.NationalSuperCupSchedule = _nationalSuperCupManager.CreateSchedule(homeTeam, awayTeam, season, matchDateManager);

            // In the meantime data of the teams has changed, so add them to the SeasonAndCompetitionSchedules object so they can be updated in the database.
            seasonAndCompetitionSchedules.Teams = newSeasonInfo.AllTeams;

            return seasonAndCompetitionSchedules;
        }

        public void EndSeason(Season season, IUnitOfWork uow)
        {
            // Get all league tables and update the team statistics.
            var teamStatistics = uow.TeamStatistics.GetAll().ToDictionary(k => k.TeamId, v => v);
            var teamStatisticsManager = new TeamStatisticsManager(teamStatistics);

            var leagueTables = uow.LeagueTables.GetBySeason(season.Id);
            var leagues = uow.Competitions.GetLeagues().ToDictionary(k => k.Id, v => v);
            teamStatisticsManager.Update(leagueTables, leagues);

            // End the season by updating the status.
            season.SeasonStatus = SeasonStatus.Ended;
            uow.Seasons.Update(season);

            // Mark the end of season date as completed.
            _gameDateTimeMutationManager.UpdateEndOfSeasonStatus(season.EndDateTime);
        }

        private void SaveCompetitionSchedule(CompetitionSchedule competitionSchedule, IUnitOfWork uow)
        {
            uow.LeagueTables.Add(competitionSchedule.LeagueTables);
            foreach (var leagueTable in competitionSchedule.LeagueTables)
            {
                uow.LeagueTablePositions.Add(leagueTable.LeagueTablePositions);
            }

            uow.SeasonCompetitions.Add(competitionSchedule.SeasonCompetitions);
            uow.SeasonCompetitionTeams.Add(competitionSchedule.SeasonCompetitionTeams);
            uow.Rounds.Add(competitionSchedule.Rounds);
            uow.Matches.Add(competitionSchedule.Matches);
        }

        private void InsertGameDateTimes(SeasonAndCompetitionSchedules schedules, IUnitOfWork uow)
        {
            // Determine the match dates the manager's team plays.
            var managersTeam = new Team(); //TODO hier via de repo managers team ophalen
            var managersMatchDates = schedules.AllMatches.Where(m => m.TeamPlaysMatch(managersTeam)).Select(m => m.Date);

            // Determine when the manager's team does not play: all dates in the season except the ones that were just determined.
            var otherTeamsMatchDates = schedules.AllMatchDates.Except(managersMatchDates);

            _gameDateTimeMutationManager.CreateNewForSeason(managersMatchDates, otherTeamsMatchDates, schedules.Season.EndDateTime);
        }
    }
}
