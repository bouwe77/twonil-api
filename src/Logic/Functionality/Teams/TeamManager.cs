using System.Collections.Generic;
using System.Linq;
using Randomization;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Teams
{
   internal class TeamManager
   {
      private readonly ListRandomizer _listRandomizer;
      private readonly DatabaseRepositoryFactory _repositoryFactory;

      public TeamManager(DatabaseRepositoryFactory repositoryFactory)
      {
         _repositoryFactory = repositoryFactory;
         _listRandomizer = new ListRandomizer();
      }

      public IEnumerable<Team> Create(int howMany)
      {
         var teams = new List<Team>();

         using (var formationRepository = new MemoryRepositoryFactory().CreateFormationRepository())
         {
            var formations = formationRepository.GetAll().ToList();

            for (int i = 0; i < howMany; i++)
            {
               var team = new Team { Formation = _listRandomizer.GetItem(formations) };
               teams.Add(team);
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
