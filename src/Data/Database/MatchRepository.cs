using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Database
{
   public class MatchRepository : ReadRepository<Match>, IMatchRepository
   {
      internal MatchRepository(string databaseFilePath, string gameId)
         : base(databaseFilePath, gameId)
      {
      }

      public Match GetMatch(string matchId)
      {
         var match = GetOne(matchId);
         if (match != null)
         {
            GetReferencedData(match);
         }

         return match;
      }

      public IEnumerable<Match> GetBySeason(string seasonId)
      {
         var matches = Find(match => match.SeasonId.Equals(seasonId));

         foreach (var match in matches)
         {
            GetReferencedData(match);
         }

         return matches;
      }

      public IEnumerable<Match> GetBySeasonAndTeam(string seasonId, string teamId)
      {
         var matches = Find(
            match => match.SeasonId.Equals(seasonId)
                     && (match.HomeTeamId != null && match.HomeTeamId.Equals(teamId)
                         || match.AwayTeamId != null && match.AwayTeamId.Equals(teamId)));

         foreach (var match in matches)
         {
            GetReferencedData(match);
         }

         return matches;
      }

      public DateTime? GetNextMatchDate(string seasonId)
      {
         DateTime? nextMatchDay = null;

         var matches = GetByMatchStatus(seasonId, MatchStatus.NotStarted).ToList();
         if (matches.Any())
         {
            nextMatchDay = matches.Min(match => match.Date);
         }

         return nextMatchDay;
      }

      public IEnumerable<Match> GetByMatchDay(DateTime matchDay)
      {
         var matches = Find(match => match.Date.Equals(matchDay));
         foreach (var match in matches)
         {
            GetReferencedData(match);
         }

         return matches;
      }

      public Match GetByMatchDayAndTeam(DateTime matchDay, string teamId)
      {
         var match = Find(m => m.Date.Equals(matchDay) && (m.HomeTeamId == teamId || m.AwayTeamId == teamId)).SingleOrDefault();

         if (match != null)
         {
            GetReferencedData(match);
         }

         return match;
      }

      public IEnumerable<Match> GetByRound(string roundId)
      {
         var matches = Find(match => match.RoundId == roundId);
         foreach (var match in matches)
         {
            GetReferencedData(match);
         }

         return matches;
      }

      private void GetReferencedData(Match match)
      {
         var repositoryFactory = new DatabaseRepositoryFactory(match.GameId);

         using (var teamRepository = repositoryFactory.CreateTeamRepository())
         {
            if (!string.IsNullOrWhiteSpace(match.HomeTeamId))
            {
               var homeTeam = teamRepository.GetTeam(match.HomeTeamId);
               match.HomeTeam = homeTeam;
            }

            if (!string.IsNullOrWhiteSpace(match.AwayTeamId))
            {
               var awayTeam = teamRepository.GetTeam(match.AwayTeamId);
               match.AwayTeam = awayTeam;
            }
         }

         using (var roundRepository = repositoryFactory.CreateRepository<Round>())
         {
            var round = roundRepository.GetOne(match.RoundId);
            match.Round = round;
         }
      }

      public IEnumerable<Match> GetByMatchStatus(string seasonId, MatchStatus matchStatus)
      {
         var matches = Find(match => match.SeasonId == seasonId && match.MatchStatus == matchStatus);
         return matches;
      }

      public IEnumerable<Match> GetMatchesBetweenTeams(SeasonCompetition seasonCompetition, string team1Id, string team2Id)
      {
         var matches = Find(m =>
            m.SeasonId == seasonCompetition.SeasonId
            && m.CompetitionId == seasonCompetition.CompetitionId
            && (m.HomeTeamId == team1Id && m.AwayTeamId == team2Id
                || m.HomeTeamId == team2Id && m.AwayTeamId == team1Id));

         var matchesBetweenTeams = matches.ToList();
         foreach (var match in matchesBetweenTeams)
         {
            GetReferencedData(match);
         }

         return matchesBetweenTeams;
      }

      public IEnumerable<TeamRoundMatch> GetTeamRoundMatches(string gameId, string teamId, string seasonId)
      {
         const string sql = @"
            SELECT r.MatchDate, r.CompetitionId, r.CompetitionName, r.Name AS RoundName, m.Id AS MatchId, m.HomeTeamId, m.AwayTeamId, 
            m.HomeScore, m.AwayScore, m.PenaltiesTaken, m.HomePenaltyScore, m.AwayPenaltyScore, m.MatchStatus
            FROM rounds r 
            LEFT OUTER JOIN matches m 
            ON (m.RoundId = r.Id AND (m.HomeTeamId = ? OR m.AwayTeamId = ?))
            WHERE r.SeasonId = ?
            ORDER BY r.MatchDate ASC";

         var matches = Connection.Query<TeamRoundMatch>(sql, teamId, teamId, seasonId);

         foreach (var match in matches)
         {
            match.GameId = gameId;
            GetReferencedData(match);
         }

         return matches;
      }

      private void GetReferencedData(TeamRoundMatch teamRoundMatch)
      {
         var repositoryFactory = new DatabaseRepositoryFactory(teamRoundMatch.GameId);

         using (var teamRepository = repositoryFactory.CreateTeamRepository())
         {
            if (!string.IsNullOrWhiteSpace(teamRoundMatch.HomeTeamId))
            {
               var homeTeam = teamRepository.GetTeam(teamRoundMatch.HomeTeamId);
               teamRoundMatch.HomeTeam = homeTeam;
            }

            if (!string.IsNullOrWhiteSpace(teamRoundMatch.AwayTeamId))
            {
               var awayTeam = teamRepository.GetTeam(teamRoundMatch.AwayTeamId);
               teamRoundMatch.AwayTeam = awayTeam;
            }
         }
      }
   }
}
