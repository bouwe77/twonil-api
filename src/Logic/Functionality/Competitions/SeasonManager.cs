using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Data.Database;
using TwoNil.Logic.Exceptions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions
{
   internal class SeasonManager
   {
      private readonly LeagueManager _leagueManager;
      private readonly DatabaseRepositoryFactory _repositoryFactory;

      public SeasonManager(DatabaseRepositoryFactory repositoryFactory)
      {
         _repositoryFactory = repositoryFactory;
         _leagueManager = new LeagueManager();
      }

      public void CreateFirstSeason(List<Team> teams, TransactionManager repository)
      {
         // First check the number of teams can be evenly divided into the number of leagues.
         bool teamsOk = teams.Count % Constants.HowManyLeagues == 0;
         if (!teamsOk)
         {
            throw new Exception($"The number of teams must be divided by {Constants.HowManyLeagues}");
         }

         var newSeasonInfo = new NewSeasonInfo { SeasonNumber = 0 };

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
         var season = InsertSeasonAndCompetitionSchedule(repository, seasonAndCompetitionSchedules);

         // Insert statistics.
         InsertStatistics(_repositoryFactory, repository, teams, seasonAndCompetitionSchedules);
      }

      public void CreateNextSeason(Season previousSeason, TransactionManager repository)
      {
         if (previousSeason.SeasonStatus != SeasonStatus.Ended)
         {
            throw new ConflictException("Season must be ended before a new one can be created");
         }

         var newSeasonInfo = new NewSeasonInfo
         {
            SeasonNumber = previousSeason.GameOrder + 1
         };

         // Determine which teams promote and relegate.
         IEnumerable<Team> allTeamsSortedOnLeagueAndPosition;
         using (var teamRepository = _repositoryFactory.CreateTeamRepository())
         {
            allTeamsSortedOnLeagueAndPosition = teamRepository.GetTeams().OrderBy(x => x.CurrentLeagueCompetition.Order).ThenBy(x => x.CurrentLeaguePosition);
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
         InsertSeasonAndCompetitionSchedule(repository, seasonAndCompetitionSchedules);

         // Insert statistics.
         InsertStatistics(_repositoryFactory, repository, allTeamsSortedOnLeagueAndPosition, seasonAndCompetitionSchedules);
      }

      private void InsertStatistics(DatabaseRepositoryFactory repositoryFactory, TransactionManager transactionManager, IEnumerable<Team> teams, SeasonAndCompetitionSchedules seasonAndCompetitionSchedules)
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

      private Season InsertSeasonAndCompetitionSchedule(TransactionManager repository, SeasonAndCompetitionSchedules seasonAndCompetitionSchedules)
      {
         // Insert the season.
         repository.RegisterInsert(seasonAndCompetitionSchedules.Season);

         // Insert league schedules.
         SaveCompetitionSchedule(seasonAndCompetitionSchedules.LeaguesSchedule, repository);

         // Insert cup schedule.
         SaveCompetitionSchedule(seasonAndCompetitionSchedules.NationalCupSchedule, repository);

         // Insert super cup schedule.
         SaveCompetitionSchedule(seasonAndCompetitionSchedules.NationalSuperCupSchedule, repository);

         // Insert pre-season friendly schedule.
         SaveCompetitionSchedule(seasonAndCompetitionSchedules.PreSeasonFriendliesSchedule, repository);

         // Insert during season friendlies.
         SaveCompetitionSchedule(seasonAndCompetitionSchedules.DuringSeasonFriendliesSchedule, repository);

         // Update the teams.
         repository.RegisterUpdate(seasonAndCompetitionSchedules.Teams);

         return seasonAndCompetitionSchedules.Season;
      }

      private SeasonAndCompetitionSchedules CreateSeasonAndCompetitionSchedules(NewSeasonInfo newSeasonInfo)
      {
         var seasonAndCompetitionSchedules = new SeasonAndCompetitionSchedules();

         // Create a new season.
         string seasonName = $"Season {newSeasonInfo.SeasonNumber + 1}";
         var season = new Season
         {
            Name = seasonName,
            GameOrder = newSeasonInfo.SeasonNumber,
            SeasonStatus = SeasonStatus.Started
         };

         seasonAndCompetitionSchedules.Season = season;

         var matchDateManager = new MatchDateManager(newSeasonInfo.SeasonNumber);
         matchDateManager.Initialize();

         // Create leagues and schedule.
         var leagueManager = new LeagueManager();
         seasonAndCompetitionSchedules.LeaguesSchedule = leagueManager.CreateSchedules(newSeasonInfo.TeamsLeague1, newSeasonInfo.TeamsLeague2, newSeasonInfo.TeamsLeague3, newSeasonInfo.TeamsLeague4, season, matchDateManager);

         // Create a national cup tournament.
         var nationalCupManager = new NationalCupManager(_repositoryFactory);
         seasonAndCompetitionSchedules.NationalCupSchedule = nationalCupManager.CreateSchedule(newSeasonInfo.AllTeams.ToList(), season, matchDateManager);

         // Create pre-season friendlies.
         var friendlyManager = new FriendlyManager(_repositoryFactory);
         seasonAndCompetitionSchedules.PreSeasonFriendliesSchedule = friendlyManager.CreatePreSeasonSchedule(newSeasonInfo.AllTeams.ToList(), Constants.HowManyPreSeasonFriendlies, season, matchDateManager);

         // Create friendlies during the season.
         // Determine on which dates these friendlies can be played. For now, this is only during the national cup tournament, except the first round and the final.
         var cupDates = seasonAndCompetitionSchedules.NationalCupSchedule.Rounds.Select(r => r.MatchDate).Skip(1).Take(seasonAndCompetitionSchedules.NationalCupSchedule.Rounds.Count - 2).ToList();
         seasonAndCompetitionSchedules.DuringSeasonFriendliesSchedule = friendlyManager.CreateDuringSeasonSchedule(seasonAndCompetitionSchedules.PreSeasonFriendliesSchedule.SeasonCompetitions.First(), cupDates, Constants.HowManyPreSeasonFriendlies + 1);

         // Create Super Cup.
         var nationalSuperCupManager = new NationalSuperCupManager();
         // The home team is always the national champion and the away team is either the cup winner or the league 1 runner up if the champion also won the cup.
         var homeTeam = newSeasonInfo.PreviousSeasonStatistics.NationalChampion;
         var awayTeam = newSeasonInfo.PreviousSeasonStatistics.NationalChampion.Equals(newSeasonInfo.PreviousSeasonStatistics.CupWinner) ? newSeasonInfo.PreviousSeasonStatistics.NationalChampionRunnerUp : newSeasonInfo.PreviousSeasonStatistics.CupWinner;
         seasonAndCompetitionSchedules.NationalSuperCupSchedule = nationalSuperCupManager.CreateSchedule(homeTeam, awayTeam, season, matchDateManager);

         // In the mean time data of the teams has changed, so add them to the SeasonAndCompetitionSchedules object so they can be updated in the database.
         seasonAndCompetitionSchedules.Teams = newSeasonInfo.AllTeams;

         return seasonAndCompetitionSchedules;
      }

      public void EndSeason(Season season, TransactionManager repository)
      {
         // End the season by updating the status.
         season.SeasonStatus = SeasonStatus.Ended;
         repository.RegisterUpdate(season);
      }

      private void SaveCompetitionSchedule(CompetitionSchedule competitionSchedule, TransactionManager repository)
      {
         repository.RegisterInsert(competitionSchedule.LeagueTables);
         foreach (var leagueTable in competitionSchedule.LeagueTables)
         {
            repository.RegisterInsert(leagueTable.LeagueTablePositions);
         }

         repository.RegisterInsert(competitionSchedule.SeasonCompetitions);

         repository.RegisterInsert(competitionSchedule.SeasonCompetitionTeams);

         repository.RegisterInsert(competitionSchedule.Rounds);

         repository.RegisterInsert(competitionSchedule.Matches);
      }
   }
}
