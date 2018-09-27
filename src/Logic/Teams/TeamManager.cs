using System.Collections.Generic;
using System.Linq;
using Randomization;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Teams
{
   public class TeamManager
   {
      private readonly ListRandomizer _listRandomizer;
      private readonly RepositoryFactory _repositoryFactory;

      public TeamManager(RepositoryFactory repositoryFactory)
      {
         _repositoryFactory = repositoryFactory;
         _listRandomizer = new ListRandomizer();
      }

      public IEnumerable<Team> Create(int howMany)
      {
         var teams = new List<Team>();

         using (var formationRepository = new RepositoryFactory().CreateFormationRepository())
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

      public void UpdateRating(Team team, List<Player> squad)
      {
         var teamRating = TeamRater.GetRating(squad);

         team.Rating = teamRating.ratingTeam;
         team.RatingGoalkeeper = teamRating.ratingGoalkeeper;
         team.RatingDefence = teamRating.ratingDefence;
         team.RatingMidfield = teamRating.ratingMidfield;
         team.RatingAttack = teamRating.ratingAttack;
      }

      public void UpdateRating(Team team)
      {
         IEnumerable<Player> players;
         using (var playerRepository = _repositoryFactory.CreatePlayerRepository())
         {
            players = playerRepository.GetPlayersByTeam(team, false).Where(p => p.InStartingEleven);
         }

         UpdateRating(team, players.ToList());
      }
   }
}
