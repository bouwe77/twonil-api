
//TODO EF/SQL migratie: Hier is een CUSTOM QUERY!!!

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using TwoNil.Shared.DomainObjects;

//namespace TwoNil.Data.Repositories
//{
//    public class MatchRepository : ReadRepository<Match>, IMatchRepository
//    {
//        public IEnumerable<TeamRoundMatch> GetTeamRoundMatches(string gameId, string teamId, string seasonId)
//        {
//            const string sql = @"
//            SELECT r.MatchDate, r.CompetitionId, r.CompetitionName, r.Name AS RoundName, m.Id AS MatchId, m.HomeTeamId, m.AwayTeamId, 
//            m.HomeScore, m.AwayScore, m.PenaltiesTaken, m.HomePenaltyScore, m.AwayPenaltyScore, m.MatchStatus
//            FROM rounds r 
//            LEFT OUTER JOIN matches m 
//            ON (m.RoundId = r.Id AND (m.HomeTeamId = ? OR m.AwayTeamId = ?))
//            WHERE r.SeasonId = ?
//            ORDER BY r.MatchDate ASC";

//            var matches = Connection.Query<TeamRoundMatch>(sql, teamId, teamId, seasonId);

//            foreach (var match in matches)
//            {
//                match.GameId = gameId;
//                GetReferencedData(match);
//            }

//            return matches;
//        }
//    }
//}
