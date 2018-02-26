using System;
using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   public interface IMatchService
   {
      IEnumerable<Match> GetBySeasonAndTeam(string seasonId, string teamId);
      IEnumerable<Match> GetByMatchDay(DateTime matchDay);
      Match GetByMatchDayAndTeam(DateTime matchDay, string teamId);
      IEnumerable<Match> GetByRound(Round round);
      Match Get(Match match);
      void PlayMatchDay(DateTime matchDay);
      DateTime? GetNextMatchDay(string seasonId);
      IEnumerable<TeamRoundMatch> GetTeamRoundMatches(string teamId, string seasonId, string leagueCompetitionId);
   }
}
