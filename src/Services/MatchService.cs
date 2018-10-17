using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Exceptions;
using TwoNil.Shared.DomainObjects;
using TwoNil.Logic.Matches.PostMatches;
using TwoNil.Logic.Matches.MatchPlay;

namespace TwoNil.Services
{
    public interface IMatchService
    {
        IEnumerable<Match> GetByMatchDay(DateTime matchDay);
        Match GetByMatchDayAndTeam(DateTime matchDay, string teamId);
        IEnumerable<Match> GetByRound(Round round);
        IEnumerable<Match> GetBySeasonAndTeam(string seasonId, string teamId);
        Match GetMatch(string matchId);
        DateTime? GetNextMatchDate(string seasonId);
        IEnumerable<TeamRoundMatch> GetTeamRoundMatches(string teamId, string seasonId, string leagueCompetitionId);
        void PlayMatchDay(DateTime matchDate);
        void PlayMatchDay(DateTime matchDate, IUnitOfWork uow);
    }

    public class MatchService : ServiceWithGameBase, IMatchService
    {
        private ISeasonService _seasonService;
        private readonly IPostMatchOrchestrator _postMatchOrchestrator;

        internal MatchService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo, ISeasonService seasonService, IPostMatchOrchestrator postMatchOrchestrator)
           : base(uowFactory, gameInfo)
        {
            _seasonService = seasonService;
            _postMatchOrchestrator = postMatchOrchestrator;
        }

        public Match GetMatch(string matchId)
        {
            using (var uow = UowFactory.Create())
            {
                // Get the match from the database.
                var match = uow.Matches.GetMatch(matchId);
                return match;
            }
        }

        public IEnumerable<Match> GetBySeasonAndTeam(string seasonId, string teamId)
        {
            using (var uow = UowFactory.Create())
            {
                var matches = uow.Matches.GetBySeasonAndTeam(seasonId, teamId).OrderBy(match => match.Date);
                return matches;
            }
        }

        public IEnumerable<Match> GetByRound(Round round)
        {
            using (var uow = UowFactory.Create())
            {
                return uow.Matches.GetByRound(round.Id);
            }
        }

        public DateTime? GetNextMatchDate(string seasonId)
        {
            using (var uow = UowFactory.Create())
            {
                return uow.Matches.GetNextMatchDate(seasonId);
            }
        }

        public IEnumerable<Match> GetByMatchDay(DateTime matchDay)
        {
            using (var uow = UowFactory.Create())
            {
                var matches = uow.Matches.GetByMatchDay(matchDay);
                return matches;
            }
        }

        public Match GetByMatchDayAndTeam(DateTime matchDay, string teamId)
        {
            using (var uow = UowFactory.Create())
            {
                var match = uow.Matches.GetByMatchDayAndTeam(matchDay, teamId);
                return match;
            }
        }

        public void PlayMatchDay(DateTime matchDate)
        {
            using (var uow = UowFactory.Create())
            {
                //TODO Deze UOW moet een transactie krijgen
                PlayMatchDay(matchDate, uow);
            }
        }

        public void PlayMatchDay(DateTime matchDate, IUnitOfWork uow)
        {
            // First check if the given matchDay is the "next" match day in this season.
            var currentSeason = _seasonService.GetCurrentSeason();
            var nextMatchDate = GetNextMatchDate(currentSeason.Id);
            if (!nextMatchDate.Equals(matchDate))
            {
                throw new ConflictException("This date is not the next match date");
            }

            var matchesToPlay = GetByMatchDay(matchDate).ToList();
            if (!matchesToPlay.Any() || matchesToPlay.All(m => m.MatchStatus == MatchStatus.Ended))
            {
                throw new NotFoundException("There are no matches on this date");
            }

            foreach (var match in matchesToPlay.Where(m => m.MatchStatus == MatchStatus.NotStarted))
            {
                new MatchPlayer().Play(match);
            }

            uow.Matches.Update(matchesToPlay);

            // After matches have been played a lot of stuff must be determined and updated.
            _postMatchOrchestrator.Handle(matchesToPlay);
        }

        public IEnumerable<TeamRoundMatch> GetTeamRoundMatches(string teamId, string seasonId, string leagueCompetitionId)
        {
            using (var uow = UowFactory.Create())
            {
                var matches = uow.Matches.GetTeamRoundMatches(GameInfo.Id, teamId, seasonId);

                // For league competitions only the team's current league must be included.
                var leagueCompetitionsToSkip = uow.Competitions.GetLeagues().Where(c => c.Id != leagueCompetitionId).Select(c => c.Id);
                return matches.Where(m => !leagueCompetitionsToSkip.Contains(m.CompetitionId));
            }
        }
    }
}
