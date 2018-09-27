﻿using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Exceptions;
using TwoNil.Logic.Competitions;
using TwoNil.Logic.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
    public class MatchService : ServiceWithGameBase
    {
        private SeasonService _seasonService;

        internal MatchService(GameInfo gameInfo)
           : base(gameInfo)
        {
            _seasonService = new ServiceFactory().CreateSeasonService(gameInfo);
        }

        public Match GetMatch(string matchId)
        {
            using (var matchRepository = RepositoryFactory.CreateMatchRepository())
            {
                // Get the match from the database.
                var match = matchRepository.GetMatch(matchId);
                return match;
            }
        }

        public IEnumerable<Match> GetBySeasonAndTeam(string seasonId, string teamId)
        {
            using (var matchRepository = RepositoryFactory.CreateMatchRepository())
            {
                var matches = matchRepository.GetBySeasonAndTeam(seasonId, teamId).OrderBy(match => match.Date);
                return matches;
            }
        }

        public IEnumerable<Match> GetByRound(Round round)
        {
            using (var matchRepository = RepositoryFactory.CreateMatchRepository())
            {
                return matchRepository.GetByRound(round.Id);
            }
        }

        public DateTime? GetNextMatchDate(string seasonId)
        {
            using (var matchRepository = RepositoryFactory.CreateMatchRepository())
            {
                return matchRepository.GetNextMatchDate(seasonId);
            }
        }

        public IEnumerable<Match> GetByMatchDay(DateTime matchDay)
        {
            using (var matchRepository = RepositoryFactory.CreateMatchRepository())
            {
                var matches = matchRepository.GetByMatchDay(matchDay);
                return matches;
            }
        }

        public Match GetByMatchDayAndTeam(DateTime matchDay, string teamId)
        {
            using (var matchRepository = RepositoryFactory.CreateMatchRepository())
            {
                var match = matchRepository.GetByMatchDayAndTeam(matchDay, teamId);
                return match;
            }
        }

        public void PlayMatchDay(DateTime matchDate)
        {
            using (var transactionManager = RepositoryFactory.CreateTransactionManager())
            {
                PlayMatchDay(matchDate, transactionManager);
                transactionManager.Save();
            }
        }

        public void PlayMatchDay(DateTime matchDate, TransactionManager transactionManager)
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

            transactionManager.RegisterUpdate(matchesToPlay);

            // After matches have been played a lot of stuff must be determined and updated.
            var postMatchManager = new PostMatchManager(transactionManager, RepositoryFactory);
            postMatchManager.Handle(currentSeason.Id, matchesToPlay);
        }

        public IEnumerable<TeamRoundMatch> GetTeamRoundMatches(string teamId, string seasonId, string leagueCompetitionId)
        {
            using (var matchRepository = RepositoryFactory.CreateMatchRepository())
            using (var competitionRepository = new RepositoryFactory().CreateCompetitionRepository())
            {
                var matches = matchRepository.GetTeamRoundMatches(GameInfo.Id, teamId, seasonId);

                // For league competitions only the team's current league must be included.
                var leagueCompetitionsToSkip = competitionRepository.GetLeagues().Where(c => c.Id != leagueCompetitionId).Select(c => c.Id);
                return matches.Where(m => !leagueCompetitionsToSkip.Contains(m.CompetitionId));
            }
        }
    }
}
