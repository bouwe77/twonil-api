using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services.Interfaces
{
   public interface ITeamService
   {
      IEnumerable<Team> GetAll();
      IEnumerable<Team> GetBySeasonCompetition(SeasonCompetition seasonCompetition);
      Team GetMyTeam();
      Team GetTeam(string teamId);
   }
}
