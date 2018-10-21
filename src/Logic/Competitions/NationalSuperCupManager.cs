using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Competitions
{
    public class NationalSuperCupManager
    {
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly SingleRoundTournamentManager _singleRoundTournamentManager;

        public NationalSuperCupManager(IUnitOfWorkFactory uowFactory, SingleRoundTournamentManager singleRoundTournamentManager)
        {
            _uowFactory = uowFactory;
            _singleRoundTournamentManager = singleRoundTournamentManager;
        }

        public CompetitionSchedule CreateSchedule(Team team1, Team team2, Season season, MatchDateManager matchDateManager)
        {
            var competitionSchedule = new CompetitionSchedule();

            Competition superCupCompetition;
            using (var uow = _uowFactory.Create())
            {
                // Create a super cup season competition and round and save it to the database.
                superCupCompetition = uow.Competitions.GetNationalSuperCup();
            }

            var superCupSeasonCompetition = new SeasonCompetition
            {
                Competition = superCupCompetition,
                Season = season
            };
            competitionSchedule.SeasonCompetitions.Add(superCupSeasonCompetition);

            const int roundNr = 0;

            var matchDate = matchDateManager.GetNextMatchDate(CompetitionType.NationalSuperCup, roundNr);

            var superCupRound = RoundFactory.CreateRound("Final", superCupSeasonCompetition, matchDate, roundNr, superCupCompetition);
            competitionSchedule.Rounds.Add(superCupRound);

            // Create the super cup match and save it to the database.
            var teams1 = new List<Team> { team1 };
            var teams2 = new List<Team> { team2 };
            var match = _singleRoundTournamentManager.GetMatches(teams1, teams2).Single();

            match.Season = season;
            match.Round = superCupRound;
            match.Date = matchDate;
            match.DrawPermitted = false;
            match.CompetitionId = superCupCompetition.Id;
            competitionSchedule.Matches.Add(match);

            // Add both teams to the super cup competition of this season.
            var seasonCompetitionTeam1 = new SeasonCompetitionTeam
            {
                Team = team1,
                SeasonCompetition = superCupSeasonCompetition
            };
            var seasonCompetitionTeam2 = new SeasonCompetitionTeam
            {
                Team = team2,
                SeasonCompetition = superCupSeasonCompetition
            };
            competitionSchedule.SeasonCompetitionTeams.Add(seasonCompetitionTeam1);
            competitionSchedule.SeasonCompetitionTeams.Add(seasonCompetitionTeam2);

            return competitionSchedule;
        }
    }
}
