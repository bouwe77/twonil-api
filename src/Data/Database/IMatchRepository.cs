using System;
using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Database
{
   public interface IMatchRepository : IRepository<Match>
   {
      IEnumerable<Match> GetByMatchDay(DateTime matchDay);
      Match GetByMatchDayAndTeam(DateTime matchDay, string teamId);
      IEnumerable<Match> GetByMatchStatus(string seasonId, MatchStatus matchStatus);
      IEnumerable<Match> GetByRound(string roundId);
      IEnumerable<Match> GetBySeason(string seasonId);
      IEnumerable<Match> GetBySeasonAndTeam(string seasonId, string teamId);
      IEnumerable<Match> GetMatchesBetweenTeams(SeasonCompetition seasonCompetition, string team1Id, string team2Id);
      DateTime? GetNextMatchDate(string seasonId);
      IEnumerable<TeamRoundMatch> GetTeamRoundMatches(string gameId, string teamId, string seasonId);
      Match GetMatch(string matchId);
   }
}