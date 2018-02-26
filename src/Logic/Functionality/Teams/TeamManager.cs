using System.Collections.Generic;
using System.Linq;
using Randomization;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Teams
{
   internal class TeamManager
   {
      private ListRandomizer _listRandomizer;
      private DatabaseRepositoryFactory _repositoryFactory;
      private TeamGenerator _teamGenerator;

      public TeamManager(DatabaseRepositoryFactory repositoryFactory)
      {
         _repositoryFactory = repositoryFactory;
         _teamGenerator = new TeamGenerator();
         _listRandomizer = new ListRandomizer();
      }

      public IEnumerable<Team> Create(int howMany)
      {
         var teams = new List<Team>();

         using (var formationRepository = new MemoryRepositoryFactory().CreateFormationRepository())
         {
            var formations = formationRepository.GetAll();
            bool teamGenerationReady = false;
            while (!teamGenerationReady)
            {
               var team = _teamGenerator.Generate();

               // Team names must be unique.
               bool teamExists = teams.Any(t => t.Name == team.Name);
               if (!teamExists)
               {
                  team.Formation = _listRandomizer.GetItem(formations);
                  teams.Add(team);
               }

               teamGenerationReady = (teams.Count == howMany);
            }
         }

         return teams;
      }

      public void UpdateRating(Team team, IEnumerable<Player> squad)
      {
         decimal teamRating = TeamRater.GetRating(squad);
         team.Rating = teamRating;
      }

      public void UpdateRating(Team team)
      {
         IEnumerable<Player> players;
         using (var playerRepository = _repositoryFactory.CreatePlayerRepository())
         {
            players = playerRepository.GetPlayersByTeam(team, false);
         }

         UpdateRating(team, players);
      }
   }
}
