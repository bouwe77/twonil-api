using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Competitions
{
    public class LeagueManager
    {
        private readonly IUnitOfWorkFactory _uowFactory;

        public LeagueManager(IUnitOfWorkFactory uowFactory)
        {
            _uowFactory = uowFactory;
        }

        public CompetitionSchedule CreateSchedules(List<Team> teamsLeague1, List<Team> teamsLeague2, List<Team> teamsLeague3, List<Team> teamsLeague4, Season season, MatchDateManager matchDateManager)
        {
            var competitionSchedule = new CompetitionSchedule();

            using (var uow = _uowFactory.Create())
            {
                // Create the leagues with the teams.
                CreateLeague(competitionSchedule, uow.Competitions.GetLeague1(), teamsLeague1, season, matchDateManager);
                CreateLeague(competitionSchedule, uow.Competitions.GetLeague2(), teamsLeague2, season, matchDateManager);
                CreateLeague(competitionSchedule, uow.Competitions.GetLeague3(), teamsLeague3, season, matchDateManager);
                CreateLeague(competitionSchedule, uow.Competitions.GetLeague4(), teamsLeague4, season, matchDateManager);
            }

            return competitionSchedule;
        }

        public List<List<Team>> PromoteAndRelegateTeams(List<List<Team>> leagues, int howManyTeamsPromoteAndRelegate)
        {
            if (leagues.Count != 4)
            {
                throw new NotImplementedException("At this moment only 4 leagues are supported");
            }

            var newLeague1 = new List<Team>();
            newLeague1.AddRange(leagues[0].Take(leagues[0].Count - howManyTeamsPromoteAndRelegate));
            newLeague1.AddRange(leagues[1].Take(howManyTeamsPromoteAndRelegate));

            var newLeague2 = new List<Team>();
            newLeague2.AddRange(leagues[0].Skip(leagues[0].Count - howManyTeamsPromoteAndRelegate).Take(howManyTeamsPromoteAndRelegate));
            newLeague2.AddRange(leagues[1].Skip(howManyTeamsPromoteAndRelegate).Take(leagues[1].Count - (howManyTeamsPromoteAndRelegate * 2)));
            newLeague2.AddRange(leagues[2].Take(howManyTeamsPromoteAndRelegate));

            var newLeague3 = new List<Team>();
            newLeague3.AddRange(leagues[1].Skip(leagues[1].Count - howManyTeamsPromoteAndRelegate).Take(howManyTeamsPromoteAndRelegate));
            newLeague3.AddRange(leagues[2].Skip(howManyTeamsPromoteAndRelegate).Take(leagues[2].Count - (howManyTeamsPromoteAndRelegate * 2)));
            newLeague3.AddRange(leagues[3].Take(howManyTeamsPromoteAndRelegate));

            var newLeague4 = new List<Team>();
            newLeague4.AddRange(leagues[2].Skip(leagues[2].Count - howManyTeamsPromoteAndRelegate).Take(howManyTeamsPromoteAndRelegate));
            newLeague4.AddRange(leagues[3].Skip(howManyTeamsPromoteAndRelegate).Take(leagues[3].Count - howManyTeamsPromoteAndRelegate));

            return new List<List<Team>>
            {
                newLeague1,
                newLeague2,
                newLeague3,
                newLeague4
            };
        }

        private void CreateLeague(CompetitionSchedule competitionSchedule, Competition leagueCompetition, List<Team> teams, Season season, MatchDateManager matchDateManager)
        {
            // Create a competition for the League and save it to the database.
            var leagueSeasonCompetition = new SeasonCompetition
            {
                Competition = leagueCompetition,
                Season = season
            };

            competitionSchedule.SeasonCompetitions.Add(leagueSeasonCompetition);

            // Add the teams to the league.
            foreach (var team in teams)
            {
                var seasonCompetitionTeam = new SeasonCompetitionTeam
                {
                    SeasonCompetition = leagueSeasonCompetition,
                    Team = team
                };
                competitionSchedule.SeasonCompetitionTeams.Add(seasonCompetitionTeam);

                // Update current league for the team.
                team.CurrentLeagueCompetition = leagueCompetition;
            }

            // Create a match schedule.
            var roundRobinTournamentManager = new RoundRobinTournamentManager();
            var matchSchedule = roundRobinTournamentManager.GetSchedule(teams);
            foreach (var round in matchSchedule)
            {
                var matchDate = matchDateManager.GetNextMatchDate(CompetitionType.League, round.Key);
                var leagueRound = RoundFactory.CreateRound($"Round {round.Key + 1}", leagueSeasonCompetition, matchDate, round.Key, leagueCompetition);
                competitionSchedule.Rounds.Add(leagueRound);

                foreach (var match in round.Value)
                {
                    match.Season = season;
                    match.Round = leagueRound;
                    match.Date = matchDate;
                    match.CompetitionId = leagueCompetition.Id;
                    competitionSchedule.Matches.Add(match);
                }
            }

            // Create a league table.
            var leagueTable = new LeagueTable
            {
                CompetitionName = leagueCompetition.Name,
                SeasonCompetition = leagueSeasonCompetition,
                SeasonId = leagueSeasonCompetition.SeasonId
            };

            leagueTable.LeagueTablePositions = new List<LeagueTablePosition>();
            int position = 1;
            foreach (var team in teams)
            {
                team.CurrentLeaguePosition = position;
                leagueTable.LeagueTablePositions.Add(new LeagueTablePosition { Team = team, LeagueTable = leagueTable, Position = position });
                position++;
            }

            competitionSchedule.LeagueTables.Add(leagueTable);
        }
    }
}
