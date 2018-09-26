using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Functionality.Calendar;
using TwoNil.Logic.Functionality.Competitions;
using TwoNil.Logic.Functionality.Competitions.Friendlies;
using TwoNil.Logic.Functionality.Teams;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Matches
{
    /// <summary>
    /// Responsible for handling everything after matches has been played.
    /// </summary>
    internal class PostMatchManager
    {
        private readonly TransactionManager _transactionManager;
        private readonly IRepositoryFactory _repositoryFactory;

        public PostMatchManager(TransactionManager transactionManager, IRepositoryFactory repositoryFactory)
        {
            _transactionManager = transactionManager;
            _repositoryFactory = repositoryFactory;
        }

        public void Handle(string seasonId, List<Match> matches)
        {
            var rounds = matches.Select(c => c.Round).Distinct().ToList();

            Season season;
            using (var seasonRepository = _repositoryFactory.CreateSeasonRepository())
            {
                season = seasonRepository.GetOne(seasonId);
            }

            var seasonStatisticsManager = new SeasonStatisticsManager(_transactionManager, _repositoryFactory);
            var seasonTeamStatisticsManager = new SeasonTeamStatisticsManager(_transactionManager, _repositoryFactory, seasonId);
            var leagueTableManager = new LeagueTableManager(_repositoryFactory);
            var gameDateTimeManager = new GameDateTimeMutationManager(_transactionManager, _repositoryFactory);

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
                    _transactionManager.RegisterUpdate(leagueTable);
                    _transactionManager.RegisterUpdate(leagueTable.LeagueTablePositions);
                    var teams = leagueTable.LeagueTablePositions.Select(x => x.Team).ToList();
                    _transactionManager.RegisterUpdate(teams);

                    // Update the team statistics for this season.
                    seasonTeamStatisticsManager.Update(seasonId, matchesForThisRound, leagueTable);

                    // If all league matches have been played, update the season statistics.
                    using (var competitionRepository = new RepositoryFactory().CreateCompetitionRepository())
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
                    Team managersTeam;
                    using (var gameInfoRepository = _repositoryFactory.CreateGameInfoRepository())
                    {
                        managersTeam = gameInfoRepository.GetGameInfo().CurrentTeam;
                    }

                    // If the round is a cup round: draw the next round.
                    var nationalCupManager = new NationalCupManager(_repositoryFactory);
                    var cupMatches = matches.Where(m => m.RoundId == round.Id);
                    var matchesNextRound = nationalCupManager.DrawNextRound(round, cupMatches, season);
                    _transactionManager.RegisterInsert(matchesNextRound);

                    if (matchesNextRound.Any(m => m.TeamPlaysMatch(managersTeam)))
                        gameDateTimeManager.UpdateManagerPlaysMatch(matchesNextRound.Select(m => m.Date).First());

                    // If the final has been played: update the winner to the season statistics.
                    if (round.Name == Round.Final)
                    {
                        var match = matches.Single(m => m.RoundId == round.Id);
                        var winner = match.GetWinner();
                        var runnerUp = match.HomeTeam.Equals(winner) ? match.AwayTeam : match.HomeTeam;
                        seasonStatisticsManager.UpdateNationalCupWinner(seasonId, winner, runnerUp);
                    }
                    else
                    {
                        // During the next cup round there also might be a friendly round. If so, generate friendly matches.
                        var teamsInNextCupRound = matchesNextRound.Select(m => m.HomeTeam).ToList();
                        teamsInNextCupRound.AddRange(matchesNextRound.Select(m => m.AwayTeam));

                        var friendlyManager = new DuringSeasonFriendlyManager(_repositoryFactory);
                        var duringSeasonFriendlies = friendlyManager.CreateDuringSeasonFriendlies(round, teamsInNextCupRound);
                        _transactionManager.RegisterInsert(duringSeasonFriendlies);

                        if (duringSeasonFriendlies.Any(m => m.TeamPlaysMatch(managersTeam)))
                            gameDateTimeManager.UpdateManagerPlaysMatch(matchesNextRound.Select(m => m.Date).First());
                    }
                }

                // Handle National Super Cup related stuff.
                else if (round.CompetitionType == CompetitionType.NationalSuperCup)
                {
                    var match = matches.Single(m => m.RoundId == round.Id);
                    var winner = match.GetWinner();

                    seasonStatisticsManager.UpdateNationalSuperCupWinner(seasonId, winner);
                }
            }

            // Update match status in the calendar.
            var matchDateTime = rounds.Select(x => x.MatchDate).First();
            gameDateTimeManager.UpdateMatchStatus(matchDateTime);
        }
    }
}