using System.Collections.Generic;
using System.Linq;
using Randomization;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Teams
{
    public class TeamManager
    {
        private readonly IListRandomizer _listRandomizer;
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly TeamRater _teamRater;

        public TeamManager(IUnitOfWorkFactory uowFactory, TeamRater teamRater, IListRandomizer listRandomizer)
        {
            _uowFactory = uowFactory;
            _teamRater = teamRater;
            _listRandomizer = listRandomizer;
        }

        public IEnumerable<Team> Create(int howMany)
        {
            var teams = new List<Team>();

            using (var uow = _uowFactory.Create())
            {
                var formations = uow.Formations.GetAll().ToList();

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
            var teamRating = _teamRater.GetRating(squad);

            team.Rating = teamRating.ratingTeam;
            team.RatingGoalkeeper = teamRating.ratingGoalkeeper;
            team.RatingDefence = teamRating.ratingDefence;
            team.RatingMidfield = teamRating.ratingMidfield;
            team.RatingAttack = teamRating.ratingAttack;
        }

        public void UpdateRating(Team team)
        {
            IEnumerable<Player> players;
            using (var uow = _uowFactory.Create())
            {
                players = uow.Players.GetPlayersByTeam(team, false).Where(p => p.InStartingEleven);
            }

            UpdateRating(team, players.ToList());
        }
    }
}
