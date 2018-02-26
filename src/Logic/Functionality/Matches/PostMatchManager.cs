using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Data.Database;
using TwoNil.Logic.Functionality.Competitions;
using TwoNil.Logic.Functionality.Teams;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Matches
{
   /// <summary>
   /// Responsible for handling everything after matches has been played.
   /// </summary>
   internal class PostMatchManager
   {
      private readonly TransactionManager _repository;
      private readonly SeasonManager _seasonManager;
      private DatabaseRepositoryFactory _repositoryFactory;

      public PostMatchManager(TransactionManager repository, DatabaseRepositoryFactory repositoryFactory, SeasonManager seasonManager)
      {
         _repository = repository;
         _seasonManager = seasonManager;
         _repositoryFactory = repositoryFactory;
      }

      public void Handle(string seasonId, IEnumerable<Match> matches)
      {
         var leagueTableManager = new LeagueTableManager();

         var rounds = matches.Select(c => c.Round).Distinct().ToList();

         var seasonStatisticsManager = new SeasonStatisticsManager(_repository, _repositoryFactory);
         var seasonTeamStatisticsManager = new SeasonTeamStatisticsManager(_repository, _repositoryFactory, seasonId);

         Season season;
         using (var seasonRepository = _repositoryFactory.CreateSeasonRepository())
         {
            season = seasonRepository.GetOne(seasonId);
         }

         foreach (var round in rounds)
         {
            // Handle league related stuff.
            if (round.CompetitionType == CompetitionType.League)
            {
               var matchesForThisRound = matches.Where(m => m.RoundId == round.Id).ToList();

               LeagueTable leagueTable;
               using (var leagueTableRepository = _repositoryFactory.CreateLeagueTableRepository())
               {
                  leagueTable = leagueTableRepository.GetBySeasonCompetition(new[] { round.SeasonCompetitionId }).Single();
               }

               // Update the league table and current league table position of the team.
               leagueTableManager.UpdateLeagueTable(leagueTable, matchesForThisRound);
               _repository.RegisterUpdate(leagueTable);
               _repository.RegisterUpdate(leagueTable.LeagueTablePositions);
               var teams = leagueTable.LeagueTablePositions.Select(x => x.Team).ToList();
               _repository.RegisterUpdate(teams);

               // Update the team statistics for this season.
               seasonTeamStatisticsManager.Update(seasonId, matchesForThisRound, leagueTable);

               // If all league matches have been played, update the season statistics.
               using (var competitionRepository = new MemoryRepositoryFactory().CreateCompetitionRepository())
               using (var matchRepository = _repositoryFactory.CreateMatchRepository())
               {
                  if (round.CompetitionId == competitionRepository.GetLeague1().Id)
                  {
                     int numberOfLeagueMatches = (Constants.HowManyTeamsPerLeague - 1) * 2;
                     bool leagueFinished = leagueTable.LeagueTablePositions.All(x => x.Matches == numberOfLeagueMatches);
                     if (leagueFinished)
                     {
                        seasonStatisticsManager.UpdateNationalChampion(seasonId, leagueTable.LeagueTablePositions[0].Team, leagueTable.LeagueTablePositions[1].Team);
                     }
                  }
               }
            }

            // Handle National Cup related stuff.
            else if (round.CompetitionType == CompetitionType.NationalCup)
            {
               // If the round is a cup round: draw the next round.
               var nationalCupManager = new NationalCupManager(_repositoryFactory);
               var cupMatches = matches.Where(m => m.RoundId == round.Id);
               var matchesNextRound = nationalCupManager.DrawNextRound(round, cupMatches, season);
               _repository.RegisterInsert(matchesNextRound);

               // If the final has been played: update the winner to the season statistics.
               if (round.Name == Round.Final)
               {
                  var match = matches.Where(m => m.RoundId == round.Id).Single();
                  var winner = match.GetWinner();
                  var runnerUp = match.HomeTeam.Equals(winner) ? match.AwayTeam : match.HomeTeam;
                  seasonStatisticsManager.UpdateNationalCupWinner(seasonId, winner, runnerUp);
               }
            }

            // If the round is a National Super Cup: save the winner to the season statistics.
            else if (round.CompetitionType == CompetitionType.NationalSuperCup)
            {
               var match = matches.Where(m => m.RoundId == round.Id).Single();
               var winner = match.GetWinner();

               seasonStatisticsManager.UpdateNationalSuperCupWinner(seasonId, winner);
            }
         }

         // In the meantime there may be rounds that have matches where only a few teams participate, typically a cup round that was just drawn.
         CreateDuringSeasonFriendlies(seasonId);
      }

      private void CreateDuringSeasonFriendlies(string seasonId)
      {
         var rounds = new List<Round>();
         IEnumerable<Team> teams = new List<Team>();

         // Get rounds and matches from the database.
         using (var roundRepository = _repositoryFactory.CreateRoundRepository())
         using (var matchRepository = _repositoryFactory.CreateMatchRepository())
         using (var teamRepository = _repositoryFactory.CreateTeamRepository())
         {
            rounds = roundRepository.GetBySeason(seasonId).ToList();
            var matchesFromDatabase = matchRepository.GetBySeason(seasonId);

            foreach (var round in rounds)
            {
               round.Matches = matchesFromDatabase.Where(m => m.RoundId == round.Id).ToList();
            }

            teams = teamRepository.GetAll();
         }

         // Add the matches that will be saved (i.e. inserted) to the database.
         var transactions = _repository.GetTransactions().Where(x => x.DomainObject is Match);
         foreach (var transaction in transactions)
         {
            Match match = transaction.DomainObject as Match;
            var round = rounds.Single(r => r.Id == match.RoundId);

            if (!round.Matches.Contains(match))
            {
               round.Matches.Add(match);
            }
         }

         var friendlyManager = new FriendlyManager(_repositoryFactory);
         var matches = friendlyManager.CreateMatchesForFriendlyRound(rounds, teams);

         if (matches.Any())
         {
            _repository.RegisterInsert(matches);
         }
      }
   }
}