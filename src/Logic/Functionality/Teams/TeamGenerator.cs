using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Teams
{
   internal class TeamGenerator
   {
      private readonly TeamNameGenerator _teamNameGenerator = new TeamNameGenerator();

      public Team Generate()
      {
         string teamName = _teamNameGenerator.GetTeamName();
         
         var team = new Team { Name = teamName };
         
         return team;
      }
   }
}
